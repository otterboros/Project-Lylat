// EnemyData.cs - Health, score, shot attributes, and other data about this enemy
//-------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int maxHealth = 1;

    public int scoreValue = 1;

    [Tooltip("How many shots per second. Processed as '1/shotsPerSecond' in EnemyAI.cs")]
    public float shotsPerSecond = 1;
    public float shotSpeed = 1;
    public int shotDamage = 1;
    public int distToDestroy = 0;

    public bool isArmored = false;
    public bool isShielded = false;

    public GameObject target;
    public string firingMode;
}
