using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotExplosion : MonoBehaviour
{
    [SerializeField] float radius;

    private void OnEnable() => Explode();

    private void Explode()
    {
        // Add sound

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        ComboManager.instance.StartCombo();

        foreach (Collider c in colliders)
        {
            if(c.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage(2, "ChargedShot");
            }
        }
        ComboManager.instance.FinishCombo();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
