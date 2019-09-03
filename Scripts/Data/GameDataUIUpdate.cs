using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameDataUIUpdate : MonoBehaviour
{
    public GameObject coinDisplay;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        try
        {
        coinDisplay.GetComponent<TextMeshProUGUI>().text = "x " + GameDataManager.currentPlayer.coinCount.ToString();
        }
        catch (System.NullReferenceException)
        {
            coinDisplay.GetComponent<TextMeshProUGUI>().text = "None";
        }
    }
}
