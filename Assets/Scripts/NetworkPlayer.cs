using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    #region Fields

    [SerializeField] public PlayerAnimator PlayerAnimator;

    [SerializeField] GameObject _cameraObject;
    [SerializeField] ParticleSystem _spawnEffect;
    [SerializeField] TextMeshPro _playerTitle;
    [SerializeField] GameObject _playerCanvas;

    private CameraFollow _camera;

    #endregion

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //We instantiate the camera when the  local player joins and set its target to this transform
        _camera = Instantiate(_cameraObject).GetComponent<CameraFollow>();
        _camera.SetTarget(transform);

        //Activate the player canvas
        _playerCanvas.SetActive(true);

        //Disable layers as to not accidently register self player for raycasts
        gameObject.tag = "Untagged";
        gameObject.layer = 0;
    }

    public override void OnStopLocalPlayer()
    {
        //Destroy the camera on player disconnect
        base.OnStopLocalPlayer();
        Destroy(_camera.gameObject);
    }

    public override void OnStopClient()
    {
        //A spoof effect is played on all clients
        base.OnStopClient();
        _spawnEffect.Play();
    }



    public override void OnStartClient()
    {
        base.OnStartClient();

        //Sets the title of players accordingly
        //A spoof effect is played on all clients
        _playerTitle.text = isLocalPlayer ? "Me" : "Player " + connectionToClient.connectionId.ToString();
        _spawnEffect.Play();
    }

}
