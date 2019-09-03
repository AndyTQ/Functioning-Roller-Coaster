using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefinitionBoxManager : MonoBehaviour
{
    GameObject definitionBox;
    void Start()
    {
        definitionBox = GameObject.Find("InfoBox").transform.GetChild(6).gameObject;
    }

 
    // Start is called before the first frame update
    public void SetDefinitionBox()
    {   
        string term = extractTerm(gameObject.GetComponent<TextMeshProUGUI>().text);
        GameObject termImage = definitionBox.transform.GetChild(4).gameObject;
        Sprite termImageSprite = Resources.Load<Sprite>("Images/" + term.ToLower());
        termImage.GetComponent<Image>().overrideSprite = termImageSprite;
        definitionBox.SetActive(true);
        term = term[0].ToString().ToUpper() + term.Substring(1);
        definitionBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = term;
        TextMeshProUGUI definition = definitionBox.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        switch (term.ToLower())
        {
            case "slope":
                definition.text = "The slope is defined as the ratio of the vertical change between two points, the rise, to the horizontal change between the same two points, the run. The slope of a line is usually represented by the letter m. (x1, y1) represents the first point whereas (x2, y2) represents the second point.";
                break;
            case "x-intercepts":
                definition.text = "X-intercept is the x-coordinate of a point where a line, curve, or surface intersects the x-axis.";
                break;
            case "y-intercepts":
                definition.text = "Y-intercept is the y-coordinate of a point where a line, curve, or surface intersects the y-axis.";
                break;
            case "reflection":
                definition.text = "A reflection of an object is a 'flip' of that object across a line.";
                break;
            case "vertical shift":
                definition.text = "A vertical shift is when the graph literally moves vertically, up or down. The movement is all based on what happens to the y-value of the graph. The y-axis of a coordinate plane is the vertical axis. When a function shifts vertically, the y-value changes.";
                break;
            case "horizontal shift":
                definition.text = "Horizontally translating a graph is equivalent to shifting the base graph left or right in the direction of the x-axis. A graph is translated k units horizontally by moving each point on the graph k units horizontally. Definition. For the base function f (x) and a constant k, the function given by.";
                break;
            case "stretch":
                definition.text = "A stretch in which a graph is distorted horizontally or vertically.";
                break;
            case "amplitude":
                definition.text = "The Amplitude is the height from the center line to the peak (or to the trough). Or we can measure the height from highest to lowest points and divide that by 2.";
                break;
            case "period":
                definition.text = "The Period goes from one peak to the next (or from any point to the next matching point).";
                break;
            default:
                definition.text = "404 Not Found!";
                break;
        }
    }

    private string extractTerm(string rawTerm)
    {
        return rawTerm.Substring(0, rawTerm.IndexOf(":"));
    }

}
