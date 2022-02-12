using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour, IDamagable
{
    Scoreboard scoreboard;
    GameObject parentGameObject;

    private EnemyData _data;
    public int health { get; set; }

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
        health = _data.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Remove damage from health
        health -= damage;
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

    void OnParticleCollision(GameObject other)
    {
        // Update lasers to have a damage value!
        // We'll have to turn them into non-particles I think.

        // Process collision with player or NPC particle colliders (lasers)
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            TakeDamage(1);
            ProcessHealthState(health); 
        }
        else
            Debug.Log($"{gameObject.transform.name} was hit by an unknown particle {other.gameObject.name}");
    }
}