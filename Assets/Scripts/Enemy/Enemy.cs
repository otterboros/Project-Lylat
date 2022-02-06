using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] GameObject hitVFX;

    [SerializeField] int enemyScoreValue = 1;
    [SerializeField] int enemyHealth = 1;

    Scoreboard scoreboard;
    GameObject parentGameObject;

    void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
        AddRigidBody();
    }

    private void AddRigidBody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void OnParticleCollision(GameObject other)
    {
        switch (other.gameObject.tag)
        {
            case "EnemyWeapon":
                Debug.Log($"{transform.name} was hit by {other.transform.name}. Friendly fire!");
                break;
            case "Player":
                Damage(1, "Laser");
                Debug.Log($"{transform.name} was hit by player.");
                break;
            case "Friendly":
                Damage(1, "Laser");
                Debug.Log($"{transform.name} was hit by {other.transform.name}.");
                break;
            default:
                break;
        }
    }

    public void Damage(int damage, string weaponType)
    {
        enemyHealth -= damage;

        if (enemyHealth >= 1)
            ProcessEnemyDamage();

        else if (enemyHealth < 1)
        {
            if (weaponType == "ChargedShot")
                ComboManager.instance.AddToCombo();
            ProcessEnemyDeath();
        }   
    }

    void ProcessEnemyDamage()
    {
        // Play Damage sound
        // Play damage animation color
        // Play damage animation movement

        GameObject fx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        fx.transform.parent = parentGameObject.transform;
    }

    private void ProcessEnemyDeath()
    {
        scoreboard.ModifyScore(enemyScoreValue);

        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parentGameObject.transform;

        // Play Damage sound
        Destroy(gameObject);
    }
}
