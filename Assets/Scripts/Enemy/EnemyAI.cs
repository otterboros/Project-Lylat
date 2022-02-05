using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float triggerRangeZ;

    private Transform target;
    private int layerMask;
    private Collider[] colliders;
    private Vector3 boxOrigin = new Vector3();
    private Vector3 boxSize = new Vector3();

    private void Awake()
    {
        target = GameObject.Find("PlayerShip2").transform;

        // Bit shift layer mask to get a bit mask that only contains colliders in layer 3: Player
        layerMask = 1 << 3;
    }

    private void FixedUpdate()
    {
        TriggerEnemyAttack();
    }

    private void TriggerEnemyAttack()
    {
        boxOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z - triggerRangeZ / 2);
        boxSize = new Vector3(300, 100, triggerRangeZ / 2);
        colliders = Physics.OverlapBox(boxOrigin, boxSize/2, Quaternion.identity, layerMask);

        if (colliders.Length >= 1)
        {
            Debug.Log($"{transform.name} fire at {target.name}!");
            GetComponent<Animator>().SetBool("Attack", true);
        }
        else if (colliders.Length < 1)
        {
            Debug.Log($"Hold your fire {transform.name}.");
            GetComponent<Animator>().SetBool("Attack", false);
        }
            
    }
    private void OnDrawGizmosSelected()
    {
        // Display the trigger range cube of the enemy ship
        boxOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z - triggerRangeZ / 2);
        boxSize = new Vector3(300, 100, triggerRangeZ / 2);
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawCube(boxOrigin, boxSize);
    }
}
