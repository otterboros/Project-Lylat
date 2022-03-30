using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotData : MonoBehaviour
{
    [Header("Charged Shot Settings")]
    [Tooltip("Maximum Z range of Charged Shot lock-on targeting")]
    public int maximumLockOnRange = 50;
    public Vector3 chargedShotPositionOffset = new Vector3(0f, -0.753000021f, 1.30999994f);
    public int chargedShotSpeed = 2;
    public static int chargedShotDamage = -2;

    public static GameObject chargedShot { get; set; }
    public bool isChargedShotCreated
    {
        get { return chargedShot; }
        set
        {
            if (chargedShot != null)
                isChargedShotCreated = true;
            else
                isChargedShotCreated = false;
        }
    }

    public static GameObject enemyTargeted { get; set; }
    public bool isEnemyTargeted
    {
        get { return enemyTargeted;}
        set { 
              if (enemyTargeted != null) 
                isEnemyTargeted = true;
              else 
                isEnemyTargeted = false;
        }
    }

    public static GameObject enemyTargetedReticle { get; set; }

    public static bool isChargedShotFired = false;

    public static bool isChargedShotSequenceEnded = false;
}
