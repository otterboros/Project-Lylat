using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float triggerRangeZ;

    [SerializeField] int enemyDamage;
    [SerializeField] float weaponSpeed;
    [SerializeField] string weaponType;

    private Transform target;

    private int layerMask;
    private Collider[] colliders;
    private Vector3 boxOrigin = new Vector3();
    private Vector3 boxSize = new Vector3();

    private GameObject parentGameObject;

    private void Awake()
    {
        target = GameObject.Find("PlayerShip2").transform.Find("Collider").transform;

        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        // Bit shift layer mask to get a bit mask that only contains colliders in layer 3: Player
        layerMask = 1 << 3;
    }

    private void FixedUpdate()
    {
        TriggerEnemyAttack();

        DisableBehindPlayer();
    }

    private void TriggerEnemyAttack()
    {
        boxOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z - triggerRangeZ / 2);
        boxSize = new Vector3(300, 100, triggerRangeZ);
        colliders = Physics.OverlapBox(boxOrigin, boxSize/2, Quaternion.identity, layerMask);

        if (colliders.Length >= 1)
        {
            GetComponent<Animator>().SetBool("Attack", true);
        }
        else if (colliders.Length < 1)
        {
            GetComponent<Animator>().SetBool("Attack", false);
        }
            
    }

    public void FireAtTarget(string weaponType)
    {
        switch (weaponType)
        {
            case "SingleShot":
                GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/EnemyLaserBullet"), transform.position - new Vector3(0, 1.23000002f, 13.2600002f), Quaternion.identity, parentGameObject.transform);

                // Update this, rn it feels flawed. Gotta be a better way!
                bullet.GetComponent<EnemyAttack>().bulletDamage = enemyDamage;
                bullet.GetComponent<EnemyAttack>().bulletSpeed = weaponSpeed;
                break;
            default:
                break;
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Display the trigger range cube of the enemy ship
        boxOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z - triggerRangeZ / 2);
        boxSize = new Vector3(300, 100, triggerRangeZ);
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawCube(boxOrigin, boxSize);
    }

    private void DisableBehindPlayer()
    {
        // if a ship goes behind the player, disable it's attacks
        if(transform.position.z < target.transform.position.z - 1)
            GetComponent<Animator>().SetBool("Attack", false);
    }
}
