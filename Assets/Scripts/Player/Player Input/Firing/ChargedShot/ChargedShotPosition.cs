using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotPosition : MonoBehaviour
{
    // Turn this into an abstract parent class!

    private GameObject playerShip;
    //private Transform laserOne;
    private ChargedShotData _data;
    private ObjectFiring _of;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip2");
        _data = playerShip.GetComponent<ChargedShotData>();
        _of = GetComponent<ObjectFiring>();
    }

    private void Update()
    {
        if (!ChargedShotData.isChargedShotFired)
        {
            transform.position = playerShip.transform.position + _data.chargedShotPositionOffset;
        }
        else if (ChargedShotData.isChargedShotFired && ChargedShotData.enemyTargeted)
            _of.FireHomingObjectAtTarget(ChargedShotData.enemyTargeted.transform, _data.chargedShotSpeed);
        else if (ChargedShotData.isChargedShotFired && !ChargedShotData.enemyTargeted)
            Debug.Log("Error! Charged shot is being fired without a targeted enemy.");
    }
}
