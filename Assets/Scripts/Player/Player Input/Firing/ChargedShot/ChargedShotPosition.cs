// ChargedShotPosition.cs - Update position of charged shot based on targeting states
//-----------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotPosition : MonoBehaviour
{ 
    private GameObject playerShip;
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
        {
            Debug.Log("Error! Charged shot is being fired without a targeted enemy.");
            // This can happen if a ship is shot down before the player fires a shot at it.
            // Rare but possible, should account for this case by giving the charged shot a straight ahead state.
        }
    }
}
