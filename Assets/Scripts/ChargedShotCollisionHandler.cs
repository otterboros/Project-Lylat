using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotCollisionHandler : MonoBehaviour
{
    int timeTilDestroy = 2;

    void OnTriggerEnter(Collider other)
    {
        StartExplosionSequence();
    }

    void StartExplosionSequence()
    {
        SetExplosionActive(true);
        Destroy(gameObject, timeTilDestroy);
        // This should Trigger the OnParticleCollision in Enemy.cs

        // Later, it should modiy ProcessEnemyHealth to do more damage
        // And ProcessEnemyDeath to add more score
        // As well as create a combo meter

        // For now, just use current scripts
    }

    void SetExplosionActive(bool isExplosionActive)
    {
        var emission = GetComponent<ParticleSystem>().emission; // Stores the module in a local variable
        emission.enabled = isExplosionActive; // Applies the new value directly to the Particle System
        // play charged shot explosion sound
    }
}
