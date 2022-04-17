// HealthBarManager.cs - Calculate and update healthbar shaders for NPC and Player
// TO-DO: Replace 10.00f with a form of _data.maxHealth
//----------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager instance;

    [SerializeField] GameObject PCHealthbar;
    [SerializeField] GameObject NPCHealthbar;

    private Material m_PCHealthbar;
    private Material m_NPCHealthbar;
    //private PlayerData _data;

    private void Awake()
    {
        instance = this;

        m_PCHealthbar = PCHealthbar.GetComponent<Image>().material;
        m_NPCHealthbar = NPCHealthbar.GetComponent<Image>().material;
    }

    private float NormalizeHealth(int health)
    {
        float unnormHealth = (float) health;
        
        float normHealth = unnormHealth / 10.00f;
        return normHealth;
    }

    public void UpdateHealthBar(int health, string hbType)
    {
        float shaderHealth = NormalizeHealth(health);
        if(hbType == "Player")
        {
            m_PCHealthbar.SetFloat("_Health", shaderHealth);
        }
        else if(hbType == "NPC")
        {
            m_NPCHealthbar.SetFloat("_Health", shaderHealth);
        }
    }
}
