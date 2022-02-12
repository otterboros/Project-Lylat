using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    Scoreboard scoreboard;
    int comboValue;

    public static ComboManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
    }

    public void StartCombo()
    {
        comboValue = -1;
    }

    public void AddToCombo()
    {
        comboValue++;
    }

    public void FinishCombo()
    {
        scoreboard.ModifyScore(comboValue);
        if (comboValue >= 1)
        {
            // Add a UI Element that displays "Downed + comboValue"
            Debug.Log("Downed +" + comboValue);
        }
        else if (comboValue < 1)
            Debug.Log("No combo!");
    }
}
