using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] IInputProvider inputProvider;

    private void Awake()
    {
        //Gets the attached Input controller which could either be Keyboard or Mobile
        inputProvider = GetComponent<IInputProvider>();
    }

    void Update()
    {
        //Sends the states to the server for it to assign them to the animator, the animations are then sync'd to all observers using the NetworkAnimator component
        if (isLocalPlayer)
        {
            CmdSetAnimation(inputProvider.MovementInput, inputProvider.AttackPressed, playerCombat.isDead, playerCombat.isHit);
        }
    }

    [Command]
    void CmdSetAnimation(Vector3 movement, bool combat, bool dead, bool hit)
    {
        animator.SetBool("isMoving", movement.magnitude > 0);
        animator.SetBool("inCombat", combat);
        animator.SetBool("isDead", dead);
        animator.SetBool("isHit", hit);
    }
}
