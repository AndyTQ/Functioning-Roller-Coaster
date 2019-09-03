using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Splines;
using UnityEngine.UI;

public class HoleController : MonoBehaviour
{
    public List<int> discontinuity;
    public GameObject HolePrompts;
    public GameObject HolePrefab;
    // Start is called before the first frame update
    void Start()
    {
        HolePrompts = GameObject.Find("HolePrompts");
    }

    // Update is called once per frame
    void Update()
    {
        // add new members into the discontinuity list.
        foreach (Transform promptTransform in HolePrompts.transform)
        {
            GameObject prompt = promptTransform.gameObject;
            int discontinuityPt = int.Parse(prompt.name.Substring(prompt.name.IndexOf('=') + 1));
            if (!discontinuity.Contains(discontinuityPt))
            {
                discontinuity.Add(discontinuityPt);
            }
        }
        
        // check for all pts in discontinuity, there are some points that no longer is a valid discontinuous point.
        foreach (int currentPt in discontinuity)
        {
            bool endPtOccupy = false;
            bool startPtOccupy = false;
            foreach (Transform currSplineTransform in GameObject.Find("Splines").transform)
            {
                GameObject currSpline = currSplineTransform.gameObject;
                Spline spline = currSpline.GetComponent<Spline>();
                float splineStartX = spline.GetControlPointPosition(0).x + spline.transform.position.x;
                splineStartX = (Mathf.Round(splineStartX * 10f)) / 10;
                float splineEndX =  spline.GetControlPointPosition(3).x + spline.transform.position.x;
                splineEndX = (Mathf.Round(splineEndX * 10f)) / 10;
                
                if (Mathf.RoundToInt(splineStartX) == currentPt)
                {
                    startPtOccupy = true;
                }

                if (Mathf.RoundToInt(splineEndX) == currentPt)
                {
                    endPtOccupy = true;
                }
            }

            if (!(endPtOccupy && startPtOccupy)) // remove this discontinuity from the list.
            {
                discontinuity.Remove(currentPt);
            }
        }

        // add new portals to the portal pool.

        foreach (int currentPt in discontinuity)
        {
            foreach (Transform currSplineTransform in GameObject.Find("Splines").transform)
            {
                GameObject currSpline = currSplineTransform.gameObject;
                Spline spline = currSpline.GetComponent<Spline>();
                float splineStartX = spline.GetControlPointPosition(0).x + spline.transform.position.x;
                splineStartX = (Mathf.Round(splineStartX * 10f)) / 10;
                float splineEndX =  spline.GetControlPointPosition(3).x + spline.transform.position.x;
                splineEndX = (Mathf.Round(splineEndX * 10f)) / 10;


                float splineStartY = spline.GetControlPointPosition(0).y + spline.transform.position.y;
                splineStartY = (Mathf.Round(splineStartY * 10f)) / 10;
                float splineEndY =  spline.GetControlPointPosition(3).y + spline.transform.position.y;
                splineEndY = (Mathf.Round(splineEndY * 10f)) / 10;
                
                if (Mathf.RoundToInt(splineStartX) == currentPt)
                {
                    if (GameObject.Find("DiscontinuityX = " + currentPt.ToString() + "Right") == null)
                    {
                        GameObject hole = Instantiate(HolePrefab, new Vector3(splineStartX, splineStartY, -10), Quaternion.identity);
                        hole.name = "DiscontinuityX = " + currentPt.ToString() + "Right";
                        hole.transform.SetParent(GameObject.Find("Portals").transform);
                    }
                }

                if (Mathf.RoundToInt(splineEndX) == currentPt)
                {
                    if (GameObject.Find("DiscontinuityX = " + currentPt.ToString() + "Left") == null)
                    {
                        GameObject hole = Instantiate(HolePrefab, new Vector3(splineEndX, splineEndY, -10), Quaternion.identity);
                        hole.name = "DiscontinuityX = " + currentPt.ToString() + "Left";
                        hole.transform.SetParent(GameObject.Find("Portals").transform);
                    }
                }
            }
        }


        // update portal sprites to all existing discontinuities.
        foreach (int currentPt in discontinuity)
        {
            try
            {
            GameObject promptWindow = GameObject.Find("HolePromptx = " + currentPt.ToString());
            GameObject choices = promptWindow.transform.GetChild(1).gameObject;
            foreach(Transform choiceTransform in choices.transform)
            {
                GameObject currentChoice = choiceTransform.gameObject;
                if (currentChoice.name == "Left" && currentChoice.GetComponent<Toggle>().isOn)
                {
                    GameObject.Find("DiscontinuityX = " + currentPt.ToString() + "Left").GetComponent<DiscontinuityDotController>().isHole = false;
                    GameObject.Find("DiscontinuityX = " + currentPt.ToString() + "Right").GetComponent<DiscontinuityDotController>().isHole = true;
                }
                
                if (currentChoice.name == "Right" && currentChoice.GetComponent<Toggle>().isOn)
                {
                    GameObject.Find("DiscontinuityX = " + currentPt.ToString() + "Left").GetComponent<DiscontinuityDotController>().isHole = true;
                    GameObject.Find("DiscontinuityX = " + currentPt.ToString() + "Right").GetComponent<DiscontinuityDotController>().isHole = false;
                }
            }
            }
            catch(System.NullReferenceException)
            {
                Debug.Log("Roller coaster is running, disabling UI editing window's update");
            }
        }
    }


    public void SetUpPortals()
    {
        foreach (Transform portalTransform in GameObject.Find("Portals").transform)
        {
            portalTransform.GetChild(4).gameObject.SetActive(false);
            portalTransform.localScale = new Vector3(1, 1, 1);
            portalTransform.Rotate(new Vector3(0, 90, 0));
            portalTransform.localPosition = new Vector3(portalTransform.localPosition.x, portalTransform.localPosition.y + 1, 0);
        }
    }


  
}
