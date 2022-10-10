// EnemyDamage.cs - Process OnTriggerEnter collision as damage that Enemies take
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : BaseDamage
{
    protected DamageAnimator _da;
    protected EnemyData _enemyData;

    public GameObject currentAttacker { get; set; }

    protected override void Awake()
    {
        // Replace this with just preassigned RBs.
        var rb = GetComponent<RigidBodyAddition>();
        rb.AddRigidBody();

        base.Awake();

        if (TryGetComponent<EnemyData>(out EnemyData enemyData)) { _enemyData = enemyData; }
        else { Debug.Log("Error! This enemy doesn't contain enemy Data!"); }

        // Get Damage Animator
        _da = GetComponent<DamageAnimator>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        currentAttacker = other.gameObject;

        // Process enemy laser damage, with charged shot damage processed in ChargedShotExplosion.cs
        if (other.gameObject.CompareTag("PlayerLaser") && _enemyData.isArmored == false)
        {
            Debug.Log($"{transform.name} was hit by {other.transform.name}");
            ChangeHealth(other.GetComponent<BulletData>().shotDamage);
            _da.StartAnimatingDamage();
            ProcessHealthState(currentHealth);
        }
        else if (other.gameObject.CompareTag("PlayerLaser") && _enemyData.isArmored == true)
        {
            // reflect lasers
            Debug.Log($"{transform.name} is Armored!");
        }
    }
    public override void ChangeHealth(int damage)
    {
        // Remove damage from health
        currentHealth += damage;
    }

    public override void ProcessHealthState(int health)
    {
        //if health is equal to or above 1, play damage effect.
        if (health >= 1)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/HitVFX"), transform.position, Quaternion.identity, _parentGameObject.transform);
            return;
        }

        // If health is below 1, process death.
        else if (health < 1)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/EnemyExplosionVFX&SFX"), transform.position, Quaternion.identity, _parentGameObject.transform);

            // Update player score based on score value of killed enemy
            _scoreboard.ModifyScore(_enemyData.scoreValue);

            // If the enemy should instantiate an item on death, instantiate it
            switch(_enemyData.spawnPickup)
            {
                case "PickupLaser":
                    Instantiate(Resources.Load<GameObject>("Prefabs/Pickups/PickupLaser"), transform.position, new Quaternion(0, 0, 0.382f, 0.923f), _parentGameObject.transform);
                    break;
                case "PickupHealth":
                    Instantiate(Resources.Load<GameObject>("Prefabs/Pickups/PickupHealth"), transform.position, new Quaternion(0, 0, 0.382f, 0.923f), _parentGameObject.transform);
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
        return _enemyData.isArmored;
    }
}