using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ship AI parent class
/// </summary>
public abstract class BaseAI : MonoBehaviour
{
    //NPCs and enemies both...
    // fire at targets
    // move away from projectiles and through paths
    // move
    // 

    protected Coroutine firing;
    protected bool isFiring { get { return firing != null; } }

    protected GameObject _bullet;
    protected BaseData _data;

    protected GameObject currentTarget;
    protected GameObject createAtRuntimeObj;


    protected virtual void Awake()
    {
        _data = GetComponent<BaseData>();
        createAtRuntimeObj = GameObject.FindWithTag("CreateAtRuntime");
        currentTarget = _data.target;
    }

    #region Spawn Bullets
    protected virtual void StartSpawningBullets()
    {
        // Instantiate Bullet & Assign new properties
        StopSpawningBullets(); //If this ship is firing, stop firing.
        firing = StartCoroutine(SpawnBullet());
    }

    protected virtual void StopSpawningBullets()
    {
        if (isFiring)
        {
            StopCoroutine(firing);
        }
        firing = null;
    }

    protected abstract IEnumerator SpawnBullet();

    protected virtual void SetBulletProperties()
    {
        BulletData _bd = _bullet.GetComponent<BulletData>();

        _bd.shotDamage = _data.shotDamage;
        _bd.shotSpeed = _data.shotSpeed;
        _bd.shotDistToDestroy = _data.shotDistToDestroy;
        _bd.target = currentTarget;
        _bd.firingMode = _data.firingMode;
    }
    #endregion

    #region UpdateTarget
    protected virtual void UpdateTarget(string newTarget)
    {
        currentTarget = GameObject.Find(newTarget);
    }
    #endregion
}