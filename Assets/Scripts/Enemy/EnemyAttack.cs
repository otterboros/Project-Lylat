using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    GameObject target;
    Camera gameCamera;

    public int bulletDamage;
    public float bulletSpeed;

    private void Awake()
    {
        target = GameObject.Find("PlayerShip2");

        gameCamera = Camera.main;

        transform.LookAt(target.transform);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed);

        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (transform.position.z < gameCamera.transform.position.z - 1)
            Destroy(gameObject);
    }
}
