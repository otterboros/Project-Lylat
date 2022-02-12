using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargeting : MonoBehaviour
{
    GameObject target;

    private BulletData _data;
    public int shotDamage;
    private ObjectFiring _firing;

    private void Awake()
    {
        target = GameObject.Find("PlayerShip2");
        transform.LookAt(target.transform);

        _data = GetComponent<BulletData>();
        shotDamage = _data.shotDamage;

        _firing = GetComponent<ObjectFiring>();
    }

    private void Update()
    {
        _firing.FireObjectForward(_data.shotSpeed);
    }
}
