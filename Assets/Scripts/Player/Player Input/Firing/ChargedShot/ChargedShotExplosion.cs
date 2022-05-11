using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotExplosion : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float timeTilDestroy = 1f;

    private Transform enemyTransform;

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
                if (!enemy.CheckIfArmored())
                {
                    enemy.ChangeHealth(ChargedShotData.chargedShotDamage);
                    enemy.ProcessHealthState(enemy.currentHealth);
                    if (enemy.currentHealth < 1)
                    {
                        ComboManager.instance.AddToCombo();
                        enemyTransform = enemy.transform;

                    }
                }

                else if (enemy.CheckIfArmored())
                {
                    var asc = c.GetComponent<ArmoredStateController>();
                    asc.DeactivateArmor();
                }
            }
        }
        ComboManager.instance.FinishCombo(enemyTransform);

        ChargedShotData.isChargedShotSequenceEnded = true;
        Destroy(this.gameObject, timeTilDestroy); // "this" is unnecessary, but helps me grok the code
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
