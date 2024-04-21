using UnityEngine;
using Mirror;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerCombat : NetworkBehaviour
{
    #region Fields

    // A SyncVar syncronizes the value across all clients, this means that all clients know what that particular player's health is
    [Header("Health")]
    [SyncVar(hook = nameof(OnHealthChanged))]
    public int Health = 100;
    public int MaxHealth = 100;
    public float RespawnTime = 5.0f;

    [Header("Attack")]
    public float AttackRange = 2f;
    public LayerMask EnemyLayer;
    public int AttackDamage = 10;

    [Header("VFX")]
    public ParticleSystem HitEffect;
    public ParticleSystem SpawnSystem;

    [Header("UI")]
    public Image HealthText;

    [SerializeField] PlayerAudio _playerAudio;

    public bool IsDead;
    public bool IsHit;

    #endregion

    // This method is called using animation events, that makes it accurate to the action
    void Attack()
    {
        if (!isLocalPlayer) return;

        //On attack, a raycast is sent to see if a player was in attack range
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, AttackRange, EnemyLayer))
        {
            //if yes, the attack data is sent to the server and a hit sound is played
            CmdDealDamage(AttackDamage, hit.collider.gameObject);
            _playerAudio.TryPlayHitSound();
        }

    }

    // This method is called by the attacker.
    [Command]
    public void CmdDealDamage(int damage, GameObject target)
    {
        // Target player takes damage.
        target.GetComponent<PlayerCombat>().TakeDamage(damage);
    }

    // This runs on the server but is called on the target's PlayerCombat.
    public void TakeDamage(int damage)
    {
        if (Health <= 0) return;

        Health -= damage;

        RpcHit();

        if (Health <= 0)
        {
            RpcRespawn(); // Call respawn method on all clients.
        }
    }


    // This is called on all clients when health changes.
    void OnHealthChanged(int oldHealth, int newHealth)
    {
        Debug.Log($"Health updated: {newHealth}");

        if (isLocalPlayer)
            HealthText.fillAmount = (float)Health / 100;
    }

    [ClientRpc]
    void RpcHit()
    {
        IsHit = true;
        HitEffect.Play();
    }

    [ClientRpc]
    void RpcRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        IsDead = true;
        yield return new WaitForSeconds(RespawnTime);

        if (isServer)
        {
            // Move the player to a spawn point or nearby location
            transform.position = NetworkManager.singleton.GetStartPosition().position;
            Health = MaxHealth; // Reset health for the respawned player.
        }

        IsDead = false;
        SpawnSystem.Play();
    }


    // This is called as an animation event to reset the isHit value
    void DisableHit()
    {
        IsHit = false;
    }
}
