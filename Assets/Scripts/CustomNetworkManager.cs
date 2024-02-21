using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] GameObject leaveCanvas;
    [SerializeField] GameObject mainCanvas;

    //Disables the main canvas on Connect and displays it back on Disconnects
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        leaveCanvas.SetActive(true);
        mainCanvas.SetActive(false);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        leaveCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }
}

