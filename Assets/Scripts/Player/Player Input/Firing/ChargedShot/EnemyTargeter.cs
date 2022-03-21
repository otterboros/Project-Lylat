using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{
    private GameObject playerShip;
    private Camera gameCamera;
    private ChargedShotData _data;
    private GameObject uiCanvas;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip2");
        gameCamera = Camera.main;
        _data = playerShip.GetComponent<ChargedShotData>();
        uiCanvas = GameObject.Find("UI");
    }

    private void FixedUpdate()
    {
        if (!_data.isEnemyTargeted)
            TargetingChargedShot();
    }

    public void TargetingChargedShot()
    {
        RaycastHit hit;

        int layerMask = 1 << 2;
        layerMask = ~layerMask; // Ignore only objects in the Ignore Raycast layer (2)

        if (Physics.Raycast(playerShip.transform.position, playerShip.transform.TransformDirection(Vector3.forward), out hit, _data.maximumLockOnRange, layerMask) && hit.collider.gameObject.tag == "Enemy")
        {
            ChargedShotData.enemyTargeted = hit.collider.gameObject;
            ChargedShotData.enemyTargetedReticle = Instantiate(Resources.Load<GameObject>("Prefabs/Player/ChargedShot/EnemyTargetedReticle"), gameCamera.WorldToScreenPoint(ChargedShotData.enemyTargeted.transform.position), Quaternion.identity, uiCanvas.transform);
            // Add sound
        }
    }
}
