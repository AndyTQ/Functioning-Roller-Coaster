using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using WSMGameStudio.Splines;
using TMPro;

public class GridCursor : MonoBehaviour
{
    private int numTrackPlaced = 0;
    private Grid grid;
    private Vector3 closestPt;
    public GameObject splinePrefab;
    private Camera cam;
    public GameObject[] otherCursors;
    private GameObject Aux1;
    private GameObject Aux2;
    public static bool selected;
    public static bool allowClick;
    public int editingState = 0;
    private GameObject spawnTrack;
    private Vector3 startingPt;
    public LineRenderer lineRenderer;
    private MessageController messageController;
    public GameObject EraserCursor;
    public Material trackMaterial;
    public GameObject previewLine;
    public GameObject holePromptPrefab;
  
    void Start()
    {
        cam = Camera.main;
        selected = false;
        allowClick = true;
    }
    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        previewLine = GameObject.Find("TrackPreviewLine");
        previewLine.GetComponent<LineRenderer>().enabled = false;
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        closestPt = grid.GetNearestPointOnGrid(Input.mousePosition);
        Vector3 worldPos = grid.GetNearestPointOnGrid(cam.ScreenToWorldPoint(closestPt));
        worldPos.z = 0;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        transform.position = new Vector3(screenPos.x, screenPos.y, -50);
        lineRenderer = previewLine.GetComponent<LineRenderer>();   

        if(Input.GetMouseButtonDown(0)){
            if (editingState == 0 && allowClick){ 
                Image i = GetComponent<Image>();
                Sprite newCursor = Resources.Load<Sprite>("Cursors/PointUnderSelect");
                i.overrideSprite = newCursor;
                lineRenderer.enabled = true;
                startingPt = worldPos;
                GetComponent<CursorRotator>().enabled = true;
                gameObject.GetComponent<ClickSound>().PlaySound();
                editingState ++;
            }
            else if (editingState == 1){
                lineRenderer.enabled = false;
                if (!(IsOverlapping(startingPt.x, worldPos.x)) && !(startingPt.x == worldPos.x && startingPt.y == worldPos.y))
                {
                    
                    gameObject.GetComponent<ClickSound>().PlaySound("TrackPlaced");
                    startingPt.z = 0;
                    spawnTrack = Instantiate(splinePrefab, startingPt, Quaternion.identity);
                    spawnTrack.name = "Spline" + numTrackPlaced.ToString();
                    spawnTrack.transform.parent = GameObject.Find("Splines").transform;
                    numTrackPlaced ++;
                    Spline spline = spawnTrack.GetComponent<Spline>();
                    Vector3 origin = new Vector3(0,0,0);
                    spline.SetControlPointPositionAbsolute(0, origin);
                    spline.SetControlPointPositionAbsolute(1, (worldPos - startingPt) * 0.25f);
                    spline.SetControlPointPositionAbsolute(2, (worldPos - startingPt) * 0.75f);
                    spline.SetControlPointPositionAbsolute(3, worldPos - startingPt); // Pt for the ending anchor.
                    
                    if (worldPos.x < startingPt.x)
                    {
                        spline.SetControlPointPositionAbsolute(3, origin);
                        spline.SetControlPointPositionAbsolute(2, (worldPos - startingPt) * 0.25f);
                        spline.SetControlPointPositionAbsolute(1, (worldPos - startingPt) * 0.75f);
                        spline.SetControlPointPositionAbsolute(0, worldPos - startingPt); // Pt for the ending anchor.
                    }

                    SplineMeshRenderer splineRenderer = spawnTrack.GetComponent<SplineMeshRenderer>();
                    splineRenderer.ExtrudeMesh();

                    splineRenderer.enableCollision = true;

                    MeshCollider collider = spawnTrack.GetComponent<MeshCollider>();
                    collider.enabled = true;
                    collider.sharedMesh = spawnTrack.GetComponent<MeshFilter>().mesh;


                    /**** For chapter 4: Now check if a portal can be deployed. */
                    if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Contains("D"))
                    {
                        float EndX =  spline.GetControlPointPosition(0).x + spline.transform.position.x;
                        EndX = (Mathf.Round(EndX * 10f)) / 10;
                        float EndY =  spline.GetControlPointPosition(0).y + spline.transform.position.y;
                        EndY = (Mathf.Round(EndY * 10f)) / 10;

                        float StartX =  spline.GetControlPointPosition(3).x + spline.transform.position.x;
                        StartX = (Mathf.Round(StartX * 10f)) / 10;
                        float StartY =  spline.GetControlPointPosition(3).y + spline.transform.position.y;
                        StartY = (Mathf.Round(StartY * 10f)) / 10;

                        Dictionary<Spline, bool> sameXSplines = checkSameXSplines(EndX, StartX, EndY, StartY);
                        foreach (Spline currentSpline in sameXSplines.Keys)
                        {
                            GameObject prompt;
                            if (sameXSplines[currentSpline] == false) // this discontinuity happens on the left side of this spline.
                            {
                              

                                Vector3 otherEnd = new Vector3(EndX, EndY - 2.5f, 0);
                                Vector3 otherEndScreen = cam.WorldToScreenPoint(otherEnd);
                                // deploy the prompt to ask how to place a hole.
                                prompt = Instantiate(holePromptPrefab, otherEndScreen, Quaternion.identity);
                                prompt.name = "HolePrompt" + "x = " + ((int)Mathf.Round(EndX)).ToString();
                                prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text += ((int)Mathf.Round(EndX)).ToString() + "?";
                            
                            }
                            else // this discontinuity happens on the right side of this spline.
                            {
                                // deploy the prompt to ask how to place a hole. (same as above)
                                Vector3 otherStart = new Vector3(StartX, StartY - 2.5f, 0);
                                Vector3 otherStartScreen = cam.WorldToScreenPoint(otherStart);
                                // deploy the prompt to ask how to place a hole.
                                prompt = Instantiate(holePromptPrefab, otherStartScreen, Quaternion.identity);
                                
                                prompt.name = "HolePrompt" + "x = " + ((int)Mathf.Round(StartX)).ToString();
                                prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text += ((int)Mathf.Round(StartX)).ToString() + "?";
                            
                            }
                            prompt.transform.SetParent(GameObject.Find("HolePrompts").transform);
                        }
                    }
                    
                    
                    Image i = GetComponent<Image>();
                    Sprite newCursor = Resources.Load<Sprite>("Cursors/selectCursor");
                    i.overrideSprite = newCursor;
                    GetComponent<CursorRotator>().enabled = false;   
                }
                else if (IsOverlapping(startingPt.x, worldPos.x)) // display error message
                {
                    GameObject.Find("MessageController").GetComponent<MessageController>().PlayMessage("Invalid placement! This track is overlapping with your other tracks.");
                }
                editingState = 0;
            }
        }

        if (editingState == 1){
            Vector3[] positions = {startingPt, worldPos};
            lineRenderer.SetPositions(positions);
        } 
    }

    
    /**
        Determins whether the current spline's starting point's x coordinate is same as other spline's ending pt's x.!--
        Also check whether the current spline's ending point's x coordinate is same as other spline's start pt's x.
     */
    public Dictionary<Spline, bool> checkSameXSplines(float startX, float endX, float StartY, float EndY)
    {   
        Dictionary<Spline, bool> res = new Dictionary<Spline, bool>();
        foreach (Transform currSplineTransform in GameObject.Find("Splines").transform)
        {
            GameObject currSpline = currSplineTransform.gameObject;
            Spline spline = currSpline.GetComponent<Spline>();
            float otherStartX = spline.GetControlPointPosition(0).x + spline.transform.position.x;
            otherStartX = (Mathf.Round(otherStartX * 10f)) / 10;
            float otherEndX =  spline.GetControlPointPosition(3).x + spline.transform.position.x;
            otherEndX = (Mathf.Round(otherEndX * 10f)) / 10;

            float otherStartY = spline.GetControlPointPosition(0).y + spline.transform.position.y;
            otherStartY = (Mathf.Round(otherStartY * 10f)) / 10;
            float otherEndY =  spline.GetControlPointPosition(3).y + spline.transform.position.y;
            otherEndY = (Mathf.Round(otherEndY * 10f)) / 10;

            if (Mathf.Approximately(otherEndX, startX) && !Mathf.Approximately(StartY, otherEndY) && !res.ContainsKey(spline))
            {
                res.Add(spline, false);
            }
            if (Mathf.Approximately(otherStartX, endX) && !Mathf.Approximately(otherStartY, EndY) && !res.ContainsKey(spline))
            {
                res.Add(spline, true);
            }
        }
        return res;
    }

    public void ToggleState() {
        selected = true;
        editingState = 0;
        gameObject.SetActive(!gameObject.activeInHierarchy); //this changes the state from on to off and vice-versa
        if (!gameObject.activeInHierarchy == false) // reset cursor icon
        {
            Image i = GetComponent<Image>();
            Sprite newCursor = Resources.Load<Sprite>("Cursors/selectCursor");
            i.overrideSprite = newCursor;
        }
        // disable all other cursors.
        foreach (GameObject cursor in otherCursors)
        {
            if (cursor.name == "EditorCursor")
            {
                cursor.GetComponent<SplineEditorCursor>().DisableEditing();
                cursor.GetComponent<SplineEditorCursor>().DisableLines();
            }
            cursor.SetActive(false);
        }

        TutorialNextMsg();
    }

    public void SelectOn() {
        selected = true;
        editingState = 0;
        gameObject.SetActive(true); //this changes the state from on to off and vice-versa
        // disable all other cursors.
        foreach (GameObject cursor in otherCursors)
        {
            if (cursor.name == "EditorCursor")
            {
                cursor.GetComponent<SplineEditorCursor>().DisableEditing();
                cursor.GetComponent<SplineEditorCursor>().DisableLines();
            }
            cursor.SetActive(false);
        }

        TutorialNextMsg();
    }

    private void TutorialNextMsg()
    {
        if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Contains("A") && TutorialManager.currentIndex == 4)
        {
            GameObject.Find("TutorialManager").GetComponent<TutorialManager>().NextMessage();
        }
        else if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Contains("B") && TutorialManager.currentIndex == 3)
        {
            GameObject.Find("TutorialManager").GetComponent<TutorialManager>().NextMessage();
        }
    }

    public void TurnOn()
    {   
        
        gameObject.SetActive(true); //this changes the state from on to off and vice-versa
        // reset cursor icon
        
        selected = true;
        editingState = 0;


        Image i = GetComponent<Image>();
        Sprite newCursor = Resources.Load<Sprite>("Cursors/selectCursor");
        i.overrideSprite = newCursor;
        
        // disable all other cursors.
        foreach (GameObject cursor in otherCursors)
        {
            if (cursor.name == "EditorCursor")
            {
                cursor.GetComponent<SplineEditorCursor>().DisableEditing();
                cursor.GetComponent<SplineEditorCursor>().DisableLines();
            }
            cursor.SetActive(false);
        }

        TutorialNextMsg();
    }
    public void ShutDown()
    {
        lineRenderer.enabled = false;
        editingState = 0;
        gameObject.SetActive(false);
        Image i = GetComponent<Image>();
        Sprite newCursor = Resources.Load<Sprite>("Cursors/selectCursor");
        i.overrideSprite = newCursor;
    }
 

      
    public void turnOnSelect()
    {
        selected = true;
    }

    public void turnOffSelect()
    {
        selected = false;
    }

    /**
    Checks whether there are multiple outputs for one input. (which is not valid in this game.)
     */
    private bool IsOverlapping(float startX, float endX)
    {
        if (endX < startX) // Swap start and end pt in this case.
        {
            float tmp = endX;
            endX = startX;
            startX = tmp;
        }
        foreach (Transform currSplineTransform in GameObject.Find("Splines").transform)
        {
            GameObject currSpline = currSplineTransform.gameObject;
            Spline spline = currSpline.GetComponent<Spline>();
            float otherStart = spline.GetControlPointPosition(0).x + spline.transform.position.x;
            otherStart = (Mathf.Round(otherStart * 10f)) / 10;
            float otherEnd =  spline.GetControlPointPosition(3).x + spline.transform.position.x;
            otherEnd = (Mathf.Round(otherEnd * 10f)) / 10;
            if (otherStart <= startX)
            {
                if (otherEnd > startX + 0.2f) 
                {
                    return true;
                }
            }
            else if (otherStart + 0.2f < endX)
            {
                return true;
            }
        }

        return false;
    }
}