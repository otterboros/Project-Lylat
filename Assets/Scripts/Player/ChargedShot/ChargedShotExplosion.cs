using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotExplosion : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] int chargedShotDamage = 2;

    public GameObject playerShip;

    private void Awake()
    {
        // Doing this once for each step of charged shot. Find a better way!
        // Also, this will screw things up if anyone other than player ship uses charged shot. :/
        playerShip = GameObject.Find("PlayerShip2");
    }
    private void OnEnable() => Explode();

    private void Explode()
    {
        // Add sound

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        ComboManager.instance.StartCombo();

        foreach (Collider c in colliders)
        {
            if(c.TryGetComponent(out EnemyDamage enemy))
            {
                enemy.TakeDamage(chargedShotDamage);
                enemy.ProcessHealthState(enemy.health);
                if (enemy.health < 1)
                    ComboManager.instance.AddToCombo();
            }
        }
        ComboManager.instance.FinishCombo();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
