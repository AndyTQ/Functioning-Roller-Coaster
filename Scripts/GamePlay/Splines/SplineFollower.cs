using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WSMGameStudio.Splines
{
    public class SplineFollower : MonoBehaviour
    {
        public Spline currentSpline;
        private float speed;
        private bool playingTrackSound = false;
        public float initialSpeed = 8;
        public SplineFollowerMode mode;
        public GameObject scoreboardButtons;

        private float checkSphereRadius = 0.5f;
        private float _cicleDuration;
        private float _progress;
        private bool _goingForward = false;
        private float _distance;

        public bool endLevel = false;

        private void Start()
        {
            if (currentSpline == null)
                return;

            _distance = currentSpline.GetTotalDistance();
            _progress = 0.5f; //reset progress
            _cicleDuration = 0;
        }

       


        // 20190621: Changed FixedUPdate to update.
        private void Update()
        {
            FollowSpline();
            if (_progress == 1 && currentSpline.name != "EndingSpline")
            {
                Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, checkSphereRadius);
                bool nextSplineSelected = false;
                // Switch to the next spline
                // For chapter 4: Check if a portal is reached.
                if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Contains("D"))
                {
                    if (ReachedDiscontinuity(gameObject.transform.position.x))
                    {
                        currentSpline = FindDestinationSpline(gameObject.transform.position.x);
                        nextSplineSelected = true;
                    }
                }

                if (!nextSplineSelected)
                {
                    foreach (Collider collider in colliders)
                    {   
                        if (collider.gameObject.name.Contains("Spline") && collider.gameObject.name != currentSpline.gameObject.name)
                        {
                            GameObject nextSplineObject = collider.gameObject;
                            currentSpline = nextSplineObject.GetComponent<Spline>();
                            break;
                        }
                    }
                }
                

                _distance = currentSpline.GetTotalDistance();
                _progress = 0;
            }

            else if (currentSpline.name == "EndingSpline")
            {
                // Ask scoreboard to display the end result.
                if (!endLevel)
                { 
                    GameObject.Find("TrackSound").GetComponent<ClickSound>().StopSound();
                    // Start displaying results.
                    scoreboardButtons.SetActive(true);
                    GameObject.Find("ScoreBar").GetComponent<Animator>().Play("pullDownScorePanel");
                    GameObject.Find("AccuracyBar").GetComponent<Animator>().Play("closeAccuracyPanel");
                    endLevel = true;
                    if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().win)
                    {
                        GameObject.Find("ResultMessage").GetComponent<TextMeshProUGUI>().text = "Cleared!";
                        // Start Confetti effect.
                        gameObject.transform.GetChild(2).gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject.Find("ResultMessage").GetComponent<TextMeshProUGUI>().text = "Try again next time.";
                        GameObject.Find("ButtonNextLevel").GetComponent<Button>().interactable = false;
                    }

                    GameObject.Find("DarkProgressBar").GetComponent<Animator>().Play("Idle");

                    string levelStr = GameObject.Find("CheckWinController").GetComponent<CheckWin>().level;

                    int currentLevel = int.Parse(GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Substring(1));
                    int nextLevel = currentLevel + 1;
                    string nextLevelStr = levelStr[0] + nextLevel.ToString();
                    if (!GameDataManager.currentPlayer.levelProgress.ContainsKey(nextLevelStr))
                    {
                        GameObject.Find("ButtonNextLevel").GetComponent<Button>().interactable = false;
                        GameObject.Find("FinishedAllLevelsText").GetComponent<TextMeshProUGUI>().text 
                        = "You have reached the end of this level. \nPlease return to the menu for more levels.";
                    }
                    CheckWin checkWinController = GameObject.Find("CheckWinController").GetComponent<CheckWin>();


                    if (checkWinController.win)
                    {
                        int starObtained = checkWinController.starCount;
                        int scoreObtained = checkWinController.sumScore;
                        Player currentPlayer = GameDataManager.currentPlayer;
                        if (starObtained > GameDataManager.currentPlayer.levelStar[GameDataManager.currentLevel])
                        {
                            // update highest star obtained
                            GameDataManager.currentPlayer.levelStar[GameDataManager.currentLevel] = starObtained;
                        }

                        if (scoreObtained > GameDataManager.currentPlayer.levelHiScore[GameDataManager.currentLevel])
                        {
                            // update highest score obtained
                            GameDataManager.currentPlayer.levelHiScore[GameDataManager.currentLevel] = scoreObtained;
                        }
                    }

                    
        

                    GameDataManager.currentPlayer.SavePlayer();
                }
                speed -= 0.5f;
                if (speed < 0) speed = 0;
            }
        }
        /**
            Check whether the cart reached a point of discontinuity.
         */
        public bool ReachedDiscontinuity(float currentX)
        {
            foreach(int x in GameObject.Find("HoleController").GetComponent<HoleController>().discontinuity)
            {
                if (Mathf.Approximately(currentX, x))
                {
                    return true;
                }
            }
            return false;
        }

        /**
            FOR CHAPTER 4: Find the destination of the portal given discontunuity.
         */
        public Spline FindDestinationSpline(float discontinuityX)
        {
            foreach (Transform currSplineTransform in GameObject.Find("Splines").transform)
            {
                GameObject currSpline = currSplineTransform.gameObject;
                Spline spline = currSpline.GetComponent<Spline>();
                float splineStartX = spline.GetControlPointPosition(0).x + spline.transform.position.x;
                splineStartX = (Mathf.Round(splineStartX * 10f)) / 10;
                if (Mathf.Approximately(splineStartX, discontinuityX))
                {
                    return spline;
                }
            }
            return null; // destination not found!
        }

        /// <summary>
        /// Follow currentSpline path
        /// </summary>
        private void FollowSpline()
        {
            if (currentSpline == null)
                return;

            _goingForward = (speed > 0);
            _cicleDuration = speed == 0f ? 0f : _distance / Mathf.Abs(speed);

            if (_goingForward)
            {
                if (!playingTrackSound) 
                {
                    playingTrackSound = true;
                    // Play sound
                    GameObject.Find("TrackSound").GetComponent<ClickSound>().PlaySound("TrackSound");
                }
                if (_cicleDuration > 0f)
                    _progress += Time.deltaTime / _cicleDuration;

                if (_progress > 1f)
                {
                    switch (mode)
                    {
                        case SplineFollowerMode.StopAtTheEnd:
                            _progress = 1f;
                            break;
                        case SplineFollowerMode.Loop:
                            _progress -= 1f;
                            break;
                        case SplineFollowerMode.PingPong:
                            _progress = 1f;
                            speed *= -1;
                            break;
                    }
                }
            }
            else
            {
                if (_cicleDuration > 0f)
                    _progress -= Time.deltaTime / _cicleDuration;

                if (_progress < 0f)
                {
                    switch (mode)
                    {
                        case SplineFollowerMode.StopAtTheEnd:
                            _progress = 0f;
                            break;
                        case SplineFollowerMode.Loop:
                            _progress += 1f;
                            break;
                        case SplineFollowerMode.PingPong:
                            _progress = 0;
                            speed *= -1;
                            break;
                    }
                }
            }
            Vector3 position = currentSpline.GetPoint(_progress);
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, 1f);
            Vector3 LookPos = position + currentSpline.GetDirection(_progress);
            transform.LookAt(LookPos);



            // Rotate the cube by converting the angles into a quaternion.
            // Quaternion reverseRotation = new Quaternion(0, 0, 180.0f);


            
          
        
            if (derivative(_progress).x < 0)
            {
                GameObject.Find("LuggCart").transform.localScale = new Vector3(1, -1 , 1);
                try
                {
                    Vector3 rot = GameObject.Find("First Person Camera").transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x,rot.y,180);
                    GameObject.Find("First Person Camera").transform.rotation = Quaternion.Euler(rot);
                } 
                catch(System.NullReferenceException) 
                {
                    
                }

            }
            else
            {
                GameObject.Find("LuggCart").transform.localScale = new Vector3(1, 1 , 1);
                
                try
                {
                    Vector3 rot = GameObject.Find("First Person Camera").transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x,rot.y,0);
                    GameObject.Find("First Person Camera").transform.rotation = Quaternion.Euler(rot);
                } 
                catch(System.NullReferenceException) 
                {

                }
            }
            // TODO: fix line 135.
            // this line determines the position of the tranform. try to modify it!
        }

        private Vector2 derivative(float progress)
        {
            if (progress + 0.0001f < 1){
                Vector2 h = new Vector2(0.0001f, 0.0001f);
                Vector2 xAndH = currentSpline.GetPoint(progress + 0.0001f);
                Vector2 x = currentSpline.GetPoint(progress);
                Vector2 resVec = xAndH - x;
                resVec = new Vector2(resVec.x / 0.0001f, resVec.y / 0.0001f);
                return resVec;
            } 
            else 
            {
                return Vector2.zero;
            }
        }

        

        public void toggleStart()
        {
            if (currentSpline == null)
                return;
            _distance = currentSpline.GetTotalDistance();
            speed = initialSpeed;
        }
    }
}
