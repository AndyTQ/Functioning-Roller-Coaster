using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelStarDisplayer : MonoBehaviour
{
    public GameObject starImages;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        string levelName = GetLevelName();
        if (GameDataManager.currentPlayer.levelStar[levelName] >= 1)
        {
            starImages.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }
        
        if (GameDataManager.currentPlayer.levelStar[levelName] >= 2)
        {
            starImages.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }

        if (GameDataManager.currentPlayer.levelStar[levelName] >= 3)
        {
            starImages.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }
    }

     public string GetLevelName()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        int levelIndexStarting = 2;
        string chapter;
        if (SceneName.Contains("1"))
        {
            chapter = "A";
        }
        else if (SceneName.Contains("2"))
        {
            chapter = "B";
        }
        else if (SceneName.Contains("3"))
        {
            chapter = "C";
        }
        else
        {
            chapter = "D";
        }
        string levelNumber = gameObject.transform.parent.gameObject.name;
        int levelDigit = levelNumber.Length - 2;
        string levelSceneName = chapter + gameObject.transform.parent.name.Substring(levelIndexStarting, levelDigit);
        return levelSceneName;
    }
}
