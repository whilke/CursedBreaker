using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int TotalScore;
    public int HighScore;

    private static GameData instance;

    void Start()
    {
        if (GameData.instance == null)
        {
            GameData.instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        if (GameData.instance != this)
        {
            Destroy(this.gameObject);
        }
    }

}
