using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletAI : MonoBehaviour
{
    private GameObject target;

    private BulletData _data;
    private ObjectFiring _firing;
    private ObjectCleaner _cleaner;

    private Camera gameCamera;

    private void Awake()
    {
        target = GameObject.Find("PlayerShip2");
        transform.LookAt(target.transform);

        _data = GetComponent<BulletData>();
        _firing = GetComponent<ObjectFiring>();
        _cleaner = GetComponent<ObjectCleaner>();

        gameCamera = Camera.main;
    }

    private void Update()
    {
        _firing.FireObjectForward(_data.shotSpeed);
        _cleaner.DestroyThisBehindObject(_data.distToDestroy, gameCamera.gameObject, this.gameObject);
    }
}
