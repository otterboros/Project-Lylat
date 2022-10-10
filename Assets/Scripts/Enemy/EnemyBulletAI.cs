using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletAI : BaseBulletAI
{
    private Camera _gameCamera;

    protected override void Awake()
    {
        base.Awake();
        _gameCamera = Camera.main;
    }

    protected override void Update()
    {
        _firing.SetFiringMode(_bulletData.firingMode, _bulletData.target, _bulletData.shotSpeed);
        _cleaner.DestroyThisBehindObject(_bulletData.shotDistToDestroy, _gameCamera.gameObject, this.gameObject);
    }
}
