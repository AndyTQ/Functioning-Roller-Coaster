using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectionStarUpdater : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            int[] starsEarned = GetStarsEarned(getChapterName(i));
            GameObject.Find("StarCount" + i.ToString()).GetComponent<TextMeshProUGUI>().text = starsEarned[0] + "/" + starsEarned[1];
        }
    }

    string getChapterName(int id)
    {
        switch(id)
        {
            case(1):
                return "A";
            case(2):
                return "B";
            case(3):
                return "C";
            case(4):
                return "D";
            default:
                Debug.LogError("Error on line 38, LevelSelectionStarUpdater.cs!");
                return null;
        }
    }
    

    int[] GetStarsEarned(string chapter)
    {
        int[] res = new int[2];
        res[0] = 0;
        res[1] = 0;
        foreach(string currentLevel in GameDataManager.currentPlayer.levelStar.Keys)
        {
            if (currentLevel.Contains(chapter))
            {
                res[0] += GameDataManager.currentPlayer.levelStar[currentLevel];
                res[1] += 3;
            }
        }

        return res;
    }
}
