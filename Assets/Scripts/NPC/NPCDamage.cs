using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDamage : BaseDamage
{

    protected override void Start()
    {
        base.Start();
        //HealthBarManager.instance.UpdateHealthBar(currentHealth);
    }

    #region Manage Health Changes
    public override void ChangeHealth(int value) // Change health value & play damage animation
    {
        // Remove damage from health
        currentHealth += value;
    }

    public override void ProcessHealthState(int health)
    {
        //if health is equal to or above 1, play damage effect.
        if (health >= 1)
        {
            HealthBarManager.instance.UpdateHealthBar(currentHealth, "NPC");
        }

        // If health is below 1, process death.
        else if (health < 1)
        {
            Debug.Log("NPC has died!");
        }
    }
    #endregion

    #region OnTriggerEnter
    protected override void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                // This will probably always be -1 but it should be a variable for damage taken by physically crashing into enemy.
                ChangeHealth(-1);
                ProcessHealthState(currentHealth);
                break;
            case "EnemyWeapon":
                ChangeHealth(other.GetComponent<BulletData>().shotDamage);
                // Call enemy damage bark!
                ProcessHealthState(currentHealth);
                break;
            case "PlayerLaser":
                ChangeHealth(other.GetComponent<BulletData>().shotDamage);
                // Call player damage bark!
                ProcessHealthState(currentHealth);
                break;
        }
    }
    #endregion
}
