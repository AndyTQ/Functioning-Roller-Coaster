using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public bool switchToTrailer = false;
    public bool fadingToBlack = false;
    public Animator fadeToBlackAnimator;

    void Start()
    {
        switchToTrailer = false;
        fadingToBlack = false;
    }
    void Update()
    {
        // try
        // {
        //     SetContinueButton();
        // }
        // catch (System.NullReferenceException)
        // {
        //     // This happens when currentPlayer is not ready.   
        // }
        if (switchToTrailer)
        {
            SceneManager.LoadScene("IntroVideo");
        }

        if (fadingToBlack)
        {
            fadeToBlackAnimator.Play("MainMenuFadeToBlack");
        }
    }

    // public void SetContinueButton()
    // {
    //     string furthest = FindFurthestLevel();
    //     if (furthest == "A0")
    //     {
    //     }
    //     else{
    //         GameObject.Find("Buttons").transform.GetChild(3).gameObject.SetActive(true);
    //     }
    // }
    public void Quit()
    {
        Application.Quit();
    }

    public void StartButtonOnClick()
    {
        string furthest = FindFurthestLevel();
        if (furthest == "foo") //TODO: Change it back to A1, if you want to enable the cutscene.
        {
            StartFadingToBlack();
        }
        else{
            // if (GameObject.Find("EmptyScoreToggle").GetComponent<Toggle>().isOn)
            // {
            //     GameDataManager.StartRecordingData(true);
            //     StartFadingToBlack();
            // }
            // else
            // {
                Initiate.Fade("LevelSelection", new Color(0, 0, 0, 255), 2.5f); // TODO: CHANGE IT BACK
            // }
        }
    }


    public void SwitchSceneFade(string scene)
    {
        Initiate.Fade(scene, new Color(0,0,0,255), 2.5f);
        if (scene.Contains("A") 
        || scene.Contains("B")
        || scene.Contains("C")
        || scene.Contains("D"))
        {
            GameDataManager.currentLevel = scene;
        }
    }

    public void StartFadingToBlack()
    {
        fadingToBlack = true;
        GameDataManager.currentLevel = "A1";
    }

    public void LevelButtonOnClick()
    {
        string levelSceneName = GetLevelName();
        SwitchSceneFade(levelSceneName);
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

    // public void ContinueLevel()
    // {
    //     string furthest = FindFurthestLevel();
    //     SceneManager.LoadScene(furthest);
    //     GameDataManager.currentLevel = furthest;
    // }

    public void SwitchScene(){
        string nextlevel;
        if (GameObject.Find("CheckWinController") == null) // in main menu
        {
            nextlevel = "A1";
        }
        else {
            nextlevel = GameObject.Find("CheckWinController").GetComponent<CheckWin>().level[0] 
            + (int.Parse(GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Substring(1)) + 1).ToString();
            // set current level.
        }
        GameDataManager.currentLevel = nextlevel;
        SceneManager.LoadScene(nextlevel);
    }
    public void ToMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    public string FindFurthestLevel()
    {
        for(int i = 1; i < 8; i ++)
        {
            string levelName = "A" + i.ToString();
            if (GameDataManager.currentPlayer.levelStar[levelName] == 0)
            {
                return levelName;
            }
        }
        //TODO: Finish implementing the level system.
        //Currently if player passes all level, the player will play A6.
        return "A6";
    }
}
