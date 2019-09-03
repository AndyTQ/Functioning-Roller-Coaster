using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InfoBoxController : MonoBehaviour
{
    private float levelSlope;
    private float levelXintercept;
    private float levelYintercept;
    public GameObject textPrefabTitle;
    public GameObject textPrefabValue;
    private Vector2 a;
    private Vector2 b;
    private Vector2 c;
    public GameObject functionInfoTitles;
    public GameObject functionInfoValue;
    public GameObject CheckWinController;
    // Update is called once per frame
    void Start()
    {
        functionInfoTitles = GameObject.Find("FunctionInfoTitles");

        if (CheckWinController.GetComponent<CheckWin>().level.Contains("A") && SceneManager.GetActiveScene().name != "A1") //first chapter
        {
            levelSlope = GetSlope();
            levelXintercept = GetXInterCept();
            levelYintercept = GetYInterCept();

            float[] values = new float[]{levelSlope, levelXintercept, levelYintercept};
            string[] titleTexts = new string[]{"Slope: ", "X-intercepts: ", "Y-intercepts: "};

            for (int i = 0; i < values.Length; i++)
            {
                GameObject title = Instantiate(textPrefabTitle, Vector3.zero, Quaternion.identity);
                title.GetComponent<TextMeshProUGUI>().text = titleTexts[i];
                title.transform.SetParent(functionInfoTitles.transform);

                GameObject value = Instantiate(textPrefabValue, Vector3.zero, Quaternion.identity);
                value.GetComponent<TextMeshProUGUI>().text = values[i].ToString("F2");
                value.transform.SetParent(functionInfoValue.transform);
                float scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.x;
                title.GetComponent<TextMeshProUGUI>().fontSize = 42 * scale;
                    
                value.GetComponent<TextMeshProUGUI>().fontSize = 42 * scale; 
            }
            // functionInfoValue.GetComponent<TextMeshProUGUI>().text = levelSlope.ToString("F2");
            // functionInfoValue.GetComponent<TextMeshProUGUI>().text += "\n" + levelXintercept.ToString("F2");
            // functionInfoValue.GetComponent<TextMeshProUGUI>().text += "\n" + levelYintercept.ToString("F2");
            // functionInfoTitles.GetComponent<TextMeshProUGUI>().text = "Slope:\nX-intercept:\nY-intercept:";
        }
        else if (SceneManager.GetActiveScene().name == "A1")
        {
            // do nothing
        }
        else if (SceneManager.GetActiveScene().name.Contains("D"))
        {
            functionInfoTitles.GetComponent<TextMeshProUGUI>().text = "Data Error: ";
            functionInfoValue.GetComponent<TextMeshProUGUI>().text = "404 ";
        }
        else
        {
            int levelNum;
            if (SceneManager.GetActiveScene().name.Contains("B"))
            {
                levelNum = 2;
            }
            else if (SceneManager.GetActiveScene().name.Contains("C"))
            {
                levelNum = 3;
            }
            else 
            {
                levelNum = -1;
            }
            if (levelNum != -1)
            {
                GameObject.Find("DropDownDataManager").GetComponent<CSVReader>().ReadCSVFile(levelNum);
            }
        }
    }
    void Update()
    {
        
    }

    private float GetSlope()
    {
        a = GameObject.Find("StartingSpline").transform.position + new Vector3(3, 0, 0);
        b = GameObject.Find("EndingSpline").transform.position;
        c = b - a;
        float res = (c).y / (c).x;
        return (Mathf.Round(res * 10)) / 10.0f;
    }

    private float GetXInterCept()
    {
        Vector3 intersection = new Vector3();
        bool found;
        intersection = GetIntersectionPointCoordinates(a, 
        b, 
        Vector3.zero,
        new Vector3(1f, 0,0),
        out found);
        return (Mathf.Round(intersection.x) * 10) / 10.0f;
    }

    private float GetYInterCept()
    {
        Vector3 intersection = new Vector3();
        bool found;
        intersection = GetIntersectionPointCoordinates(a, 
        b, 
        Vector3.zero,
        new Vector3(0, 1, 0),
        out found);
        return (Mathf.Round(intersection.x) * 10) / 10.0f;
    }

    /**
    Reference: https://blog.dakwamine.fr/?p=1943
    */
    public Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found)
    {
        float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
    
        if (tmp == 0)
        {
            // No solution!
            found = false;
            return Vector2.zero;
        }
    
        float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;
    
        found = true;

        Vector2 res = new Vector2(
            B1.x + (B2.x - B1.x) * mu,
            B1.y + (B2.y - B1.y) * mu
        );
    
        return res;
    }
}
