using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    [SerializeField] GameObject behindThisObject;
    [SerializeField] float distBehindObject = 0;

    Camera gameCamera;

    private void Start()
    {
        gameCamera = Camera.main;
    }

    private void Update()
    {
        DestroyThisBehindCamera(distBehindObject);
    }

    private void DestroyThisBehindCamera(float distBehindObject)
    {
        if (transform.position.z < gameCamera.transform.position.z - distBehindObject)
            Destroy(gameObject);
    }
}
