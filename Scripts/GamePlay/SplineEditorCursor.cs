using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Splines;
using UnityEngine.UI;

public class SplineEditorCursor : MonoBehaviour
{
    private Grid grid;
    public static bool allowClick;
    private Vector3 closestPt;
    private Ray ray;
    private RaycastHit hit;
    public GameObject[] otherCursors;
    public bool allowToggling = true;
    private Camera cam;
    private int editingState = 0;
    private void Awake()
    {
    }
    void Start()
    {
        allowClick = true;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButtonDown(0) && allowClick){
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name.Contains("Spline") && hit.collider.name != "StartingSpline" && hit.collider.name != "EndingSpline"){
                    GameObject.Find("SplineClickSound").GetComponent<AudioSource>().Play();
                    GameObject selected = hit.collider.gameObject;
                    Spline selectedSpline = selected.GetComponent<Spline>();
                    selectedSpline.onEdit = true;
                    foreach(Transform spline in GameObject.Find("Splines").transform)
                    {
                        if (string.Compare(spline.name, hit.collider.name) != 0)
                        {
                            spline.gameObject.GetComponent<Spline>().onEdit = false;
                        }
                    }
                }
            }
            else //clicked blank, diabling editing for all splines.
            {
                if (allowToggling) {
                    DisableEditing();
                    DisableLines();
                }
            }
        }
    }


    private void TutorialNextMsg()
    {
        if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Contains("B") && TutorialManager.currentIndex == 7)
        {
            GameObject.Find("TutorialManager").GetComponent<TutorialManager>().NextMessage();
        }
    }

    
    public void DisableLines()
    {
        GameObject ControlLinesStart = GameObject.Find("ControlPointLine0");
        GameObject ControlLinesEnd = GameObject.Find("ControlPointLine1");

        LineRenderer ControlLinesStartRenderer = ControlLinesStart.GetComponent<LineRenderer>();
        LineRenderer ControlLinesEndRenderer = ControlLinesEnd.GetComponent<LineRenderer>();

        ControlLinesStartRenderer.enabled = false;
        ControlLinesEndRenderer.enabled = false;
    }

    public void DisableEditing(){
        foreach(Transform spline in GameObject.Find("Splines").transform)
        {
            spline.gameObject.GetComponent<Spline>().onEdit = false;
        }
    }
    
    public void ToggleState() {
        allowToggling = true; // restore toggling allowance.
        gameObject.SetActive(!gameObject.activeInHierarchy); //this changes the state from on to off and vice-versa.
        // disable all other cursors.
        foreach (GameObject cursor in otherCursors)
        {
            cursor.SetActive(false); // disabling other cursors, e.g. erasors, building tools, etc.
        }
        TutorialNextMsg();
    }
    public void ShutDown()
    {
        DisableEditing();
        DisableLines();
        allowToggling = true; // restore toggling allowance.
        gameObject.SetActive(false);
    }
    public void TurnOn()
    {
        gameObject.SetActive(true);
        TutorialNextMsg();
    }
}
