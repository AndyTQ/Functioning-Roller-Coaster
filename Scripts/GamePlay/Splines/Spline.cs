using System;
using System.Collections.Generic;
using UnityEngine;

namespace WSMGameStudio.Splines
{
    public class Spline : MonoBehaviour
    {
        // drag utilities
        private Vector3 mOffset;
        private float mZCoord;


        private Camera cam;
        public bool onEdit = false;
        public bool selected = false;
        private bool pointsInitialized = false;
        private bool buildingToolOn = false;
        public GameObject editorCursor;
        public GameObject gridCursor;
        private Ray ray;
        private RaycastHit hit;

        private GameObject spawnControlPointStart;
        private GameObject spawnControlPointEnd;
        private GameObject spawnAnchorPointStart;
        private GameObject spawnAnchorPointEnd;
        [Range(15f, 100f)]
        public float newCurveLength = 15f;

        [SerializeField]
        private Vector3[] _controlPointsPositions;

        [SerializeField]
        private Quaternion[] _controlPointsRotations;

        [SerializeField]
        private BezierControlPointMode[] _modes;

        [SerializeField]
        private bool _loop;


        void Start()
        {
            cam = Camera.main;
            gridCursor = GameObject.Find("Cursors").transform.GetChild(0).gameObject;
            editorCursor = GameObject.Find("Cursors").transform.GetChild(1).gameObject;
        }

        bool AnyOnEdit()
        {
            foreach(Transform spline in GameObject.Find("Splines").transform)
            {
                if (spline.gameObject.GetComponent<Spline>().onEdit)
                {
                    return true;
                }
            }
            return false;
        }

        void Update()
        {
            if (onEdit)
            {
                TutorialNextMsg();
                if (!pointsInitialized)
                {
                        GameObject controllerPrefab = Resources.Load<GameObject>("Prefabs/ControlPointDisplay");
                        GameObject anchorPrefab = Resources.Load<GameObject>("Prefabs/AnchorPointDisplay");
                        Vector3 newPos;
                        newPos = cam.WorldToScreenPoint(_controlPointsPositions[0] + transform.position);
                        newPos.z = 0;
                        spawnAnchorPointStart = Instantiate(anchorPrefab, newPos, Quaternion.identity); 
                        newPos = cam.WorldToScreenPoint(_controlPointsPositions[3] + transform.position);
                        newPos.z = 0;
                        spawnAnchorPointEnd = Instantiate(anchorPrefab, newPos, Quaternion.identity);
                        spawnAnchorPointStart.name = "AnchorPointStart" + gameObject.name;
                        spawnAnchorPointEnd.name = "AnchorPointEnd" + gameObject.name;
                        spawnAnchorPointStart.transform.SetParent(GameObject.Find("Canvas").transform);
                        spawnAnchorPointEnd.transform.SetParent(GameObject.Find("Canvas").transform);

                        newPos = cam.WorldToScreenPoint(_controlPointsPositions[1] + transform.position);
                        newPos.z = 0;
                        spawnControlPointStart = Instantiate(controllerPrefab, newPos, Quaternion.identity); 
                        newPos = cam.WorldToScreenPoint(_controlPointsPositions[2] + transform.position);
                        newPos.z = 0;
                        spawnControlPointEnd = Instantiate(controllerPrefab, newPos, Quaternion.identity);
                        spawnControlPointStart.name = "ControlPointStart" + gameObject.name;
                        spawnControlPointEnd.name = "ControlPointEnd" + gameObject.name;
                        spawnControlPointStart.transform.SetParent(GameObject.Find("Canvas").transform);
                        spawnControlPointEnd.transform.SetParent(GameObject.Find("Canvas").transform);

                        // Set all points' parents to the same place.
                        spawnAnchorPointStart.transform.SetParent(GameObject.Find("ControlPoints").transform);
                        spawnAnchorPointEnd.transform.SetParent(GameObject.Find("ControlPoints").transform);
                        spawnControlPointStart.transform.SetParent(GameObject.Find("ControlPoints").transform);
                        spawnControlPointEnd.transform.SetParent(GameObject.Find("ControlPoints").transform);

                        pointsInitialized = true;
                }
                else
                {   // commented out -- do not change anchor points. 
                    // Debug.Log("Starting pt is" + (transform.position));
                    // Vector2 newPos = cam.ScreenToWorldPoint(spawnAnchorPointStart.transform.position);
                    // transform.position = newPos;
                    Vector2 newTransform = cam.ScreenToWorldPoint(spawnControlPointStart.transform.position) - transform.position;
                    SetControlPointPositionAbsolute(1, newTransform);
                    newTransform = cam.ScreenToWorldPoint(spawnControlPointEnd.transform.position) - transform.position;
                    SetControlPointPositionAbsolute(2, newTransform);
                    newTransform = cam.ScreenToWorldPoint(spawnAnchorPointEnd.transform.position) - transform.position;
                    SetControlPointPositionAbsolute(3, newTransform);
                    newTransform = cam.ScreenToWorldPoint(spawnAnchorPointStart.transform.position) - transform.position;
                    SetControlPointPositionAbsolute(0, newTransform);
                    UpdateMeshRenderer();
                }
                int zOffset = 10;
                // render the control point lines.
                GameObject ControlLinesStart = GameObject.Find("ControlPointLine0");
                GameObject ControlLinesEnd = GameObject.Find("ControlPointLine1");

                LineRenderer ControlLinesStartRenderer = ControlLinesStart.GetComponent<LineRenderer>();
                LineRenderer ControlLinesEndRenderer = ControlLinesEnd.GetComponent<LineRenderer>();

                ControlLinesStartRenderer.enabled = true;
                ControlLinesEndRenderer.enabled = true;
                
                Vector3[] ControlLinesStartRendererPos = new Vector3[]{transform.position + _controlPointsPositions[0], transform.position + _controlPointsPositions[1]};
                Vector3[] ControlLinesEndRendererPos = new Vector3[]{transform.position + _controlPointsPositions[3], transform.position + _controlPointsPositions[2]};

                ControlLinesStartRendererPos[0].Set(ControlLinesStartRendererPos[0].x, ControlLinesStartRendererPos[0].y, ControlLinesStartRendererPos[0].z + zOffset);
                ControlLinesStartRendererPos[1].Set(ControlLinesStartRendererPos[1].x, ControlLinesStartRendererPos[1].y, ControlLinesStartRendererPos[1].z + zOffset);
                ControlLinesEndRendererPos[0].Set(ControlLinesEndRendererPos[0].x, ControlLinesEndRendererPos[0].y, ControlLinesEndRendererPos[0].z + zOffset);
                ControlLinesEndRendererPos[1].Set(ControlLinesEndRendererPos[1].x, ControlLinesEndRendererPos[1].y, ControlLinesEndRendererPos[1].z + zOffset);
                
                ControlLinesStartRenderer.SetPositions(ControlLinesStartRendererPos);
                ControlLinesEndRenderer.SetPositions(ControlLinesEndRendererPos);
            }
            else
            {  
                pointsInitialized = false;
                if (GameObject.Find("AnchorPointStart" + gameObject.name) != null)
                {
                Destroy(GameObject.Find("AnchorPointStart" + gameObject.name));
                Destroy(GameObject.Find("AnchorPointEnd" + gameObject.name));
                Destroy(GameObject.Find("ControlPointStart" + gameObject.name));
                Destroy(GameObject.Find("ControlPointEnd" + gameObject.name));
                }
            }
        }

        /**
            Returns true if and only if the spline is linear.
         */
        public bool isLinear()
        {
            Vector2 a = _controlPointsPositions[1] - _controlPointsPositions[0];
            Vector2 b = _controlPointsPositions[2] - _controlPointsPositions[3];
            
            bool res = Mathf.Approximately(Vector2.Dot(a.normalized, b.normalized), -1f);
            return res;
        }


        /**
            Returns true if and only if the spline is horizontal.
         */
        public bool isHorizontal()
        {
            return Mathf.Abs(_controlPointsPositions[1].y) < 0.1f
            && Mathf.Abs(_controlPointsPositions[1].y) < 0.1f
            &&  Mathf.Abs(_controlPointsPositions[1].y) < 0.1f;
        }

        // void OnMouseDown()
        // {
        //     mZCoord = Camera.main.WorldToScreenPoint(
        //     gameObject.transform.position).z;
        //     // Store offset = gameobject world pos - mouse world pos
        //     mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        // }

        // private Vector3 GetMouseAsWorldPoint()
        // {
        //     // Pixel coordinates of mouse (x,y)
        //     Vector3 mousePoint = Input.mousePosition;
        //     // z coordinate of game object on screen
        //     mousePoint.z = mZCoord;
        //     // Convert it to world points
        //     return Camera.main.ScreenToWorldPoint(mousePoint);
        // }
        // void OnMouseDrag()
        // {
        //     transform.position = GetMouseAsWorldPoint() + mOffset;
        //     spawnAnchorPointEnd.transform.position += mOffset;
        //     spawnAnchorPointStart.transform.position  += mOffset;
        //     spawnControlPointEnd.transform.position  += mOffset;
        //     spawnControlPointStart.transform.position  += mOffset;
        // }

        public float getLinearHorizontalShift()
        {
            Vector3 intersection = new Vector3();
            bool found;
            intersection = GetIntersectionPointCoordinates(transform.position, 
            _controlPointsPositions[1] - _controlPointsPositions[0] + transform.position, 
            Vector3.zero,
            new Vector3(1f, 0,0),
            out found);
            return (Mathf.Round(intersection.x) * 10) / 10.0f;
        }

        public float getLinearVerticalShift()
        {
            Vector3 intersection = new Vector3();
            bool found;
            intersection = GetIntersectionPointCoordinates(transform.position, 
            _controlPointsPositions[1] - _controlPointsPositions[0] + transform.position, 
            Vector3.zero,
            new Vector3(0, 1f,0),
            out found);

            return (Mathf.Round(intersection.y) * 10) / 10.0f;
        }

        private void TutorialNextMsg()
        {
            if (GameObject.Find("CheckWinController").GetComponent<CheckWin>().level.Contains("B") && TutorialManager.currentIndex == 8)
            {
                GameObject.Find("TutorialManager").GetComponent<TutorialManager>().NextMessage();
            }
        }




        /**
            Referenc: https://blog.dakwamine.fr/?p=1943
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



        #region PROPERTIES

        public bool Loop
        {
            get { return _loop; }
            set
            {
                _loop = value;
                if (value == true)
                {
                    _modes[_modes.Length - 1] = _modes[0];
                    SetControlPointPosition(0, _controlPointsPositions[0]);
                }
            }
        }

        public int ControlPointCount
        {
            get { return _controlPointsPositions == null ? 0 : _controlPointsPositions.Length; }
        }

        public int CurveCount
        {
            get { return (_controlPointsPositions == null ? 0 : _controlPointsPositions.Length - 1) / 3; }
        }

        #endregion

        /// <summary>
        /// Get control point by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetControlPointPosition(int index)
        {
            if (_controlPointsPositions == null)
                Reset();

            return _controlPointsPositions[index];
        }

        /// <summary>
        /// Get rotation by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Quaternion GetControlPointRotation(int index)
        {
            return _controlPointsRotations[index];
        }

        /// <summary>
        /// Set control point rotation
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public void SetControlPointRotation(int index, Quaternion rotation)
        {
            if (index % 3 == 0)
            {
                Quaternion deltaRotation = rotation * Quaternion.Inverse(_controlPointsRotations[index]);
                if (_loop)
                {
                    if (index == 0)
                    {
                        _controlPointsRotations[1] *= deltaRotation;
                        _controlPointsRotations[_controlPointsRotations.Length - 2] *= deltaRotation;
                        _controlPointsRotations[_controlPointsRotations.Length - 1] = rotation;
                    }
                    else if (index == _controlPointsPositions.Length - 1)
                    {
                        _controlPointsRotations[0] = rotation;
                        _controlPointsRotations[1] *= deltaRotation;
                        _controlPointsRotations[index - 1] *= deltaRotation;
                    }
                    else
                    {
                        _controlPointsRotations[index - 1] *= deltaRotation;
                        _controlPointsRotations[index + 1] *= deltaRotation;
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        _controlPointsRotations[index - 1] *= deltaRotation;
                    }
                    if (index + 1 < _controlPointsRotations.Length)
                    {
                        _controlPointsRotations[index + 1] *= deltaRotation;
                    }
                }
            }

            _controlPointsRotations[index] = rotation;
        }

        /// <summary>
        /// Set control point by index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        public void SetControlPointPosition(int index, Vector3 point)
        {
            if (index % 3 == 0)
            {
                Vector3 deltaPosition = point - _controlPointsPositions[index];
                if (_loop)
                {
                    if (index == 0)
                    {
                        _controlPointsPositions[1] += deltaPosition;
                        _controlPointsPositions[_controlPointsPositions.Length - 2] += deltaPosition;
                        _controlPointsPositions[_controlPointsPositions.Length - 1] = point;
                    }
                    else if (index == _controlPointsPositions.Length - 1)
                    {
                        _controlPointsPositions[0] = point;
                        _controlPointsPositions[1] += deltaPosition;
                        _controlPointsPositions[index - 1] += deltaPosition;
                    }
                    else
                    {
                        _controlPointsPositions[index - 1] += deltaPosition;
                        _controlPointsPositions[index + 1] += deltaPosition;
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        _controlPointsPositions[index - 1] += deltaPosition;
                    }
                    if (index + 1 < _controlPointsPositions.Length)
                    {
                        _controlPointsPositions[index + 1] += deltaPosition;
                    }
                }
            }

            _controlPointsPositions[index] = point;
            EnforceMode(index);
        }


        public void SetControlPointPositionAbsolute(int index, Vector3 point)
        {
            _controlPointsPositions[index] = point;
            EnforceMode(index);
        }

        /// <summary>
        /// Get control point mode by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BezierControlPointMode GetControlPointMode(int index)
        {
            return _modes[(index + 1) / 3];
        }

        /// <summary>
        /// Set control point mode
        /// </summary>
        /// <param name="index"></param>
        /// <param name="mode"></param>
        public void SetControlPointMode(int index, BezierControlPointMode mode, bool enforceMode)
        {
            int modeIndex = (index + 1) / 3;
            _modes[modeIndex] = mode;
            if (_loop)
            {
                if (modeIndex == 0)
                {
                    _modes[_modes.Length - 1] = mode;
                }
                else if (modeIndex == _modes.Length - 1)
                {
                    _modes[0] = mode;
                }
            }

            if (enforceMode)
                EnforceMode(index);
        }

        /// <summary>
        /// Make sure selected control point mode is applied
        /// </summary>
        /// <param name="index"></param>
        public void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;
            BezierControlPointMode mode = _modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !_loop && (modeIndex == 0 || modeIndex == _modes.Length - 1))
            {
                return;
            }

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                {
                    fixedIndex = _controlPointsPositions.Length - 2;
                }
                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= _controlPointsPositions.Length)
                {
                    enforcedIndex = 1;
                }
            }
            else
            {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= _controlPointsPositions.Length)
                {
                    fixedIndex = 1;
                }
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                {
                    enforcedIndex = _controlPointsPositions.Length - 2;
                }
            }

            Vector3 middle = _controlPointsPositions[middleIndex];
            Vector3 enforcedTangent = middle - _controlPointsPositions[fixedIndex];
            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, _controlPointsPositions[enforcedIndex]);
            }
            _controlPointsPositions[enforcedIndex] = middle + enforcedTangent;
        }

        /// <summary>
        /// Get point
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetPoint(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = _controlPointsPositions.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetPoint(
                _controlPointsPositions[i], _controlPointsPositions[i + 1], _controlPointsPositions[i + 2], _controlPointsPositions[i + 3], t));
        }


        /// <summary>
        /// Get point rotation at spline postion t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Quaternion GetRotation(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = _controlPointsRotations.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return Bezier.GetPointRotation(_controlPointsRotations[i], _controlPointsRotations[i + 1], _controlPointsRotations[i + 2], _controlPointsRotations[i + 3], t);
        }

        /// <summary>
        /// Get velocity
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetVelocity(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = _controlPointsPositions.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetFirstDerivative(
                _controlPointsPositions[i], _controlPointsPositions[i + 1], _controlPointsPositions[i + 2], _controlPointsPositions[i + 3], t)) - transform.position;
        }

        /// <summary>
        /// Get direction
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        /// <summary>
        /// Get a list of spline oriented points based on the number os steps
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        public List<OrientedPoint> GetOrientedPoints(int steps)
        {
            List<OrientedPoint> ret = new List<OrientedPoint>();

            float stepPercentage = 1f / steps;
            float t = 0;

            while (t < 1f)
            {
                OrientedPoint orientedPoint = GetOrientedPoint(t);
                ret.Add(orientedPoint);
                t += stepPercentage;
            }

            return ret;
        }

        /// <summary>
        /// Get point position and rotation on spline position t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public OrientedPoint GetOrientedPoint(float t)
        {
            return new OrientedPoint(GetPoint(t), GetRotation(t) * Quaternion.LookRotation(GetDirection(t)));
        }

        /// <summary>
        /// Reset spline
        /// </summary>
        public void Reset()
        {
            _loop = false;
            _controlPointsPositions = new Vector3[4];
            _controlPointsRotations = new Quaternion[4];

            for (int i = 0; i < _controlPointsPositions.Length; i++)
            {
                _controlPointsPositions[i] = new Vector3(0f, 0f, i * (newCurveLength / 3));
                _controlPointsRotations[i] = Quaternion.identity;
            }

            _modes = new BezierControlPointMode[]
            {
                BezierControlPointMode.Aligned,
                BezierControlPointMode.Aligned
            };
        }

        /// <summary>
        /// Reset all control points rotations
        /// </summary>
        public void ResetRotations()
        {
            ResetRotations(Quaternion.identity);
        }

        /// <summary>
        /// Set all control points rotations to new rotation value
        /// </summary>
        public void ResetRotations(Quaternion newRotation)
        {
            for (int i = 0; i < _controlPointsRotations.Length; i++)
            {
                _controlPointsRotations[i] = newRotation;
            }
        }

        /// <summary>
        /// Add new curve to spline
        /// </summary>
        public void AddCurve()
        {
            //Add positions
            Vector3 lastPointPosition = _controlPointsPositions[_controlPointsPositions.Length - 1];
            Vector3 lastPointDirection = transform.InverseTransformDirection(GetDirection(1));
            Quaternion lastPointRotation = GetRotation(1);

            Array.Resize(ref _controlPointsPositions, _controlPointsPositions.Length + 3);
            Array.Resize(ref _controlPointsRotations, _controlPointsRotations.Length + 3);

            //Add the 3 new control points
            for (int i = 3; i > 0; i--)
            {
                //Calculate new position based on last point direction
                lastPointPosition += (lastPointDirection * (newCurveLength / 3));
                //Position
                _controlPointsPositions[_controlPointsPositions.Length - i] = lastPointPosition;
                //Rotation
                _controlPointsRotations[_controlPointsRotations.Length - i] = lastPointRotation;//Quaternion.identity;
            }

            //Add modes
            Array.Resize(ref _modes, _modes.Length + 1);
            _modes[_modes.Length - 1] = _modes[_modes.Length - 2];
            EnforceMode(_controlPointsPositions.Length - 4);

            if (_loop)
            {
                _controlPointsPositions[_controlPointsPositions.Length - 1] = _controlPointsPositions[0];
                _controlPointsRotations[_controlPointsRotations.Length - 1] = _controlPointsRotations[0];
                _modes[_modes.Length - 1] = _modes[0];
                EnforceMode(0);
            }
        }

        /// <summary>
        /// Remove the last curve (Disables loop property)
        /// </summary>
        public void RemoveCurve()
        {
            if (CurveCount <= 1)
            {
                Debug.Log("Spline has only one curve. Cannot remove last curve.");
                return;
            }

            Loop = false;

            Array.Resize(ref _controlPointsPositions, _controlPointsPositions.Length - 3);
            Array.Resize(ref _controlPointsRotations, _controlPointsRotations.Length - 3);
            Array.Resize(ref _modes, _modes.Length - 1);
        }

        /// <summary>
        /// Adjust control points vertical position to follow terrain elevations
        /// </summary>
        [ExecuteInEditMode]
        public void FollowTerrain()
        {
            Vector3 pointPosition = Vector3.zero;
            Vector3 origin = Vector3.zero;
            RaycastHit hit = new RaycastHit();

            //Disable colliders
            MeshCollider[] colliders = GetComponentsInChildren<MeshCollider>();
            bool[] collidersState = new bool[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                collidersState[i] = colliders[i].enabled;
                colliders[i].enabled = false;
            }

            //Adjust main control points
            for (int i = 0; i < _controlPointsPositions.Length; i += 3)
            {
                FollowTerrrainControlPointIteration(out pointPosition, out origin, out hit, i);
            }

            //Adjust auxiliar control points
            for (int i = 0; i < _controlPointsPositions.Length; i++)
            {
                if (i % 3 == 0)
                    continue;

                FollowTerrrainControlPointIteration(out pointPosition, out origin, out hit, i);
            }

            //Renable colliders
            for (int i = 0; i < colliders.Length; i++)
                colliders[i].enabled = collidersState[i];

            UpdateMeshRenderer();
        }

        /// <summary>
        /// Reset control points heights
        /// </summary>
        public void Flatten()
        {
            Vector3 firstPointPosition = GetControlPointPosition(0);

            for (int i = 0; i < _controlPointsPositions.Length; i += 3)
            {
                SetControlPointPosition(i, new Vector3(_controlPointsPositions[i].x, firstPointPosition.y, _controlPointsPositions[i].z));
            }

            //Adjust auxiliar control points
            for (int i = 0; i < _controlPointsPositions.Length; i++)
            {
                if (i % 3 == 0)
                    continue;

                SetControlPointPosition(i, new Vector3(_controlPointsPositions[i].x, firstPointPosition.y, _controlPointsPositions[i].z));
            }

            UpdateMeshRenderer();
        }

        public void FlattenCurve()
        {
            Vector2 a = _controlPointsPositions[0];
            Vector2 d = _controlPointsPositions[3];

            Vector3 b = a + (d-a) * 0.25f;
            Vector3 c = d + (a-d) * 0.25f;

            SetControlPointPositionAbsolute(1, b);
            SetControlPointPositionAbsolute(2, c);

            spawnControlPointStart.transform.position =  cam.WorldToScreenPoint(b + transform.position);
            spawnControlPointEnd.transform.position =  cam.WorldToScreenPoint(c + transform.position);

        }

        public float GetSlope()
        {   
            Vector2 splineLine = _controlPointsPositions[3] - _controlPointsPositions[0];
            float res = (splineLine).y / (splineLine).x;
            return (Mathf.Round(res * 10)) / 10.0f;
        }

        /// <summary>
        /// Updates Spline Mesh renderer (if exists)
        /// </summary>
        private void UpdateMeshRenderer()
        {
            SplineMeshRenderer splineMeshRenderer = GetComponent<SplineMeshRenderer>();

            if (splineMeshRenderer != null)
                splineMeshRenderer.ExtrudeMesh();
        }

        /// <summary>
        /// Find terrain elevation and update control point height to match terrain
        /// </summary>
        /// <param name="pointPosition"></param>
        /// <param name="origin"></param>
        /// <param name="hit"></param>
        /// <param name="i"></param>
        private void FollowTerrrainControlPointIteration(out Vector3 pointPosition, out Vector3 origin, out RaycastHit hit, int i)
        {
            pointPosition = GetControlPointPosition(i);

            origin = transform.TransformPoint(pointPosition);
            //origin = pointPosition + transform.position;

            if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
                FollowTerrainElevation(hit, i);
            else if (Physics.Raycast(origin, Vector3.up, out hit, Mathf.Infinity))
                FollowTerrainElevation(hit, i);
        }

        /// <summary>
        /// Set control point height to match terrain elevation
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="i"></param>
        private void FollowTerrainElevation(RaycastHit hit, int i)
        {
            SetControlPointMode(i, BezierControlPointMode.Free, false);

            if (i == 0)
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);

            SetControlPointPosition(i, transform.InverseTransformPoint(hit.point));
        }

        /// <summary>
        /// Return bezier curve total distance
        /// </summary>
        /// <returns></returns>
        public float GetTotalDistance()
        {
            float distance = 0f;

            for (float t = 0f; t < 1f; t += 0.1f)
            {
                distance += Vector3.Distance(GetPoint(t), GetPoint(t + 0.1f));
            }
            return distance;
        }
    }
}
