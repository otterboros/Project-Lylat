using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBulletAI : MonoBehaviour
{
    protected BulletData _bulletData;
    protected ObjectFiring _firing;
    protected ObjectCleaner _cleaner;

    protected virtual void Awake()
    {
        _bulletData = GetComponent<BulletData>();
        _firing = GetComponent<ObjectFiring>();
        _cleaner = GetComponent<ObjectCleaner>();
    }

    protected abstract void Update();

    protected virtual void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Environment":
            case "CollisionSafe":
            case "Enemy":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
