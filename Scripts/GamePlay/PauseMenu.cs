using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject pauseButtons;
    public GameObject pauseSettings;
    public GameObject cursors;
    public GameObject pauseBg;
    private Material pauseBgMaterial;
    public float blurSize = 0.0f;
    public float brightness = 1.0f;
    // Update is called once per frame
    void Start()
    {
        pauseBg.GetComponent<Image>().material = Instantiate(pauseBg.GetComponent<Image>().material);
        pauseBgMaterial = pauseBg.GetComponent<Image>().material;
        pauseBgMaterial.SetFloat("_Size", 0.0f);
        pauseBgMaterial.SetColor("_Color", new Color(1, 1, 1, 1));
    }
    void Update()
    {
        // Apply fade in effect for ui if pause is just clicked.
        if (GameIsPaused)
        {
           if (blurSize < 2f)
           {
               blurSize += 0.2f;
               pauseBgMaterial.SetFloat("_Size", blurSize);
           }
           if (brightness > 0.6f)
           {
               brightness -= 0.05f;
               pauseBgMaterial.SetColor("_Color", new Color(brightness, brightness, brightness, 1));
           }
        }

        else
        {
           if (blurSize > 0)
           {
               blurSize -= 0.2f;
               pauseBgMaterial.SetFloat("_Size", blurSize);
           }
           if (brightness < 1)
           {
               brightness += 0.05f;
               pauseBgMaterial.SetColor("_Color", new Color(brightness, brightness, brightness, 1));
           }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else 
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        cursors.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        cursors.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;// IMPORTANT: Reset timescale to 1! otherwise when player go back to this level, it will cause problems...
        SceneManager.LoadScene("MainMenu");
    }

    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    public void GoToSettings()
    {
        pauseButtons.SetActive(false);
        pauseSettings.SetActive(true);
    }
    public void SettingsToMenu()
    {
        pauseButtons.SetActive(true);
        pauseSettings.SetActive(false);
    }
}
