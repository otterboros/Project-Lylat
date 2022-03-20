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
        comboValue = 0;
    }

    public void AddToCombo()
    {
        comboValue++;
    }

    public void FinishCombo()
    {
        if (comboValue >= 2)
        {
            scoreboard.ModifyScore(comboValue - 1);
            // Add a UI Element that displays "Downed + comboValue"
            Debug.Log("Downed +" + (comboValue - 1));
        }
        else if (comboValue < 2)
            Debug.Log("No combo!");
    }
}
