// EnemyAI.cs - Coroutine to instantiate enemy attack objects
// TO-DO: Optimize and move RotateToFaceTarget to its own script
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

    private GameObject enemyTargetParent;
    private GameObject player;
    private Transform target;

    private void Awake()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");

        _data = GetComponent<EnemyData>();

        enemyTargetParent = GameObject.Find("EnemyTargets");
        player = GameObject.Find("PlayerShip2");
    }

#region Spawn Bullets
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
    #endregion

    #region
    public void UpdateTarget(string newTarget)
    {
        _data.target = GameObject.Find(newTarget);
    }
    #endregion


    #region Rotate to Face Target

    private Coroutine rotating;
    private bool isRotating { get{ return rotating != null; } }

    public void SelectTarget(string targetName)
    {
        Debug.Log("Rotating to face target!");
        switch (targetName)
        {
            case "player": case "Player":
                target = player.transform;
                break;
            case "10":
                target = enemyTargetParent.transform.GetChild(1);
                break;
            default:
                Debug.Log("Error! Target was not assigned.");
                break;
        }
        StartRotatingToTarget();
    }

    public void StartRotatingToTarget()
    {
        StopRotatingToTarget(); //If this ship is rotating, stop rotating.
        rotating = StartCoroutine(RotateToTarget());
    }

    public void StopRotatingToTarget()
    {
        if (isRotating)
        {
            StopCoroutine(rotating);
        }
        rotating = null;
    }

    IEnumerator RotateToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _data.shipRotationSpeed);
            yield return new WaitForEndOfFrame();
        }
        StopRotatingToTarget();
    }
    #endregion
}
