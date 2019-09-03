using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZenFulcrum.Track;
using UnityEngine.SceneManagement;
using WSMGameStudio.Splines;
using UnityEngine.UI;
using TMPro;

public class CheckWin : MonoBehaviour
{ 
    private Camera cam;  
    private float starPt2 = 1180;
    private float starPt3 = 1350;
    private float currentAccuracy = 100.0f;
    public GameObject cart;
    public bool scoreCounting;
    private float ACCURACY_BAR_LENGTH = 1600.0f;
    private float starPt1 = 980;
    private float timer = 0;
    public int resolution;
    public Animator ongoingPanelScore;
    public Animator ongoingPanelAccuracy;
    public string level;
    private int MAXSCORE = 100 * 20;
    private Vector2[] solution;
    private float sumDelta = 0;
    public int sumScore = 0;
    public int starCount = 0;
    private int currentCheckPt = 0;
    public bool win = false;
    public float domainStart;
    public float domainEnd;

    private bool[] played = new bool[]{false, false, false};


    private List<float> accuracySummary;
    // Update is called once per frame
    void Start()
    {
        cam = Camera.main;
        accuracySummary = new List<float>();
        buildSolution();
        GameObject.Find("LevelInfo").GetComponent<TextMeshProUGUI>().text 
            = GameObject.Find("LevelTitleResult").GetComponent<TextMeshProUGUI>().text 
            = "Level " + level[0] + "-" + level.Substring(1);
        if (!level.Contains("D")) GameObject.Find("RemovedHint").SetActive(false);
    }

    void OnGUI()
    {
        if (cart.GetComponent<SplineFollower>().endLevel)
        {
            DrawAccuracyLine();
        }
    }

    private void DrawAccuracyLine()
    {
        Vector2 topLeft = GameObject.Find("AccuracyFrame").transform.position;
        float height = GameObject.Find("AccuracyFrame").GetComponent<RectTransform>().sizeDelta.y * 1.2f * 0.56f;
        float width = GameObject.Find("AccuracyFrame").GetComponent<RectTransform>().sizeDelta.x * 0.8f 
        * GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.x;
        float widthoffset = width / resolution;
        // topLeft = new Vector2(topLeft.x, topLeft.y - (height) * 1f);
        // Drawing.DrawLine(ScreenSpaceConversion(topLeft), new Vector2(topLeft.x + width, topLeft.y), Color.black, 2, true);
        for(int i = 0; i < accuracySummary.Count; i++)
        {
            if (i != 0)
            {
                float currNode = 1 - accuracySummary[i] / 100;
                float prevNode = 1 - accuracySummary[i - 1] / 100;

                Vector2 prevPt = new Vector2(topLeft.x + widthoffset * (i - 1), topLeft.y  - height * prevNode);
                Vector2 currPt = new Vector2(topLeft.x + widthoffset * (i), topLeft.y  - height * currNode);

                Vector2 topRight = new Vector2(topLeft.x + width, topLeft.y - height * currNode);
                // Debug.Log(ScreenSpaceConversion(currPt).x + "," + ScreenSpaceConversion(topRight).x);
                if (ScreenSpaceConversion(currPt).x > ScreenSpaceConversion(topRight).x) break;

                if (currNode > 0.4)
                {
                    Drawing.DrawLine(ScreenSpaceConversion(prevPt), ScreenSpaceConversion(currPt), Color.red, 2, true);
                }
                else
                {
                    Drawing.DrawLine(ScreenSpaceConversion(prevPt), ScreenSpaceConversion(currPt), Color.green, 2, true);
                }

            }
        }
    }

    private Vector2 ScreenSpaceConversion(Vector2 a)
    {
        return new Vector2(a.x, cam.pixelHeight - a.y);
    }

    public Vector2[] GetSolution()
    {
        return solution;
    }

    private float accuracyF(float delta){
        return 10.0f * Mathf.Pow(Mathf.Exp(1), -(0.5f * delta - Mathf.Log(10, Mathf.Exp(1))));
    }

    




    void Update()
    {
        if (scoreCounting) // TODO: e.g. divide into 100 segment, analyze at each segment checkpoint.
        {
            if (cart.GetComponent<SplineFollower>().currentSpline.name != "StartingSpline" && cart.GetComponent<SplineFollower>().currentSpline.name != "EndingSpline")
            {
                if (cart.transform.position.x > solution[currentCheckPt].x)
                {
                    Debug.Log(currentCheckPt);
                    float delta = Mathf.Abs(solution[currentCheckPt].y - cart.transform.position.y);
                    float signedDelta = cart.transform.position.y - solution[currentCheckPt].y;
                    float accuracy = accuracyF(delta);
                    // if (signedDelta < 0) accuracy *= -1; // check if delta is negative.
                    sumDelta += delta;
                    
                    float accuracyToDisplay = Mathf.Round(accuracy * 0.1f) / 0.1f;
              
                    if (accuracyToDisplay + 10 > 100f)
                    {
                        accuracyToDisplay = 100f;
                    }
                    else
                    {  
                        accuracyToDisplay = accuracyToDisplay + 10;
                    }
                    accuracySummary.Add(Mathf.Round(accuracyToDisplay * 0.1f) / 0.1f);
                    
                    if (delta < 0.5)
                    {
                        sumScore += 20;
                        updateScoreBar(3);
                    }
                    else if (delta < 1)
                    {
                        sumScore += 2;
                        updateScoreBar(2);
                    }
                    else
                    {
                        sumScore += 1;
                        updateScoreBar(1);
                    }
                    

                    GameObject.Find("ScoreNumber").GetComponent<TextMeshProUGUI>().text = sumScore.ToString();
                    GameObject.Find("accuracyNumber").GetComponent<TextMeshProUGUI>().text = accuracy.ToString("f1") + "%";

                    currentCheckPt = currentCheckPt < (resolution - 1) ? currentCheckPt + 1 : currentCheckPt;

                    // set the small triangle.
                    float thresholdTriangleX = 720.0f + (Screen.width / 2 +  34.0f );
                    float triangleX = (100.0f - accuracy) * 3 + (Screen.width / 2 + 34.0f);
                    if (triangleX > thresholdTriangleX) triangleX = thresholdTriangleX;
                    if (signedDelta < 0) triangleX = (Screen.width / 2 + 34.0f) - (100.0f - accuracy) * 3;
                 
                    GameObject triangle = GameObject.Find("smallTriangle");
                    triangle.transform.position = new Vector3(
                    triangleX,
                    triangle.transform.position.y,
                    triangle.transform.position.z);
                }
            }
        }
    }
    public void toggleState(){
        scoreCounting = !scoreCounting;
    }

    public void hardRestartGame() {
        GameObject.Find("First Person Camera").SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*************************************************************/
    /*************************************************************/
    /********************* SOLUTION BUILDER **********************/
    /*************************************************************/
    /*************************************************************/
    public void buildSolution()
    {
        solution = new Vector2[resolution];
        for (int i = 0; i < solution.Length; i++)
        {
            float x = domainStart + (domainEnd - domainStart) / resolution * i;
            solution[i] = new Vector2(x, yOutput(x));
        }
    }

    public float yOutput(float x)
    {
        switch (level)
        {
            case "A1":
                return 0;
            case "A2":
                return x;
            case "A3":
                return -x;
            case "A4":
                return x + 1;
            case "A5":
                return 0.5f * x;
            case "A6":
                return 2f * x;
            case "A7":
                return -0.5f * x - 3.0f;
            case "B1":
                return x * x;
            case "B2":
                return - x * x;
            case "B3":
                return - x * x + 4;
            case "B4":
                return 2 * x * x;
            case "B5":
                return - (0.5f) * x * x;
            case "B6":
                return (x - 1) * (x - 1);
            case "B7":
                return (x + 3) * (x + 3) - 6;
            case "B8":
                return (x + 2) * (x - 2);
            case "B9":
                return -2 * (x * x - 1);
            case "B10":
                return -0.5f * (x * x - 1f) + 5f;
            case "B11":
                return x * x - x - 2;
            case "B12":
                return x * x * x;
            case "B13":
                return -x * x * x; 
            case "C1":
                return Mathf.Sin(x);
            case "C2":
                return Mathf.Cos(x);
            case "C3":
                return 4 * Mathf.Sin(x);
            case "C4":
                return Mathf.Sin(x);
            case "C5":
                return Mathf.Sin(0.5f * x);
            case "C6":
                return Mathf.Cos(2 * x);
            case "C7":
                return Mathf.Cos(2 * (x + Mathf.PI/2));
            case "C8":
                return (4 * Mathf.Sin(x)) + 2;
            case "C9":
                return 2 * Mathf.Sin(0.5f * (x - 2 * Mathf.PI)) - 3;
            case "D1":
                if (x <= 0)
                {
                    return 0;
                }
                else
                {
                    return -x;
                }
            case "D2":
                if (x <= 0)
                {
                    return x + 4;
                }
                else if (0 < x && x < 6)
                {
                    return 0;
                }
                else
                {
                    return 6;
                }
            case "D3":
                if (x <= 0)
                {
                    return (x + 2) * (x + 2);
                }
                else
                {
                    return (x - 2) * (x - 2);
                }
            case "D4":
                if (x <= -1)
                {
                    return (3 * x + 5);
                }
                else
                {
                    return (x - 2) * (x + 4);
                }
            case "D5":
                if (x <= 1)
                {
                    return -(x - 1) * (x - 1);
                }
                else
                {
                    return 0.5f * x + 4.5f;
                }
            case "D6":
                if (x < 1)
                {
                    return (x - 1) * (x - 1);
                }
                else if (1 <= x && x < 2)
                {
                    return x * x;
                }
                else
                {
                    return -(x - 1) * (x - 1);
                }
            case "D7":
                if (0 <= x && x < 8)
                {
                    return -(0.5f * x - 2) * (0.5f * x - 2)  + 9;
                }
                else
                {
                    return (0.2f * x) * (0.2f * x);
                }
            case "D8":
                if (x < -2)
                {
                    return 2 * x + 8;
                }
                else
                {
                    return x * x;
                }
            case "D9":
                return Mathf.Log(x, 10);
            case "D10":
                return Mathf.Log(x, 2);
            case "D11":
                return -Mathf.Log(x, 2);
            case "D12":
                return Mathf.Abs(x);
            case "D13":
                return Mathf.Abs(x + 2);
            case "D14":
                return 5 - Mathf.Abs(x - 1);
            default:
                return 0;
        }
    }
    /*************************************************************/
    /*************************************************************/
    /******************* END SOLUTION BUILDER ********************/
    /*************************************************************/
    /*************************************************************/

    public void toggleOngoingPanel()
    {
        ongoingPanelAccuracy.Play("openAccuracyPanel");
        ongoingPanelScore.Play("openScorePanel");
    }
    public void togglePullingScore()
    {
        ongoingPanelScore.Play("pullDownScorePanel");
    }
    public void updateScoreBar(float currentAccuracyLevel)
    {
        if (scoreCounting)
        {   
            float offset;
            if (currentAccuracyLevel == 3)
            {
                offset = (ACCURACY_BAR_LENGTH - 20.0f) / resolution;
            }
            else if (currentAccuracyLevel == 2)
            {
                offset = (ACCURACY_BAR_LENGTH - 20.0f) / resolution / 10.0f;
            }
            else if(currentAccuracyLevel == 1)
            {
                offset = (ACCURACY_BAR_LENGTH - 20.0f) / resolution / 20.0f;
            } 
            else {
                offset = 0;
            }
            GameObject scoreBar = GameObject.Find("ScoreBar");
            GameObject tipGlow = scoreBar.transform.GetChild(3).gameObject;
            GameObject barMask = scoreBar.transform.GetChild(4).gameObject;
            RectTransform rtTip = tipGlow.GetComponent<RectTransform>();
            RectTransform rtMask = barMask.GetComponent<RectTransform>();
            if (rtMask.sizeDelta.x < ACCURACY_BAR_LENGTH)
            {
                rtMask.sizeDelta = new Vector2 (rtMask.sizeDelta.x + offset, rtMask.sizeDelta.y);
                rtTip.position = new Vector2 (rtTip.position.x + offset, rtTip.position.y);
            }
            else
            {
                tipGlow.SetActive(false);
            }
            // Check if any star has been reached. 
            if (rtMask.sizeDelta.x > starPt1)
            {
                GameObject.Find("Star1").GetComponent<Image>().color = new Color(255, 255, 255, 1);
                GameObject.Find("MainStar1").GetComponent<Image>().color = new Color(255, 255, 255, 1);
                win = true;
                starCount = 1;
                if (!played[0])
                {
                    GameObject.Find("StarlightenSound").GetComponent<ClickSound>().PlaySound("sound_onestar");
                    played[0] = true;
                    
                    GameDataManager.currentPlayer.coinCount ++;
                }
            }
            if (rtMask.sizeDelta.x > starPt2)
            {
                GameObject.Find("Star2").GetComponent<Image>().color = new Color(255, 255, 255, 1);
                GameObject.Find("MainStar2").GetComponent<Image>().color = new Color(255, 255, 255, 1);
                starCount = 2;
                if (!played[1])
                {
                    GameObject.Find("StarlightenSound").GetComponent<ClickSound>().PlaySound("sound_twostars");
                    played[1] = true;
    
                    
                    GameDataManager.currentPlayer.coinCount ++;
                }
            }
            if (rtMask.sizeDelta.x > starPt3)
            {
                GameObject.Find("Star3").GetComponent<Image>().color = new Color(255, 255, 255, 1);
                GameObject.Find("MainStar3").GetComponent<Image>().color = new Color(255, 255, 255, 1);
                starCount = 3;
                if (!played[2])
                {
                    GameObject.Find("StarlightenSound").GetComponent<ClickSound>().PlaySound("sound_threestars");
                    GameObject.Find("GoodJobSound").GetComponent<ClickSound>().PlaySound("goodjob");
                    played[2] = true;

                    
                    GameDataManager.currentPlayer.coinCount ++;
                }
            }
        }
    }

}
