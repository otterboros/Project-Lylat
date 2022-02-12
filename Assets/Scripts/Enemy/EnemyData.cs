using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] public int maxHealth = 1;

    [SerializeField] public int scoreValue = 1;

    [SerializeField] public float shotsPerSecond = 1;
    [SerializeField] public float shotSpeed = 1;
    [SerializeField] public int shotDamage = 1;
}
