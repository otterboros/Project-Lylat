using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryMover : MonoBehaviour
{
    private GameObject playerShip;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip2");
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3 (0,0,playerShip.transform.position.z);
    }
}
