using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetReticlePosition : MonoBehaviour
{
    private Camera gameCamera;

    private void Awake()
    {
        gameCamera = Camera.main;
    }

    private void Update()
    {
        transform.position = gameCamera.WorldToScreenPoint(ChargedShotData.enemyTargeted.transform.position);
    }
}
