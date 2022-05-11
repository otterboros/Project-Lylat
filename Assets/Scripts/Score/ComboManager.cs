using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    private Scoreboard _scoreboard;

    private int comboValue;
    private DownedTextController _dtc;

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

    public void FinishCombo(Transform enemyTransform)
    {
        if (comboValue >= 2)
        {
            _scoreboard.ModifyScore(comboValue - 1);

            Debug.Log($"Downed + {comboValue - 1}!");
            _dtc.UpdateDownedTextPosition(enemyTransform);
            _dtc.UpdateDownedText(comboValue);
            _dtc.StartResettingDownedText();
        }
        else if (comboValue < 2)
            Debug.Log("No combo!");
    }
}
