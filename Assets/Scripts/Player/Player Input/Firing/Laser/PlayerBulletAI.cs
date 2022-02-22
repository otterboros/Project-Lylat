using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletAI : MonoBehaviour
{
    private GameObject playerShip;

    private BulletData _data;
    private ObjectFiring _firing;
    private ObjectCleaner _cleaner;

    private Camera gameCamera;


    private void Awake()
    {
        playerShip = GameObject.Find("PlayerShip2");
        transform.rotation = playerShip.transform.rotation;

        _data = GetComponent<BulletData>();
        _firing = GetComponent<ObjectFiring>();
        _cleaner = GetComponent<ObjectCleaner>();

        gameCamera = Camera.main;
    }

    private void Update()
    {
        _firing.FireObjectForward(_data.shotSpeed);
        _cleaner.DestroyThisAheadOfObject(_data.distFromCamera, gameCamera.gameObject);
    }
}
