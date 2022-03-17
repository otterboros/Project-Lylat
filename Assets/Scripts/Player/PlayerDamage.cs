using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerDamage : MonoBehaviour, IDamagable
{
    private GameObject parentGameObject;
    private PlayerData _data;

    public int currentHealth { get; set; }

    private void Start()
    {
        // Set current health for this game object as it's stored max health
        _data = GetComponent<PlayerData>();
        currentHealth = _data.maxHealth;
        HealthBarManager.instance.UpdateHealthBar(currentHealth);

        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
    }

    public void TakeDamage(int damage)
    {
        // Remove damage from health
        currentHealth -= damage;
    }

    public void ProcessHealthState(int health)
    {
        //if health is equal to or above 1, play damage effect.
        if (health >= 1)
        {
            HealthBarManager.instance.UpdateHealthBar(health);
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/HitVFX"), transform.position, Quaternion.identity, parentGameObject.transform);
        }

        // If health is below 1, process death.
        else if (health < 1)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/PlayerDeathVFX"), transform.position, Quaternion.identity, parentGameObject.transform);

            GetComponent<PlayerDeath>().DisablePlayerControls();
            GetComponent<ReloadLevel>().RestartLevel();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // Process collisions with trigger-enabled objects like enemy lasers (tagged Enemy Weapon) & enemy ships (tagged Enemy)
        switch (other.gameObject.tag)
        {
            case "NPC":
                Debug.Log("Player collided with an NPC.");
                break;
            case "Enemy":
                // This will probably always be 1 but it should be a variable for damage taken by physically crashing into enemy.
                TakeDamage(1);
                ProcessHealthState(currentHealth);
                break;
            case "EnemyWeapon":
                TakeDamage(other.GetComponent<BulletData>().shotDamage);
                ProcessHealthState(currentHealth);
                break;
            default:
                Debug.Log($"Player collided with {other.transform.name}.");
                break;
        }
    }
}
