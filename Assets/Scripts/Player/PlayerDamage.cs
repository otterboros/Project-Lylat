// PlayerDamage.cs - Handle collisions
//                   & process invincibility frames
// TO-DO: Combine TakeDamage and Add Health into one function
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

    private Scoreboard scoreboard;

    private void Start()
    {
        // Set current health for this game object as it's stored max health
        _data = GetComponent<PlayerData>();
        currentHealth = _data.maxHealth;
        HealthBarManager.instance.UpdateHealthBar(currentHealth);

        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        _rb = GetComponent<Rigidbody>();

        scoreboard = FindObjectOfType<Scoreboard>();
    }

    public void ChangeHealth(int value) // Change health value & play damage animation
    {
        currentHealth += value;

        // If player takes damage, play HitVFX
        if (value < 0)
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/HitVFX"), transform.position, Quaternion.identity, parentGameObject.transform);
    }

    public void ProcessHealthState(int health)
    {
        //if health is equal to or above 1, play damage effect.
        if (health >= 1)
        {
            HealthBarManager.instance.UpdateHealthBar(health);
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
        // Process trigger collisions whether or not IFrames are off
        switch (other.gameObject.tag)
        {
            case "PickupHealth":
                var _pd = other.gameObject.GetComponent<PickupData>();
                if (currentHealth < _data.maxHealth)
                {
                    ChangeHealth(_pd.healthValue);
                    ProcessHealthState(currentHealth);
                }
                else if (currentHealth == _data.maxHealth)
                {
                    scoreboard.ModifyScore(_pd.scoreValue);
                }
                else
                {
                    Debug.Log("Error! Player health is above maxhealth!");
                }
                // Add Sound
                // Add Animation
                Destroy(other.gameObject);
                break;
        }

        // Process trigger collisions if IFrames are off
        if(!_areIFramesOn)
        {
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    // This will probably always be -1 but it should be a variable for damage taken by physically crashing into enemy.
                    ChangeHealth(-1);
                    ProcessHealthState(currentHealth);
                    StartInvincibilityFrames();
                    break;
                case "EnemyWeapon":
                    ChangeHealth(other.GetComponent<BulletData>().shotDamage);
                    ProcessHealthState(currentHealth);
                    StartInvincibilityFrames();
                    break;
                default:
                    Debug.Log($"Player collided with trigger {other.transform.name}.");
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Process non-trigger collisions if Iframes are off
        if (!_areIFramesOn)
        {
            switch (collision.gameObject.tag)
            {
                case "Environment":
                    ChangeHealth(1);
                    ProcessHealthState(currentHealth);
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
        _iFramesOn = StartCoroutine(InvincibilityFramesOn());
    }

    private void StopInvincibilityFrames()
    {
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
            ctr++;
            yield return new WaitForEndOfFrame();
        }
        StopInvincibilityFrames();
    }
}
