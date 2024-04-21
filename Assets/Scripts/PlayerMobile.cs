using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMobile : NetworkBehaviour, IInputProvider
{
    #region Fields
    
    //Mobile Input
    [Header("References")]
    public static PlayerMobile s_instance;
    [SerializeField] public GameObject MobileCanvas;
    [SerializeField] Joystick _joystick;
    [SerializeField] FixedButton _attackButton;

    public float Horizontal => _joystick.Horizontal;

    public float Vertical => _joystick.Vertical;

    public bool AttackPressed => _attackButton.ButtonDown;

    public Vector3 MovementInput => _movement;

    public bool IsAttacking => _attack;

    private Vector3 _movement;
    private bool _attack;

    #endregion

    void Update()
    {
        if (isLocalPlayer)
            _attack = AttackPressed;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        _movement = new Vector3(Horizontal, 0.0f, Vertical).normalized;
    }
}
