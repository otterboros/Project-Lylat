// EnemyData.cs - Health, score, shot attributes, and other data about this enemy
//-------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Maximum health of ship.")]
    public int maxHealth = 1;

    [Header("Score Settings")]
    [Tooltip("Point value of ship.")]
    public int scoreValue = 1;

    [Header("Pickup Settings")]
    [Tooltip("Pickup to spawn on death.")]
    public string spawnPickup = "";

    [Header("Attack Settings")]
    [Tooltip("How many shots per second. Processed as '1/shotsPerSecond' in EnemyAI.cs")]
    public float shotsPerSecond = 1;
    public float shotSpeed = 1;
    public int shotDamage = 1;
    public int distToDestroy = 0;

    [Header("Targetting Settings")]
    [Tooltip("Target to fire at.")]
    public GameObject target;
    [Tooltip("Attack type.")]
    public string firingMode;

    [Header("Movement Settings")]
    [Tooltip("How many degrees per second the ship can rotate.")]
    public float shipRotationSpeed;

    public bool isArmored { get { return TryGetComponent(out ArmoredStateController asc); } }
    public bool isShielded = false;
}