using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    #region Fields
    [SerializeField] Animator _animator;
    [SerializeField] PlayerCombat _playerCombat;
    [SerializeField] IInputProvider _inputProvider;
    #endregion

    private void Awake()
    {
        //Gets the attached Input controller which could either be Keyboard or Mobile
        _inputProvider = GetComponent<IInputProvider>();
    }

    void Update()
    {
        //Sends the states to the server for it to assign them to the animator, the animations are then sync'd to all observers using the NetworkAnimator component
        if (isLocalPlayer)
        {
            CmdSetAnimation(_inputProvider.MovementInput, _inputProvider.AttackPressed, _playerCombat.IsDead, _playerCombat.IsHit);
        }
    }

    [Command]
    void CmdSetAnimation(Vector3 movement, bool combat, bool dead, bool hit)
    {
        _animator.SetBool("isMoving", movement.magnitude > 0);
        _animator.SetBool("inCombat", combat);
        _animator.SetBool("isDead", dead);
        _animator.SetBool("isHit", hit);
    }
}
