using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateHealthBar(int health)
    {
        // Palceholder
        Debug.Log($"Health is now {health}!");
    }
}
