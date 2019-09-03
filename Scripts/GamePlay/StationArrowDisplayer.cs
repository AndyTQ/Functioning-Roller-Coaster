using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationArrowDisplayer : MonoBehaviour
{
    private Vector2 startingPtArrowPos;
    private Camera cam;
    private Vector2 endingPtArrowPos;
    public GameObject startingPtArrow;
    public GameObject endingPtArrow;
    public GameObject stationArrows;
    public bool fadeOut;
    void Start()
    {
        cam = Camera.main;

    }
    // Update is called once per frame
    void Update()
    {
        startingPtArrowPos = cam.WorldToScreenPoint(GameObject.Find("StartingSpline").transform.position + new Vector3(3, 2, 0));
        endingPtArrowPos = cam.WorldToScreenPoint(GameObject.Find("EndingSpline").transform.position + new Vector3(0, 2, 0));
        startingPtArrow.transform.position = startingPtArrowPos;
        endingPtArrow.transform.position = endingPtArrowPos;
        if (fadeOut)
        {
            if (stationArrows.GetComponent<CanvasGroup>().alpha > 0) 
            {
                stationArrows.GetComponent<CanvasGroup>().alpha -= 0.05f;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }   
        else
        {
            if (stationArrows.GetComponent<CanvasGroup>().alpha < 1) 
            {
                stationArrows.GetComponent<CanvasGroup>().alpha += 0.05f;
            }
        }
    }
}
