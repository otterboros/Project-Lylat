using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    private Scoreboard _scoreboard;
    private DownedTextController _dtc;

    private int comboValue;

    private void Awake()
    {
        instance = this;

        _scoreboard = FindObjectOfType<Scoreboard>();
        _dtc = FindObjectOfType<DownedTextController>();
    }

    public void StartCombo()
    {
        comboValue = 0;
    }

    public void AddToCombo()
    {
        comboValue++;
    }

    public void FinishCombo(Transform lastEnemyTransform)
    {
        if (comboValue >= 2)
        {
            _scoreboard.ModifyScore(comboValue - 1);

            // Add a UI Element that displays "Downed + comboValue"
            _dtc.UpdateDownedTextPosition(lastEnemyTransform);
            _dtc.UpdateDownedText(comboValue);
            _dtc.StartResettingDownedText();
            // Delay for 3 sec and move slightly, then reset
        }
        else if (comboValue < 2)
            Debug.Log("No combo!");
    }
}
