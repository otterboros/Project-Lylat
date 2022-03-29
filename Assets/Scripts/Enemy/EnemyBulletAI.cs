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

    private void OnTriggerEnter(Collider other)
    {
        switch(other.transform.tag)
        {
            case "Environment": case "CollisionSafe": case "Player":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
