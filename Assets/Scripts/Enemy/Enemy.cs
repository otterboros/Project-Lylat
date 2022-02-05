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
        Damage(1,"Laser");
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
