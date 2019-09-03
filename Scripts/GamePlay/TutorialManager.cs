using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutArrow;
    public GameObject tutPanel;
    public GameObject nextStepButton;
    public GameObject checkWinController;
    private bool initPanel = false;
    public string[] messages;
    public GameObject functionBoard;
    public static int currentIndex = 0;
    public GameObject tutProtector;
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI tutButtonText;
    private int colorValue = 255;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        cam = Camera.main;
        if (checkWinController.GetComponent<CheckWin>().level.Contains("A"))
        {
            messages = new string[]{
                "Welcome to FRC Headquarters.",
                "Your boss has given you a bunch of functions. You must draw the graphical representations of all of them.",
                "Your function has been given at the top. ",
                "Now let's start!",
                "Left Click the build tool to draw a line for your graph.",
                "Left Click to determine the starting point of your graph.",
                "Left Click to determine the end point of your graph.",
                "You've just created a piece of track!",
                "If your roller coaster is fully connected, press Go!"
                };
        }
        else if (checkWinController.GetComponent<CheckWin>().level.Contains("B"))
        {
            messages = new string[]{
                "Yo, welcome back.",
                "You did a great job in Toronto.",
                "But now you gotta build NON-LINEAR functions!",
                "Select the build tool again as usual.",
                "As usual, determine the starting point of your graph.",
                "Right click to determine the end point of this track.",
                "But this is a straight line. We need to BEND it.",
                "Click the 'Curve Editor'.",
                "Now you are under editing mode. Click the track.",
                "You can see two control points on the track!",
                "Try to drag one of them. If you are feeling right, click Go!"
                };
        }
        tutorialText.text = messages[0]; 
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentIndex);
      
        if(functionBoard.transform.localPosition.y > 190)
        {
            if (!initPanel)
            {
                tutPanel.SetActive(true);
                initPanel = true;
            }
        }
        


        // Mouse Click Responser
        if(Input.GetMouseButtonDown(0)){
            if (checkWinController.GetComponent<CheckWin>().level.Contains("A"))
            {
                if (currentIndex == 5 || currentIndex == 6)
                {
                    NextMessage();
                }
            }
            else if (checkWinController.GetComponent<CheckWin>().level.Contains("B"))
            {
                if (currentIndex == 4 || currentIndex == 5)
                {
                    NextMessage();
                }

            }
        }
    }

    public void NextMessage()
    {
        currentIndex ++;
        Debug.Log(currentIndex);
        try
        {                     
        tutorialText.text = messages[currentIndex]; 
        }
        catch (System.IndexOutOfRangeException)
        {
            tutPanel.SetActive(false);
        }
        if (checkWinController.GetComponent<CheckWin>().level.Contains("A"))
        {
            if (currentIndex == 3)
            {
            }
            else if (currentIndex == 4)
            {
                tutArrow.SetActive(true);
                nextStepButton.GetComponent<Button>().interactable = false;
                Vector3 newPosition = new Vector3(7.5f, -5.0f, 0.0f);
                tutArrow.transform.localScale = new Vector3(tutArrow.transform.localScale.x, tutArrow.transform.localScale.y * (-1.0f), tutArrow.transform.localScale.z);
                tutArrow.transform.position = cam.WorldToScreenPoint(newPosition);
                tutButtonText.text = "Waiting..."; 
                
                GameObject.Find("ButtonBuild").GetComponent<Animator>().Play("Flashing");
            } 
            else if (currentIndex == 5)
            {
                
                GameObject.Find("ButtonBuild").GetComponent<Animator>().Play("Idle");
                Vector3 newPosition = new Vector3(-15f, 2f, 0.0f);
                tutArrow.transform.localScale = new Vector3(tutArrow.transform.localScale.x, tutArrow.transform.localScale.y, tutArrow.transform.localScale.z);
                tutArrow.transform.position = cam.WorldToScreenPoint(newPosition);
                tutButtonText.text = "Waiting..."; 
            }
            else if (currentIndex == 6)
            {
                Vector3 newPosition = new Vector3(15f, 2f, 0.0f);
                tutArrow.transform.localScale = new Vector3(tutArrow.transform.localScale.x, tutArrow.transform.localScale.y, tutArrow.transform.localScale.z);
                tutArrow.transform.position = cam.WorldToScreenPoint(newPosition);
                tutButtonText.text = "Got It";
                nextStepButton.GetComponent<Button>().interactable = true;  
            }
            else if (currentIndex == 7)
            {

                tutButtonText.text = "Got It";
                nextStepButton.GetComponent<Button>().interactable = true;  
                tutProtector.SetActive(false);
            }
            else if (currentIndex == 8)
            {   Vector3 newPosition = new Vector3(0, -6.0f, 0.0f);
                tutArrow.transform.localScale = new Vector3(tutArrow.transform.localScale.x, tutArrow.transform.localScale.y, tutArrow.transform.localScale.z);
                tutArrow.transform.position = cam.WorldToScreenPoint(newPosition);
                tutButtonText.text = "Waiting...";
                nextStepButton.GetComponent<Button>().interactable = false;  
                
                // move the position of tut panel.
            }
        }

        if (checkWinController.GetComponent<CheckWin>().level.Contains("B"))
        {
            if (currentIndex == 3)
            {
                tutButtonText.text = "Waiting";
                nextStepButton.GetComponent<Button>().interactable = false;  
                GameObject.Find("ButtonBuild").GetComponent<Image>().raycastTarget = true;
                GameObject.Find("ButtonBuild").GetComponent<Animator>().Play("Flashing");
            }
            else if (currentIndex == 4)
            {   
                GameObject.Find("ButtonBuild").GetComponent<Animator>().Play("Idle");
                Vector3 newPosition = new Vector3(7.5f, -5.0f, 0.0f);
                tutArrow.transform.localScale = new Vector3(tutArrow.transform.localScale.x, tutArrow.transform.localScale.y * (-1.0f), tutArrow.transform.localScale.z);
                tutArrow.transform.position = cam.WorldToScreenPoint(newPosition);
                tutButtonText.text = "Waiting";
                nextStepButton.GetComponent<Button>().interactable = false;  
            } 
            else if (currentIndex == 5)
            {
                Vector3 newPosition;
                GameObject.Find("ButtonBuild").GetComponent<Image>().color = Color.white;
                newPosition = new Vector3(-15.0f, 2.0f, 0.0f);
                tutArrow.transform.localScale = new Vector3(tutArrow.transform.localScale.x, tutArrow.transform.localScale.y * (-1.0f), tutArrow.transform.localScale.z);
                tutArrow.transform.position = cam.WorldToScreenPoint(newPosition);
            }
            else if (currentIndex == 6)
            {
                Vector3 newPosition = new Vector3(15.0f, 2.0f, 0.0f);
                tutArrow.transform.position = cam.WorldToScreenPoint(newPosition);
                tutButtonText.text = "Got It";
                nextStepButton.GetComponent<Button>().interactable = true;
            }
            else if (currentIndex == 7)
            {
                GameObject.Find("ButtonBender").GetComponent<Animator>().Play("Flashing");
                tutProtector.SetActive(false);
                tutButtonText.text = "Waiting";
                nextStepButton.GetComponent<Button>().interactable = false;  
            }
            else if (currentIndex == 8)
            {
                GameObject.Find("ButtonBender").GetComponent<Animator>().Play("Idle");
                Vector3 newPosition = new Vector3(0, -6.0f, 0.0f);
                tutArrow.transform.localScale = new Vector3(tutArrow.transform.localScale.x, tutArrow.transform.localScale.y * (-1.0f), tutArrow.transform.localScale.z);
                tutArrow.transform.position = cam.WorldToScreenPoint(newPosition);
                tutButtonText.text = "Waiting";
                nextStepButton.GetComponent<Button>().interactable = false;
                tutPanel.GetComponent<Animator>().Play("tutorialShiftPosition1");
            }
            else if (currentIndex == 9)
            {
                
                tutButtonText.text = "Got It!";
                nextStepButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void changeColor()
    {

    }

    public void shutDown()
    {
        GameObject.Find("TutorialPanel").SetActive(false);
        return;
    }
}
