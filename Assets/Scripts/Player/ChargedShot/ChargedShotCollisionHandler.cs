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
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Friendly" || other.gameObject.tag == "EnemyWeapon")
            return;
        else
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/ChargedShotExplosion"), transform.position, Quaternion.identity, parentGameObject.transform);
            PlayerControls.ResetChargedShotStates();
        }
    }
}
