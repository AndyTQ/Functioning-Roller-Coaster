using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioSource bgm;
    // Update is called once per frame
    void Start()
    {
        string[] options = new string[]{"toronto", "japan", "nepal", "france", "toronto1", "space"};
        
        string level;
        level = options[GameDataManager.currentBGM];
        string sceneName = SceneManager.GetActiveScene().name;


        // if (SceneManager.GetActiveScene().name.Contains("A"))
        // {
        //     level = "toronto";
        // }
        // else if (SceneManager.GetActiveScene().name.Contains("B"))
        // {
        //     level = "japan";
        // }
        // else if (SceneManager.GetActiveScene().name.Contains("C"))
        // {
        //     level = "nepal";
        // }
        // else if (SceneManager.GetActiveScene().name.Contains("D"))
        // {
        //     level = "france";
        // }
        // else
        // {
        //     level = "toronto";
        // }
        bgm.clip = Resources.Load<AudioClip>("Audio/" + level);
        bgm.Play();
        bgm.loop = true;

        GameDataManager.currentBGM += 1;
        if (GameDataManager.currentBGM >= GameDataManager.BGMCount)
        {
            GameDataManager.currentBGM = 0;
        }
    }
}
