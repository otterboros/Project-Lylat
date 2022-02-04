using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotCollisionHandler : MonoBehaviour
{
    private GameObject parentGameObject;

    private static GameObject _linkedReticle;

    private void Start()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
    }

    public static void AssignLinkedReticle(GameObject linkedReticle)
    {
        _linkedReticle = linkedReticle;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerControls.ResetChargedShotBools();

        Instantiate(Resources.Load<GameObject>("Prefabs/ChargedShotExplosion"), transform.position, Quaternion.identity, parentGameObject.transform);

        Destroy(_linkedReticle);
        Destroy(gameObject);
    }
}
