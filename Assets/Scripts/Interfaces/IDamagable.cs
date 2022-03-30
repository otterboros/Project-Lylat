using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    int currentHealth { get; set; }
    void ChangeHealth(int value);
    void ProcessHealthState(int health);
}
