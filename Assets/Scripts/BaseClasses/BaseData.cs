using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData : MonoBehaviour
{
    [Header("Character Name")]
    public string charName;

    [Header("Health Settings")]
    [Tooltip("Maximum health of ship.")]
    public int maxHealth = 1;

    [Header("Attack Settings")]
    [Tooltip("How many shots per second. Processed as '1/shotsPerSecond' in BaseAI.cs")]
    public float shotsPerSecond = 1;
    public float shotSpeed = 1;
    public int shotDamage = 1;
    public int shotDistToDestroy = 0;

    [Header("Targetting Settings")]
    [Tooltip("Target to fire at.")]
    public GameObject target;
    [Tooltip("Attack type.")]
    public FiringModes firingMode;

    public enum FiringModes
    {
        None,
        FireObjectForward,
        FireObjectAtTarget,
        FireHomingObjectAtTarget
    }
}
