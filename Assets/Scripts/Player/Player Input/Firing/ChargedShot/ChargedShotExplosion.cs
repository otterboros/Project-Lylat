using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotExplosion : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float timeTilDestroy = 1f;

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
                enemy.TakeDamage(ChargedShotData.chargedShotDamage);
                enemy.ProcessHealthState(enemy.currentHealth);
                if (enemy.currentHealth < 1)
                    ComboManager.instance.AddToCombo();
            }
        }
        ComboManager.instance.FinishCombo();

        // "this" is unnecessary, but helps me grok the code below
        ChargedShotData.isChargedShotSequenceEnded = true;
        Destroy(this.gameObject, timeTilDestroy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
