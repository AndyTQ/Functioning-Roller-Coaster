using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : MonoBehaviour
{
    public static Player currentPlayer;
    public static string currentLevel;
    public static int currentBGM = 0;
    public static int BGMCount = 6;

    void Start()
    {
        StartRecordingData(false);
    }

    public static void StartRecordingData(bool newPlayer)
    {
        PlayerData loadedPlayerData = SaveSystem.LoadPlayer();
        if (loadedPlayerData == null || newPlayer) // no saved files
        {
            Debug.Log("Creating a new player...");
            // create a new player
            currentPlayer = new Player();
            SaveSystem.SavePlayer(currentPlayer);
        }
        else{
            Debug.Log("Loading an existing player...");
            currentPlayer = new Player();
            // load existing data into currentplayer.
            currentPlayer.levelHiScore = loadedPlayerData.levelHiScore;
            currentPlayer.levelStar = loadedPlayerData.levelStar;
            currentPlayer.coinCount = loadedPlayerData.coinCount;
            currentPlayer.levelProgress = loadedPlayerData.levelProgress;
            currentPlayer.hintPrice = loadedPlayerData.hintPrice;
            // SaveSystem.SavePlayer(currentPlayer);
            GameObject.Find("EmptyScoreToggle").GetComponent<Toggle>().isOn = false;
        }
    }

    public void SetLevel(string levelName)
    {
        currentLevel = levelName;
    }
}
