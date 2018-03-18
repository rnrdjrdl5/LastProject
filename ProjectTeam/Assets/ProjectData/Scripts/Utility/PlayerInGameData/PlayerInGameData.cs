using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInGameData {


    public PlayerInGameData(string s, float f)
    {
        PlayerName = s;
        PlayerScore = f;
    }

    private string PlayerName;
    
    public string GetPlayerName()
    {
        return PlayerName;
    }
    public void SetPlayerName(string s)
    {
        PlayerName = s;
    }

    private float PlayerScore;

    public void SetPlayerScore(float s)
    {
        PlayerScore = s;
    }
    public float GetPlayerScore()
    {
        return PlayerScore;
    }
}
