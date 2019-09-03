using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Dictionary<string, int> LEVELCOUNTS;
    public Dictionary<string, int> levelStar;
    public Dictionary<string, int> levelHiScore;
    public Dictionary<string, bool> levelProgress;
    public int coinCount;
    public int hintPrice;

    
    void Start() {
    }

    public Player()
    {
        // Initialize level stars
        // Initialize level high scores
        // Initialize level progresses

        
        // Initialize coin count
        coinCount = 10;
        hintPrice = 3;
        
        levelStar = new Dictionary<string, int>();
        levelHiScore = new Dictionary<string, int>();
        levelProgress = new Dictionary<string, bool>();

        LEVELCOUNTS = new Dictionary<string, int>();
        LEVELCOUNTS.Add("A", 7);
        LEVELCOUNTS.Add("B", 13);
        LEVELCOUNTS.Add("C", 9);
        LEVELCOUNTS.Add("D", 14);

        foreach (string chapter in LEVELCOUNTS.Keys)
        {
            for (int i = 1; i < LEVELCOUNTS[chapter] + 1; i++)
            {
                levelStar.Add(chapter + i.ToString(), 0);
                levelHiScore.Add(chapter + i.ToString(), 0);
                levelProgress.Add(chapter + i.ToString(), false);
            }
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        levelHiScore = data.levelHiScore;
        levelStar = data.levelStar;
        coinCount = data.coinCount;
        levelProgress = data.levelProgress;
    }
}
