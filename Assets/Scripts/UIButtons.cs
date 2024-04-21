using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    // This is a prefab button used to populate the scrollView when getting available lobbies for the EOS lobby system
    [SerializeField] public TextMeshProUGUI LobbyName;
    [SerializeField] public TextMeshProUGUI PlayerNumber;
    [SerializeField] public Button JoinLobby;
}
