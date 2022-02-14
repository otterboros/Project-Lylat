using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReticle : MonoBehaviour
{
    [SerializeField] GameObject[] reticles;
    [SerializeField] GameObject[] reticleTargets;

    Camera gameCamera;

    private void Start()
    {
        gameCamera = Camera.main;
    }

    private void Update()
    {
        PositionReticles();
    }

    void PositionReticles()
    {
        int i = 0;

        foreach(GameObject reticle in reticles)
        {
            reticle.transform.position = gameCamera.WorldToScreenPoint(reticleTargets[i].transform.position);

            i++;
        }
    }
}
