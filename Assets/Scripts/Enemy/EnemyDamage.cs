using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour, IDamagable
{
    Scoreboard scoreboard;
    GameObject parentGameObject;

    private EnemyData _data;
    public int currentHealth { get; set; }

    private RigidBodyAddition _rb;

    void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        // Add a rigidbody to this gameobject.
        _rb = GetComponent<RigidBodyAddition>();
        _rb.AddRigidBody();

        // Set current health for this game object as it's stored max health
        _data = GetComponent<EnemyData>();
        currentHealth = _data.maxHealth;
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

            Instantiate(Resources.Load<GameObject>("Prefabs/FX/HitVFX"), transform.position, Quaternion.identity, parentGameObject.transform);
            return;
        }

        // If health is below 1, process death.
        else if (health < 1)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/EnemyExplosionVFX&SFX"), transform.position, Quaternion.identity, parentGameObject.transform);

            // Update player score based on score value of killed enemy
            scoreboard.ModifyScore(_data.scoreValue);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Right now, "Weapon" refers only to the ChargedShot and PlayerLaserBullet
        if (other.gameObject.CompareTag("PlayerLaser"))
        {
            Debug.Log($"{transform.name} was hit by {other.transform.name}");
            TakeDamage(other.GetComponent<BulletData>().shotDamage);
            ProcessHealthState(currentHealth);
        }
        else
            Debug.Log($"{transform.name} was hit by trigger collider {other.gameObject.name}");
    }
}