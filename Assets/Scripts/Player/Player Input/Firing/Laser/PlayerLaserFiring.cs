// PlayerLaserFiring.cs - Coroutine to instantiate player's laser attacks
//---------------------------------------------------------------------------

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
            if (PlayerDataStatic.laserLevel == 0)
            {
                GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Player/PlayerLaserBullet"), transform.position - new Vector3(0, 0, 0.08f), Quaternion.identity, parentGameObject.transform);
                bullet.GetComponent<BulletData>().shotDamage = _data.shotDamage;
                bullet.GetComponent<BulletData>().shotSpeed = _data.shotSpeed;
                bullet.GetComponent<BulletData>().shotDistToDestroy = _data.distToDestroy;
            }
            else if (PlayerDataStatic.laserLevel == 1)
            {
                GameObject bullet1 = Instantiate(Resources.Load<GameObject>("Prefabs/Player/PlayerLaserBullet"), transform.position - new Vector3(-0.94f, 1.18f, 0.5f), Quaternion.identity, parentGameObject.transform);
                bullet1.GetComponent<BulletData>().shotDamage = _data.shotDamage;
                bullet1.GetComponent<BulletData>().shotSpeed = _data.shotSpeed;
                bullet1.GetComponent<BulletData>().shotDistToDestroy = _data.distToDestroy;

                GameObject bullet2 = Instantiate(Resources.Load<GameObject>("Prefabs/Player/PlayerLaserBullet"), transform.position - new Vector3(0.94f, 1.18f, 0.5f), Quaternion.identity, parentGameObject.transform);
                bullet2.GetComponent<BulletData>().shotDamage = _data.shotDamage;
                bullet2.GetComponent<BulletData>().shotSpeed = _data.shotSpeed;
                bullet2.GetComponent<BulletData>().shotDistToDestroy = _data.distToDestroy;
            }
            yield return new WaitForSeconds(_data.shotsPerSecond);
        }
    }
}
