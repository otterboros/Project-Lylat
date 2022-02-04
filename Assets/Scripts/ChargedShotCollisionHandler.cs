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
        switch (other.gameObject.tag)
        {
            case "Player":
                Debug.Log("This thing is the player!");
                break;
            default:
                Instantiate(Resources.Load<GameObject>("Prefabs/ChargedShotExplosion"), transform.position, Quaternion.identity, parentGameObject.transform);
                PlayerControls.ResetChargedShotStates();
                break;
        }
    }
}
