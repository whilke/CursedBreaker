using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    public  TMP_Text text;
    void Start()
    {
        var gameData = FindObjectOfType<GameData>();

        string data = String.Format(this.text.text, gameData.TotalScore, gameData.HighScore);
        this.text.text = data;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }
}
