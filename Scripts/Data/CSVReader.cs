using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class CSVReader : MonoBehaviour
{
    public GameObject functionInfoTitles;
    public GameObject functionInfoValues;
    public GameObject textPrefabTitle;
    public GameObject textPrefabValue;
    private string[] titles;

    // Start is called before the first frame update
    void Start()
    {
        functionInfoTitles = GameObject.Find("FunctionInfoTitles");
        functionInfoValues = GameObject.Find("FunctionInfoValues");
    }

    // Update is called once per frame
    public void ReadCSVFile(int levelNum)
    {
        var text = Resources.Load("LevelInfo/levelInfo" + levelNum.ToString());
    
        // convert string to stream
        byte[] byteArray = Encoding.UTF8.GetBytes(text.ToString());
        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        MemoryStream stream = new MemoryStream(byteArray);
    
        StreamReader strReader = new StreamReader(stream);
        bool eof = false;
        int row = 1;
        while (!eof)
        {
            string dataString = strReader.ReadLine();
            if (dataString == null)
            {
                eof = true;
                break;
            }
            var data_values = dataString.Split(',');
            replaceComma(data_values);
            if (row == 1)
            {
                titles = data_values;
            }
            if (row - 1 == int.Parse(GameObject.Find("CheckWinController").GetComponent
            <CheckWin>().level.Substring(1)))
            {
                PlaceDisplay(data_values);
            }
            Debug.Log(data_values);
            row += 1;
        }
    }

    void PlaceDisplay(string[] data)
    {
        for (int i = 1; i < titles.Length; i++)
        {
            if (!data[i].Equals("none"))
            {
               
                GameObject title = Instantiate(textPrefabTitle, Vector3.zero, Quaternion.identity);
                title.GetComponent<TextMeshProUGUI>().text = titles[i] + ": ";
                title.transform.SetParent(functionInfoTitles.transform);
                GameObject value = Instantiate(textPrefabValue, Vector3.zero, Quaternion.identity);
                value.GetComponent<TextMeshProUGUI>().text = data[i];
                value.transform.SetParent(functionInfoValues.transform);
                if(SceneManager.GetActiveScene().name.Contains("C"))
                {
                    
                    float scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.x;
                    title.GetComponent<TextMeshProUGUI>().fontSize = 42 * scale;
                    
                    value.GetComponent<TextMeshProUGUI>().fontSize = 42 * scale;   
                }
                
            }
        }
    }

    void replaceComma(string[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i].Replace(";", ",");
        }
    }
}
