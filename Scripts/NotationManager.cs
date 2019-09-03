using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotationManager : MonoBehaviour
{
    public static bool isfx = true;
    public GameObject functionText;
    public void Start()
    {
        gameObject.GetComponent<Slider>().value = isfx ? 0 : 1;
    }
    // Update is called once per frame
    public void UpdateNotation()
    {
        isfx = Mathf.Approximately(gameObject.GetComponent<Slider>().value, 1) ? false : true;
        if (!isfx)
        {
            string text = functionText.GetComponent<TextMeshProUGUI>().text;
            functionText.GetComponent<TextMeshProUGUI>().text = text.Replace("f(x) ", "   y ");
        }
        else
        {
            string text = functionText.GetComponent<TextMeshProUGUI>().text;
            functionText.GetComponent<TextMeshProUGUI>().text = text.Replace("   y ", "f(x) ");
        }
        
    }
}
