// HealthBarManager.cs - Calculate and update healthbar shader based on player health data
// TO-DO: Replace 10.00f with a form of _data.maxHealth
//----------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    private Material m_healthbar;
    private PlayerData _data;
    public static HealthBarManager instance;

    private void Start()
    {
        m_healthbar = GetComponent<Image>().material;
        instance = this;
    }

    private float NormalizeHealth(int health)
    {
        float unnormHealth = (float) health;
        
        float normHealth = unnormHealth / 10.00f;
        return normHealth;
    }

    public void UpdateHealthBar(int health)
    {
        float shaderHealth = NormalizeHealth(health);
        m_healthbar.SetFloat("_Health", shaderHealth);
    }
}
