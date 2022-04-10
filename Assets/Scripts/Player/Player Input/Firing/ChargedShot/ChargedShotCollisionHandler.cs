using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotCollisionHandler : MonoBehaviour
{
    private GameObject parentGameObject;

    private void Start()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("CollisionSafe"))
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Player/ChargedShot/ChargedShotExplosion"), transform.position, Quaternion.identity, parentGameObject.transform);
        }
    }
}
