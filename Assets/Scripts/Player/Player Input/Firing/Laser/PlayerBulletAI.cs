using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletAI : MonoBehaviour
{
    private GameObject playerShip;

    private BulletData _data;
    private ObjectFiring _firing;
    private ObjectCleaner _cleaner;

    private float startingPosZ;

    private void Awake()
    {
        playerShip = GameObject.Find("PlayerShip2");
        transform.rotation = playerShip.transform.rotation;

        _data = GetComponent<BulletData>();
        _firing = GetComponent<ObjectFiring>();
        _cleaner = GetComponent<ObjectCleaner>();

        startingPosZ = transform.position.z;
    }

    private void Update()
    {
        _firing.FireObjectForward(_data.shotSpeed);
        _cleaner.DestroyAfterDistance(_data.distToDestroy, startingPosZ, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Environment": case "CollisionSafe": case "Enemy":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
