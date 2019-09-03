using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WSMGameStudio.Splines;

public class EraserCursor : MonoBehaviour
{
    private Grid grid;
    private Vector3 closestPt;
    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    public GameObject[] otherCursors;
    private int editingState = 0;
    private GameObject spawnTrack;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButton(0)){
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name.Contains("Spline") &&
                    (hit.collider.name != "StartingSpline" && hit.collider.name != "EndingSpline")){
                    // For chapter 4: Check if there is corresponding HolePrompts before destroying the object.
                    Spline currentSpline = hit.collider.gameObject.GetComponent<Spline>();
                    float SplineStartX, SplineEndX;
                    SplineStartX = currentSpline.GetControlPointPosition(0).x + currentSpline.transform.position.x;
                    SplineEndX = currentSpline.GetControlPointPosition(3).x + currentSpline.transform.position.x;

                    if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Contains("D"))
                    {
                        foreach (Transform promptTransform in GameObject.Find("HolePrompts").transform)
                        {
                            GameObject promptObject = promptTransform.gameObject;
                            if (promptObject.name == "HolePromptx = " + (((int)Mathf.Round(SplineStartX)).ToString()) 
                            || promptObject.name == "HolePromptx = " + (((int)Mathf.Round(SplineEndX)).ToString()))
                            {
                                if (promptObject.name == "HolePromptx = " + (((int)Mathf.Round(SplineStartX)).ToString()))
                                {
                                    GameObject.Find("HoleController").GetComponent<HoleController>().discontinuity.Remove(Mathf.RoundToInt(SplineStartX));
                                }
                                if (promptObject.name == "HolePromptx = " + (((int)Mathf.Round(SplineEndX)).ToString()))
                                {
                                    GameObject.Find("HoleController").GetComponent<HoleController>().discontinuity.Remove(Mathf.RoundToInt(SplineEndX));
                                }
                                Destroy(promptObject);   
                            }
                        }
                    }

                    GameObject discontinuity1Left = GameObject.Find("DiscontinuityX = " + ((int)Mathf.Round(SplineStartX)).ToString() + "Left");
                    GameObject discontinuity1Right = GameObject.Find("DiscontinuityX = " + ((int)Mathf.Round(SplineStartX)).ToString() + "Right");
                    GameObject discontinuity2Left = GameObject.Find("DiscontinuityX = " + ((int)Mathf.Round(SplineEndX)).ToString() + "Left");
                    GameObject discontinuity2Right = GameObject.Find("DiscontinuityX = " + ((int)Mathf.Round(SplineEndX)).ToString() + "Right");

                    if (discontinuity1Left != null && discontinuity1Right != null)
                    {
                        Destroy(GameObject.Find("DiscontinuityX = " + ((int)Mathf.Round(SplineStartX)).ToString() + "Left"));
                        Destroy(GameObject.Find("DiscontinuityX = " + ((int)Mathf.Round(SplineStartX)).ToString() + "Right"));
                        GameObject.Find("HoleController").GetComponent<HoleController>().discontinuity.Remove(Mathf.RoundToInt(SplineStartX));
                    }

                    if (discontinuity2Left != null && discontinuity2Right != null)
                    {
                        Destroy(GameObject.Find("DiscontinuityX = " + ((int)Mathf.Round(SplineEndX)).ToString() + "Left"));
                        Destroy(GameObject.Find("DiscontinuityX = " + ((int)Mathf.Round(SplineEndX)).ToString() + "Right"));
                        GameObject.Find("HoleController").GetComponent<HoleController>().discontinuity.Remove(Mathf.RoundToInt(SplineEndX));
                    }

                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }


    public void ToggleState() {
        gameObject.SetActive(!gameObject.activeInHierarchy); //this changes the state from on to off and vice-versa
    
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
    }
}
