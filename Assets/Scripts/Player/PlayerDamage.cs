// PlayerDamage.cs - Handle collisions with trigger colliders
// TO-DO: Add collision with environment damage
//        Add invincibility frames (likely in another script)
//-----------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerDamage : MonoBehaviour, IDamagable
{
    private GameObject parentGameObject;
    private PlayerData _data;

    public int currentHealth { get; set; }

    private Coroutine _iFramesOn;
    private bool _areIFramesOn { get { return _iFramesOn != null; } }

    private Rigidbody _rb;

    private void Start()
    {
        // Set current health for this game object as it's stored max health
        _data = GetComponent<PlayerData>();
        currentHealth = _data.maxHealth;
        HealthBarManager.instance.UpdateHealthBar(currentHealth);

        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        _rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(int damage)
    {
        // Remove damage from health
        currentHealth -= damage;
        // Add invincibility frames
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
        if(!_areIFramesOn)
        {
            switch (other.gameObject.tag)
            {
                case "NPC":
                    Debug.Log("Player collided with an NPC.");
                    break;
                case "Enemy":
                    // This will probably always be 1 but it should be a variable for damage taken by physically crashing into enemy.
                    TakeDamage(1);
                    ProcessHealthState(currentHealth);
                    StartInvincibilityFrames();
                    break;
                case "EnemyWeapon":
                    TakeDamage(other.GetComponent<BulletData>().shotDamage);
                    ProcessHealthState(currentHealth);
                    StartInvincibilityFrames();
                    break;
                //case "Environment":
                //    // Run Script to process player interaction with environment
                //    break;
                //case "CollisionSafe":
                //    // Stop player movement but deal no damage
                //    break;
                default:
                    Debug.Log($"Player collided with {other.transform.name}.");
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!_areIFramesOn)
        {
            // Process collisions with non-trigger objects like terrain
            switch (collision.gameObject.tag)
            {
                case "Environment":
                    TakeDamage(1);
                    ProcessHealthState(currentHealth);
                    Debug.Log($"Player collided with environment object {collision.gameObject.tag}.");
                    StartInvincibilityFrames();
                    break;
                default:
                    Debug.Log($"Player collided with {collision.gameObject.tag}.");
                    break;
            }
        }
    }

    private void StartInvincibilityFrames()
    {
        StopInvincibilityFrames(); //If Invincibility Frames are already active, stop them.
        Debug.Log("Starting I Frames");
        _iFramesOn = StartCoroutine(InvincibilityFramesOn());
    }

    private void StopInvincibilityFrames()
    {
        Debug.Log("Stopping I Frames");
        if (_areIFramesOn)
        {
            StopCoroutine(_iFramesOn);
        }
        _iFramesOn = null;
    }

    IEnumerator InvincibilityFramesOn()
    {
        int ctr = 0;
        while (ctr < _data.numOfIFrames)
        {
            _rb.Sleep();
            ctr++;

            yield return new WaitForEndOfFrame();
        }

        _rb.WakeUp();

        StopInvincibilityFrames();
    }
}
