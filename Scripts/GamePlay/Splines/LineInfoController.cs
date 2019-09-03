using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Splines;
using TMPro;
public class LineInfoController : MonoBehaviour
{
    public GameObject LinearInfo;
    public GameObject NonLinearInfo;
    public GameObject DefaultInfo;
    void Update()
    {
        Spline currentEditingSpline = getEditingSpline();
        if (currentEditingSpline == null) // not in edit mode.
        {
            LinearInfo.SetActive(false);
            NonLinearInfo.SetActive(false);
            DefaultInfo.SetActive(true); 
        }
        else 
        {
            if (currentEditingSpline.isLinear())
            {
                LinearInfo.SetActive(true);
                NonLinearInfo.SetActive(false);
                DefaultInfo.SetActive(false);

                float slope = currentEditingSpline.GetSlope();
                GameObject data = GameObject.Find("StraightLineData");
                // Slope
                data.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = slope.ToString();
                // Horizontal Shift
                float HorizontalShift = currentEditingSpline.getLinearHorizontalShift();
                string res;
                if (currentEditingSpline.isHorizontal() || HorizontalShift > 100)
                {
                    res = "N/A";
                }
                else
                {
                    res = HorizontalShift.ToString();
                }
                data.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = res;

                float VerticalShift = currentEditingSpline.getLinearVerticalShift();
                data.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = VerticalShift.ToString();
            }
            else 
            {
                LinearInfo.SetActive(false);
                NonLinearInfo.SetActive(true);
                DefaultInfo.SetActive(false);
            }
        }
    }
    
    private Spline getEditingSpline()
    {
        foreach(Transform spline in GameObject.Find("Splines").transform)
        {
            if (spline.gameObject.GetComponent<Spline>().onEdit)
            {
                return spline.gameObject.GetComponent<Spline>();
            }
        }
        return null;
    }

    /**
        Precondition: The editing spline is a curve.
     */
    public void FlattenEditingSpline()
    {
        Spline currentSpline = getEditingSpline();
        currentSpline.FlattenCurve();
    }

   
}

