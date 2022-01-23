using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            Vector3 reticlePos = gameCamera.WorldToScreenPoint(reticleTargets[i].transform.position);

            reticle.transform.position = new Vector3
                (reticlePos.x,
                reticlePos.y,
                reticlePos.z);

            i += 1;
        }
    }
}
