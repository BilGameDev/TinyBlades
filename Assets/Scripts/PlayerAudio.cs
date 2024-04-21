using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAudio : NetworkBehaviour
{
    #region Fields
    public AudioClip WalkSound;
    public AudioClip SwordSwingSound;
    public AudioClip HitSound;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] IInputProvider _inputProvider;
    
    #endregion

    private void Awake()
    {
        _inputProvider = GetComponent<IInputProvider>();
    }


    // Sets up methods to be called for playing sound cues.
    // Once triggered, the signal is sent to the server which then plays that sound for all clients
    // This way everyone hears the same thing
    // Even in 3D space
    void Update()
    {
        if (!isLocalPlayer) return;

        if (_inputProvider.MovementInput.magnitude > 0.1f)
        {
            TryPlayWalkSound();
        }

        if (_inputProvider.AttackPressed)
        {
            TryPlaySwordSwingSound();
        }
    }

    [Client]
    public void TryPlayWalkSound()
    {
        if (!isLocalPlayer) return;
        if (_inputProvider.MovementInput.magnitude > 0.1f && !_audioSource.isPlaying)
        {
            CmdPlayWalkSound();
        }
    }

    [Client]
    public void TryPlaySwordSwingSound()
    {
        if (!isLocalPlayer) return;
        // input to trigger the sword swing
        CmdPlaySwordSwingSound();
    }

    [Client]
    public void TryPlayHitSound()
    {
        if (!isLocalPlayer) return;
        // input to trigger the hit
        CmdPlayHitSound();
    }

    [Command]
    private void CmdPlayWalkSound()
    {
        RpcPlayWalkSound();
    }

    [Command]
    private void CmdPlaySwordSwingSound()
    {
        RpcPlaySwordSwingSound();
    }

    [Command]
    private void CmdPlayHitSound()
    {
        RpcPlayHitSound();
    }

    [ClientRpc]
    private void RpcPlayWalkSound()
    {
        if (WalkSound && !_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(WalkSound);
        }
    }

    [ClientRpc]
    private void RpcPlaySwordSwingSound()
    {
        if (SwordSwingSound)
        {
            _audioSource.PlayOneShot(SwordSwingSound);
        }
    }

    [ClientRpc]
    private void RpcPlayHitSound()
    {
        if (HitSound)
        {
            _audioSource.PlayOneShot(HitSound);
        }
    }
}
