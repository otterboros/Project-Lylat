using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleParticleSystem : MonoBehaviour
{
    private ParticleSystem laserOne;
    private void Start()
    {
        //laserOne = transform.Find("Laser One").GetComponent<ParticleSystem>();
    }

    public void ToggleEmissionModule(ParticleSystem particleSystem, bool setActive = true)
    {
        var emission = particleSystem.GetComponent<ParticleSystem>().emission;
        emission.enabled = setActive;
    }

    public void ToggleLaserOneEmissionModule(bool setActive = true)
    {
        var emission = laserOne.emission;
        emission.enabled = setActive;
    }
}
