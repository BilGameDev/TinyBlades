using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerKeyboard : NetworkBehaviour, IInputProvider
{
    //Keyboard Input for PC
    public float Horizontal => Input.GetAxis("Horizontal");

    public float Vertical => Input.GetAxis("Vertical");

    public bool AttackPressed => Input.GetKeyDown(KeyCode.E);

    public Vector3 MovementInput => movement;

    public bool IsAttacking => attack;

    private Vector3 movement;
    private bool attack;

    void Update()
    {
        if (!isLocalPlayer) return;

        attack = AttackPressed;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        movement = new Vector3(Horizontal, 0.0f, Vertical).normalized;
    }
}
