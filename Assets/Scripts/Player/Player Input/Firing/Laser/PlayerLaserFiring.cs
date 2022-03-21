using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserFiring : MonoBehaviour
{
    Coroutine firing;
    bool isFiring { get { return firing != null; } }

    private GameObject parentGameObject;

    private PlayerData _data;

    private void Awake()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        _data = GetComponent<PlayerData>();
    }

    public void StartSpawningBullets()
    {
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
        while (true)
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Player/PlayerLaserBullet"), transform.position - new Vector3(0, 0, 0.08f), Quaternion.identity, parentGameObject.transform);
            bullet.GetComponent<BulletData>().shotDamage = _data.shotDamage;
            bullet.GetComponent<BulletData>().shotSpeed = _data.shotSpeed;
            bullet.GetComponent<BulletData>().distToDestroy = _data.distToDestroy;

            yield return new WaitForSeconds(_data.shotsPerSecond);
        }
    }
}
