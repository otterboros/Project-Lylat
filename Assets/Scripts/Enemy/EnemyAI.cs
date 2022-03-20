// EnemyAI.cs - Coroutine to instantiate enemy attack objects
//-----------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Coroutine firing;
    bool isFiring { get { return firing != null; } }

    private GameObject parentGameObject;

    private GameObject _bullet;

    private EnemyData _data;

    private void Awake()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        _data = GetComponent<EnemyData>();
    }

    public void StartSpawningBullets()
    {
        // Instantiate Bullet & Assign new properties
        StopSpawningBullets(); //If this ship is firing, stop firing.
        firing = StartCoroutine(SpawnBullet());
    }

    public void StopSpawningBullets()
    {
        if (isFiring)
        {
            StopCoroutine(firing);
        }
        firing = null;
    }

    IEnumerator SpawnBullet()
    {
        while(true)
        {
            _bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttack/EnemyLaserBullet"), transform.position, Quaternion.identity, parentGameObject.transform);
            SetBulletProperties();
            yield return new WaitForSeconds(1/_data.shotsPerSecond);
        }
    }

    private void SetBulletProperties()
    {
        BulletData _bd = _bullet.GetComponent<BulletData>();

        _bd.shotDamage = _data.shotDamage;
        _bd.shotSpeed = _data.shotSpeed;
        _bd.distToDestroy = _data.distToDestroy;
        _bd.target = _data.target;
        _bd.firingMode = _data.firingMode;
    }
}
