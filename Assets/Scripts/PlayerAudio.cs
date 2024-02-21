using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAudio : NetworkBehaviour
{
    [SerializeField] IInputProvider inputProvider;
    public AudioClip walkSound;
    public AudioClip swordSwingSound;
    public AudioClip hitSound;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        inputProvider = GetComponent<IInputProvider>();
    }


    // Sets up methods to be called for playing sound cues.
    // Once triggered, the signal is sent to the server which then plays that sound for all clients
    // This way everyone hears the same thing
    // Even in 3D space
    void Update()
    {
        if (!isLocalPlayer) return;

        if (inputProvider.MovementInput.magnitude > 0.1f)
        {
            TryPlayWalkSound();
        }

        if (inputProvider.AttackPressed)
        {
            TryPlaySwordSwingSound();
        }
    }

    [Client]
    public void TryPlayWalkSound()
    {
        if (!isLocalPlayer) return;
        if (inputProvider.MovementInput.magnitude > 0.1f && !audioSource.isPlaying)
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
        if (walkSound && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }

    [ClientRpc]
    private void RpcPlaySwordSwingSound()
    {
        if (swordSwingSound)
        {
            audioSource.PlayOneShot(swordSwingSound);
        }
    }

    [ClientRpc]
    private void RpcPlayHitSound()
    {
        if (hitSound)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}
