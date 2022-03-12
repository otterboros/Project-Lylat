using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    int currentHealth { get; set; }
    void TakeDamage(int damage);
    void ProcessHealthState(int health);
}
