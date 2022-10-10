using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletAI : BaseBulletAI
{
    private GameObject playerShip;
    private float startingPosZ;

    protected override void Awake()
    {
        base.Awake();

        playerShip = GameObject.Find("PlayerShip2");
        transform.rotation = playerShip.transform.rotation;
        startingPosZ = transform.position.z;
    }

    protected override void Update()
    {
        _firing.FireObjectForward(_bulletData.shotSpeed);
        _cleaner.DestroyAfterDistance(_bulletData.shotDistToDestroy, startingPosZ, transform.position.z);
    }
}
