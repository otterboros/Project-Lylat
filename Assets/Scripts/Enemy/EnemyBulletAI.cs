using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletAI : MonoBehaviour
{
    private BulletData _bd;
    private ObjectFiring _firing;
    private ObjectCleaner _cleaner;

    private Camera _gameCamera;

    private void Awake()
    {
        //target = GameObject.Find("PlayerShip2");
        //transform.LookAt(target.transform);

        _bd = GetComponent<BulletData>();
        _firing = GetComponent<ObjectFiring>();
        _cleaner = GetComponent<ObjectCleaner>();

        _gameCamera = Camera.main;
    }

    private void Update()
    {
        _firing.SetFiringMode(_bd.firingMode, _bd.target, _bd.shotSpeed);
        _cleaner.DestroyThisBehindObject(_bd.distToDestroy, _gameCamera.gameObject, this.gameObject);
    }
}
