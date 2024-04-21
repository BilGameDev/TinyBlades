using System.Collections;
using System.Collections.Generic;
using Epic.OnlineServices.Lobby;
using Mirror;
using UnityEngine;

// A custom script that makes use of the EOS relay and lobby system
// more information on https://github.com/FakeByte/EpicOnlineTransport
public class LobbySystem : EOSLobby
{
    #region Fields
    public Transform LobbyHolder;
    public GameObject LobbyUI;
    public GameObject LobbyCanvas;
    public GameObject LeaveCanvas;

    private const string LOBBYNAME = "My Lobby";
    private List<LobbyDetails> _foundLobbies = new List<LobbyDetails>();

    #endregion

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

        GetComponent<NetworkManager>().StartHost();
        LobbyCanvas.SetActive(false);
        LeaveCanvas.SetActive(true);
    }

    //when the user joined the lobby successfully, set network address and connect
    private void OnJoinLobbySuccess(List<Attribute> attributes)
    {
        NetworkManager netManager = GetComponent<NetworkManager>();
        netManager.networkAddress = attributes.Find((x) => x.Data.Key == hostAddressKey).Data.Value.AsUtf8;
        netManager.StartClient();
        LobbyCanvas.SetActive(false);
        LeaveCanvas.SetActive(true);

    }

    //callback for FindLobbiesSucceeded
    private void OnFindLobbiesSuccess(List<LobbyDetails> lobbiesFound)
    {
        _foundLobbies = lobbiesFound;
        PopulateButtons();
    }

    //when the lobby was left successfully, stop the host/client
    private void OnLeaveLobbySuccess()
    {
        NetworkManager netManager = GetComponent<NetworkManager>();
        netManager.StopHost();
        netManager.StopClient();
        LobbyCanvas.SetActive(true);
        LeaveCanvas.SetActive(false);
    }

    public void CreateLobbyButton()
    {
        CreateLobby(4, LobbyPermissionLevel.Publicadvertised, false, new AttributeData[] { new AttributeData { Key = AttributeKeys[0], Value = LOBBYNAME }, });
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
        foreach (LobbyDetails lobby in _foundLobbies)
        {
            //get lobby name
            Attribute lobbyNameAttribute = new Attribute();
            lobby.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions { AttrKey = AttributeKeys[0] }, out lobbyNameAttribute);

            UIButtons scrollButton = Instantiate(LobbyUI, LobbyHolder).GetComponent<UIButtons>();

            scrollButton.LobbyName.text = lobbyNameAttribute.Data.Value.AsUtf8.Length > 30 ? lobbyNameAttribute.Data.Value.AsUtf8.Substring(0, 27).Trim() + "..." : lobbyNameAttribute.Data.Value.AsUtf8;
            scrollButton.PlayerNumber.text = lobby.GetMemberCount(new LobbyDetailsGetMemberCountOptions { }).ToString();
            scrollButton.JoinLobby.onClick.AddListener(() =>
            {
                JoinLobby(lobby, AttributeKeys);
            });
        }
    }

    public void PlayMode()
    {
        FindLobbies();
        if (_foundLobbies.Count > 0)
            JoinLobby(_foundLobbies[0]);
        else
            CreateLobby(4, LobbyPermissionLevel.Publicadvertised, false, new AttributeData[] { new AttributeData { Key = AttributeKeys[0], Value = LOBBYNAME }, });
    }
}
