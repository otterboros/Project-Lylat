using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDamage : MonoBehaviour, IDamagable
{
    private GameObject _parentGameObject;
    private NPCData _data;
    private Rigidbody _rb;

    public int currentHealth { get; set; }

    private void Awake()
    {
        // Set current health for this game object as it's stored max health
        _data = GetComponent<NPCData>();
        _parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentHealth = _data.maxHealth;
        // Set NPC health
        //HealthBarManager.instance.UpdateHealthBar(currentHealth);
    }

    #region Manage Health Changes
    public void ChangeHealth(int value) // Change health value & play damage animation
    {
        // Remove damage from health
        currentHealth += value;
    }

    public void ProcessHealthState(int health)
    {
        //if health is equal to or above 1, play damage effect.
        if (health >= 1)
        {
            HealthBarManager.instance.UpdateHealthBar(currentHealth, "NPC");
        }

        // If health is below 1, process death.
        else if (health < 1)
        {
            Debug.Log("NPC has died!");
        }
    }
    #endregion

    #region OnTriggerEnter
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                // This will probably always be -1 but it should be a variable for damage taken by physically crashing into enemy.
                ChangeHealth(-1);
                ProcessHealthState(currentHealth);
                break;
            case "EnemyWeapon":
                ChangeHealth(other.GetComponent<BulletData>().shotDamage);
                // Call enemy damage bark!
                ProcessHealthState(currentHealth);
                break;
            case "PlayerLaser":
                ChangeHealth(other.GetComponent<BulletData>().shotDamage);
                // Call player damage bark!
                ProcessHealthState(currentHealth);
                break;
        }
    }
    #endregion
}
