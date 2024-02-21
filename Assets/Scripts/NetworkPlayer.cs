using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] GameObject cameraObject;
    [SerializeField] ParticleSystem spawnEffect;
    [SerializeField] public PlayerAnimator playerAnimator;
    [SerializeField] TextMeshPro playerTitle;
    [SerializeField] GameObject playerCanvas;

    private CameraFollow Camera;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //We instantiate the camera when the  local player joins and set its target to this transform
        Camera = Instantiate(cameraObject).GetComponent<CameraFollow>();
        Camera.SetTarget(transform);

        //Activate the player canvas
        playerCanvas.SetActive(true);

        //Disable layers as to not accidently register self player for raycasts
        gameObject.tag = "Untagged";
        gameObject.layer = 0;
    }

    public override void OnStopLocalPlayer()
    {
        //Destroy the camera on player disconnect
        base.OnStopLocalPlayer();
        Destroy(Camera.gameObject);
    }

    public override void OnStopClient()
    {
        //A spoof effect is played on all clients
        base.OnStopClient();
        spawnEffect.Play();
    }



    public override void OnStartClient()
    {
        base.OnStartClient();

        //Sets the title of players accordingly
        //A spoof effect is played on all clients
        playerTitle.text = isLocalPlayer ? "Me" : "Player " + connectionToClient.connectionId.ToString();
        spawnEffect.Play();
    }

}
