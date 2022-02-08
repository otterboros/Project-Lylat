using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotCollisionHandler : MonoBehaviour
{
    private GameObject parentGameObject;
    private GameObject playerShip;

    private void Start()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
        playerShip = GameObject.Find("PlayerShip2");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Friendly" || other.gameObject.tag == "EnemyWeapon")
            return;
        else
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Player/ChargedShot/ChargedShotExplosion"), transform.position, Quaternion.identity, parentGameObject.transform);
            playerShip.transform.GetComponent<PlayerControls>().ResetChargedShotStates();
        }
    }
}
