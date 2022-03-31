// PlayerDamage.cs - Handle collisions
//                   & process invincibility frames
// TO-DO: Combine TakeDamage and Add Health into one function
//        Move assignment of laserLevel and make laserLevel persist across levels
//        Add sounds and animation to Pickup
//-------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerDamage : MonoBehaviour, IDamagable
{
    private GameObject _parentGameObject;
    private PlayerData _data;
    private Rigidbody _rb;
    private Scoreboard _scoreboard;

    public int currentHealth { get; set; }

    private Coroutine iFramesOn;
    private bool areIFramesOn { get { return iFramesOn != null; } }



    private void Awake()
    {
        // Set current health for this game object as it's stored max health
        _data = GetComponent<PlayerData>();
        _parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
        _rb = GetComponent<Rigidbody>();
        _scoreboard = FindObjectOfType<Scoreboard>();
    }

    private void Start()
    {
        currentHealth = _data.maxHealth;
        HealthBarManager.instance.UpdateHealthBar(currentHealth);
        PlayerDataStatic.laserLevel = 0;
    }

    public void ChangeHealth(int value) // Change health value & play damage animation
    {
        currentHealth += value;

        // If player takes damage, play HitVFX
        if (value < 0)
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/HitVFX"), transform.position, Quaternion.identity, _parentGameObject.transform);
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
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/PlayerDeathVFX"), transform.position, Quaternion.identity, _parentGameObject.transform);

            GetComponent<PlayerDeath>().DisablePlayerControls();
            GetComponent<ReloadLevel>().RestartLevel();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // Process trigger collisions if component PickupData is found and whether or not IFrames are off
        if(other.gameObject.TryGetComponent(out PickupData _pd))
        {
            switch (other.gameObject.tag)
            {
                case "PickupHealth":
                    if (currentHealth < _data.maxHealth)
                    {
                        ChangeHealth(_pd.healthValue);
                        ProcessHealthState(currentHealth);
                    }
                    else if (currentHealth == _data.maxHealth)
                    {
                        _scoreboard.ModifyScore(_pd.scoreValue);
                    }
                    else
                    {
                        Debug.Log("Error! Player health is above maxhealth!");
                    }
                    // Add Sound
                    // Add Animation
                    Destroy(other.gameObject);
                    break;
                case "PickupLaser":
                    if (PlayerDataStatic.laserLevel < 1)
                    {
                        PlayerDataStatic.laserLevel += 1;
                    }
                    else if (PlayerDataStatic.laserLevel == 1)
                    {
                        _scoreboard.ModifyScore(_pd.scoreValue);
                    }
                    else
                    {
                        Debug.Log("Error! Player laser level is above 1!");
                    }
                    // Add Sound
                    // Add Animation
                    Destroy(other.gameObject);
                    break;
            }
        }

        // Process trigger collisions if IFrames are off
        if(!areIFramesOn)
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
        if (!areIFramesOn)
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
        iFramesOn = StartCoroutine(InvincibilityFramesOn());
    }

    private void StopInvincibilityFrames()
    {
        if (areIFramesOn)
        {
            StopCoroutine(iFramesOn);
        }
        iFramesOn = null;
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
