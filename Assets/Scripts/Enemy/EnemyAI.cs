using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Coroutine firing;
    bool isFiring { get { return firing != null; } }

    private GameObject parentGameObject;

    private EnemyData _data;

    private void Awake()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        _data = GetComponent<EnemyData>();
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
        while(true)
        {
            switch (transform.name)
            {
                case "SingleShotEnemy":
                    GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttack/EnemyLaserBullet"), transform.position - new Vector3(0, 1.23000002f, 13.2600002f), Quaternion.identity, parentGameObject.transform);
                    bullet.GetComponent<BulletData>().shotDamage = _data.shotDamage;
                    bullet.GetComponent<BulletData>().shotSpeed = _data.shotSpeed;
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(_data.shotsPerSecond);
        }
    }
}
