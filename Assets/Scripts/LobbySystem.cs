using System.Collections;
using System.Collections.Generic;
using Epic.OnlineServices.Lobby;
using Mirror;
using UnityEngine;

// A custom script that makes use of the EOS relay and lobby system
// more information on https://github.com/FakeByte/EpicOnlineTransport
public class LobbySystem : EOSLobby
{
    private string lobbyName = "My Lobby";

    private List<LobbyDetails> foundLobbies = new List<LobbyDetails>();
    private List<Attribute> lobbyData = new List<Attribute>();

    public Transform lobbyHolder;
    public GameObject lobbyUI;
    public GameObject lobbyCanvas;
    public GameObject leaveCanvas;

    //register events
    private void OnEnable()
    {
        //subscribe to events
        CreateLobbySucceeded += OnCreateLobbySuccess;
        JoinLobbySucceeded += OnJoinLobbySuccess;
        FindLobbiesSucceeded += OnFindLobbiesSuccess;
        LeaveLobbySucceeded += OnLeaveLobbySuccess;
    }

    //deregister events
    private void OnDisable()
    {
        //unsubscribe from events
        CreateLobbySucceeded -= OnCreateLobbySuccess;
        JoinLobbySucceeded -= OnJoinLobbySuccess;
        FindLobbiesSucceeded -= OnFindLobbiesSuccess;
        LeaveLobbySucceeded -= OnLeaveLobbySuccess;
    }

    //when the lobby is successfully created, start the host
    private void OnCreateLobbySuccess(List<Attribute> attributes)
    {
        lobbyData = attributes;

        GetComponent<NetworkManager>().StartHost();
        lobbyCanvas.SetActive(false);
        leaveCanvas.SetActive(true);
    }

    //when the user joined the lobby successfully, set network address and connect
    private void OnJoinLobbySuccess(List<Attribute> attributes)
    {
        lobbyData = attributes;
        NetworkManager netManager = GetComponent<NetworkManager>();
        netManager.networkAddress = attributes.Find((x) => x.Data.Key == hostAddressKey).Data.Value.AsUtf8;
        netManager.StartClient();
        lobbyCanvas.SetActive(false);
        leaveCanvas.SetActive(true);

    }

    //callback for FindLobbiesSucceeded
    private void OnFindLobbiesSuccess(List<LobbyDetails> lobbiesFound)
    {
        foundLobbies = lobbiesFound;
        PopulateButtons();
    }

    //when the lobby was left successfully, stop the host/client
    private void OnLeaveLobbySuccess()
    {
        NetworkManager netManager = GetComponent<NetworkManager>();
        netManager.StopHost();
        netManager.StopClient();
        lobbyCanvas.SetActive(true);
        leaveCanvas.SetActive(false);
    }

    public void CreateLobbyButton()
    {
        CreateLobby(4, LobbyPermissionLevel.Publicadvertised, false, new AttributeData[] { new AttributeData { Key = AttributeKeys[0], Value = lobbyName }, });
    }

    public void FindLobbyButton()
    {
        FindLobbies();
    }
    public void LeaveLobbyButton()
    {
        LeaveLobby();
    }

    public void PopulateButtons()
    {
        foreach (LobbyDetails lobby in foundLobbies)
        {
            //get lobby name
            Attribute lobbyNameAttribute = new Attribute();
            lobby.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions { AttrKey = AttributeKeys[0] }, out lobbyNameAttribute);

            UIButtons scrollButton = Instantiate(lobbyUI, lobbyHolder).GetComponent<UIButtons>();

            scrollButton.lobbyName.text = lobbyNameAttribute.Data.Value.AsUtf8.Length > 30 ? lobbyNameAttribute.Data.Value.AsUtf8.Substring(0, 27).Trim() + "..." : lobbyNameAttribute.Data.Value.AsUtf8;
            scrollButton.playerNumber.text = lobby.GetMemberCount(new LobbyDetailsGetMemberCountOptions { }).ToString();
            scrollButton.joinLobby.onClick.AddListener(() =>
            {
                JoinLobby(lobby, AttributeKeys);
            });
        }
    }

    public void PlayMode()
    {
        FindLobbies();
        if (foundLobbies.Count > 0)
            JoinLobby(foundLobbies[0]);
        else
            CreateLobby(4, LobbyPermissionLevel.Publicadvertised, false, new AttributeData[] { new AttributeData { Key = AttributeKeys[0], Value = lobbyName }, });
    }
}
