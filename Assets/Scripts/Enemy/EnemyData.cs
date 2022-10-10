// EnemyData.cs - Health, score, shot attributes, and other data about this enemy
//-------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : BaseData
{
    [Header("Score Settings")]
    [Tooltip("Point value of ship.")]
    public int scoreValue = 1;

    [Header("Pickup Settings")]
    [Tooltip("Pickup to spawn on death.")]
    public string spawnPickup = "";

    [Header("Movement Settings")]
    [Tooltip("How many degrees per second the ship can rotate.")]
    public float shipRotationSpeed;

    public bool isArmored { get { return TryGetComponent(out ArmoredStateController asc); } }
    public bool isShielded = false;
}