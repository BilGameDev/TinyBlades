using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMobile : NetworkBehaviour, IInputProvider
{
    //Mobile Input
    [Header("References")]
    public static PlayerMobile instance;
    [SerializeField] public GameObject mobileCanvas;
    [SerializeField] Joystick joystick;
    [SerializeField] FixedButton attackButton;

    public float Horizontal => joystick.Horizontal;

    public float Vertical => joystick.Vertical;

    public bool AttackPressed => attackButton.ButtonDown;

    public Vector3 MovementInput => movement;

    public bool IsAttacking => attack;

    private Vector3 movement;
    private bool attack;

    void Update()
    {
        if (isLocalPlayer)
            attack = AttackPressed;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        movement = new Vector3(Horizontal, 0.0f, Vertical).normalized;
    }
}
