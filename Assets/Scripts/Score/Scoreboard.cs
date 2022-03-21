using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    int score;
    TMP_Text scoreText;

    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        if(SceneManager.GetActiveScene().buildIndex == 0)
            scoreText.text = "0";
        if (SceneManager.GetActiveScene().buildIndex == 1)
            scoreText.text = PlayerDataStatic.scoreStatic.ToString();
    }
    
    public void ModifyScore(int amountToModify)
    {
        score += amountToModify;
        scoreText.text = score.ToString();
        PlayerDataStatic.scoreStatic = score;
    }
}
