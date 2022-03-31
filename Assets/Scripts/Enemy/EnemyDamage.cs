// EnemyDamage.cs - Process OnTriggerEnter collision as damage that Enemies take
//------------------------------------------------------------------------------

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
    private DamageAnimator _da;

    private void Awake()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        // Add a rigidbody to this gameobject.
        _rb = GetComponent<RigidBodyAddition>();
        _rb.AddRigidBody();

        // Set current health for this game object as it's stored max health
        _data = GetComponent<EnemyData>();
        currentHealth = _data.maxHealth;

        // Get Damage Animator
        _da = GetComponent<DamageAnimator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Process enemy laser damage, with charged shot damage processed in ChargedShotExplosion.cs
        if (other.gameObject.CompareTag("PlayerLaser") && _data.isArmored == false)
        {
            Debug.Log($"{transform.name} was hit by {other.transform.name}");
            ChangeHealth(other.GetComponent<BulletData>().shotDamage);
            _da.StartAnimatingDamage();
            ProcessHealthState(currentHealth);
        }
        else if (other.gameObject.CompareTag("PlayerLaser") && _data.isArmored == true)
        {
            // reflect lasers
            Debug.Log($"{transform.name} is Armored!");
        }
    }
    public void ChangeHealth(int damage)
    {
        // Remove damage from health
        currentHealth += damage;
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

            // If the enemy should instantiate an item on death, instantiate it
            switch(_data.spawnPickup)
            {
                case "PickupLaser":
                    Instantiate(Resources.Load<GameObject>("Prefabs/Pickups/PickupLaser"), transform.position, new Quaternion(0, 0, 0.382f, 0.923f), parentGameObject.transform);
                    break;
                case "PickupHealth":
                    Instantiate(Resources.Load<GameObject>("Prefabs/Pickups/PickupHealth"), transform.position, new Quaternion(0, 0, 0.382f, 0.923f), parentGameObject.transform);
                    break;
                default:
                    break;
            }

            // Destroy this enemy object
            Destroy(gameObject);
        }
    }

    public bool CheckIfArmored()
    {
        return _data.isArmored;
    }
}