using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetReticlePosition : MonoBehaviour
{
    // Turn this into an abstract parent class!

    private Camera gameCamera;
    private ChargedShotData _data;

    private void Start()
    {
        gameCamera = Camera.main;
        _data = GetComponent<ChargedShotData>();
    }

    private void Update()
    {
        transform.position = gameCamera.WorldToScreenPoint(ChargedShotData.enemyTargeted.transform.position);
    }
}
