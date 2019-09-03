using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Dictionary<string, bool> levelProgress;
    public Dictionary<string, int> levelStar;
    public Dictionary<string, int> levelHiScore;
    public int coinCount;
    public int hintPrice;
    // Update is called once per frame
    public PlayerData (Player player)
    {
        levelStar = player.levelStar;
        levelHiScore = player.levelHiScore;
        levelProgress = player.levelProgress;
        hintPrice = player.hintPrice;
        coinCount = player.coinCount;
    }
}
