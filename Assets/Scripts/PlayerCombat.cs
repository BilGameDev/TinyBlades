using UnityEngine;
using Mirror;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] PlayerAudio playerAudio;

    // A SyncVar syncronizes the value across all clients, this means that all clients know what that particular player's health is
    [Header("Health")]
    [SyncVar(hook = nameof(OnHealthChanged))]
    public int health = 100;
    public int maxHealth = 100;
    public float respawnTime = 5.0f;

    [Header("Attack")]
    public float attackRange = 2f;
    public LayerMask enemyLayer;
    public int attackDamage = 10;

    [Header("VFX")]
    public ParticleSystem hitEffect;
    public ParticleSystem spawnSystem;

    [Header("UI")]
    public Image healthText;

    public bool isDead;
    public bool isHit;

    // This method is called using animation events, that makes it accurate to the action
    void Attack()
    {
        if (!isLocalPlayer) return;

        //On attack, a raycast is sent to see if a player was in attack range
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, enemyLayer))
        {
            //if yes, the attack data is sent to the server and a hit sound is played
            CmdDealDamage(attackDamage, hit.collider.gameObject);
            playerAudio.TryPlayHitSound();
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
        if (health <= 0) return;

        health -= damage;

        RpcHit();

        if (health <= 0)
        {
            RpcRespawn(); // Call respawn method on all clients.
        }
    }


    // This is called on all clients when health changes.
    void OnHealthChanged(int oldHealth, int newHealth)
    {
        Debug.Log($"Health updated: {newHealth}");

        if (isLocalPlayer)
            healthText.fillAmount = (float)health / 100;
    }

    [ClientRpc]
    void RpcHit()
    {
        isHit = true;
        hitEffect.Play();
    }

    [ClientRpc]
    void RpcRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        isDead = true;
        yield return new WaitForSeconds(respawnTime);

        if (isServer)
        {
            // Move the player to a spawn point or nearby location
            transform.position = NetworkManager.singleton.GetStartPosition().position;
            health = maxHealth; // Reset health for the respawned player.
        }

        isDead = false;
        spawnSystem.Play();
    }


    // This is called as an animation event to reset the isHit value
    void DisableHit()
    {
        isHit = false;
    }
}
