using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    public Vector2[] solution;
    public CheckWin checkWin;
    public GameObject SolutionDots;
    public GameObject solutionDot;
    public GameObject ButtonHint;
    public GameObject ButtonCDPrefab;
    public TextMeshProUGUI PriceTag;

    void Update()
    {
        // Update price tag.
        try{
            PriceTag.text = "x " + GameDataManager.currentPlayer.hintPrice.ToString();
        }
        catch (System.NullReferenceException)
        {
            PriceTag.text = "x 0";
        }
    }

    public void DisplayHint()
    {
        try
        {
            if (GameDataManager.currentPlayer.coinCount - GameDataManager.currentPlayer.hintPrice < 0)
            {
                GameObject.Find("MessageController").GetComponent<MessageController>().PlayMessage("You don't have enough coins."); 
            }
            else{
                GetSolution();
            }
        }
        catch (System.NullReferenceException)
        {
            GetSolution();
        }
       

    }

    private void GetSolution()
    {
        if (solution.Length == 0)
        {
            solution = checkWin.GetSolution();
        }

        GameObject cdLayer = Instantiate(ButtonCDPrefab, ButtonHint.transform);
        cdLayer.transform.SetParent(GameObject.Find("Toolbox").transform);

        for (int i = 0; i < solution.Length; i ++)
        {
            GameObject currDot = Instantiate(solutionDot, Camera.main.WorldToScreenPoint((Vector3) solution[i]), Quaternion.identity);
            currDot.transform.SetParent(SolutionDots.transform);
        }
        // increment the price.
        GameDataManager.currentPlayer.coinCount -= GameDataManager.currentPlayer.hintPrice;
        GameDataManager.currentPlayer.hintPrice ++;

        // everytime the price and coin is changed, we store the new data.
        GameDataManager.currentPlayer.SavePlayer();
    }
}
