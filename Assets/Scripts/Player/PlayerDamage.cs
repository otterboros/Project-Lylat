// PlayerDamage.cs - Handle collisions
//                   & process invincibility frames
// TO-DO: Combine Player and Enemy Damage into parent class
//        Move assignment of laserLevel and make laserLevel persist across levels
//        Add sounds and animation to Pickup
//-------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerDamage : BaseDamage
{
    #region Initalization

    protected Coroutine iFramesOn;
    protected bool areIFramesOn { get { return iFramesOn != null; } }

    protected PlayerData _playerData;

    protected override void Awake()
    {
        base.Awake();

        if (TryGetComponent<PlayerData>(out PlayerData playerData)) { _playerData = playerData; }
        else { Debug.Log("Error! This player game object doesn't have player data!"); }
    }

    protected override void Start()
    {
        base.Start();
        HealthBarManager.instance.UpdateHealthBar(currentHealth, "Player");
        PlayerDataStatic.laserLevel = 0;
    }

    #endregion

    #region Manage Health Changes
    public override void ChangeHealth(int value) // Change health value & play damage animation
    {
        currentHealth += value;

        // If player takes damage, play HitVFX
        if (value < 0)
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/HitVFX"), transform.position, Quaternion.identity, _parentGameObject.transform);
    }

    public override void ProcessHealthState(int health)
    {
        //if health is equal to or above 1, play damage effect.
        if (health >= 1)
        {
            HealthBarManager.instance.UpdateHealthBar(health, "Player");
        }

        // If health is below 1, process death.
        else if (health < 1)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/FX/PlayerDeathVFX"), transform.position, Quaternion.identity, _parentGameObject.transform);

            GetComponent<PlayerDeath>().DisablePlayerControls();
            GetComponent<ReloadLevel>().RestartLevel();
        }
    }
    #endregion

    #region OnTriggerEnter
    protected override void OnTriggerEnter(Collider other)
    {
        // Process trigger collisions if component PickupData is found
        if(other.gameObject.TryGetComponent(out PickupData _pd))
        {
            switch (other.gameObject.tag)
            {
                case "PickupHealth":
                    if (currentHealth < _playerData.maxHealth)
                    {
                        ChangeHealth(_pd.healthValue);
                        ProcessHealthState(currentHealth);
                    }
                    else if (currentHealth == _playerData.maxHealth)
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
    #endregion

    #region OnCollisionEnter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Environment" && !areIFramesOn)
        {
            ChangeHealth(-1);
            ProcessHealthState(currentHealth);
            StartInvincibilityFrames();
        }
    }
    #endregion

    #region Invincibility Frames
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
        while (ctr < _playerData.numOfIFrames)
        {
            ctr++;
            yield return new WaitForEndOfFrame();
        }
        StopInvincibilityFrames();
    }
    #endregion
}
