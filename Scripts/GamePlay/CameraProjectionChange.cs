using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Splines;
[RequireComponent(typeof(Camera))]
public class CameraProjectionChange : MonoBehaviour
 {
    public Camera camera;
    public Animator TransitionAnimator;
    public GameObject cameraGroup;
    public GameObject triggerCart;
    public float ProjectionChangeTime = 0.5f;
    public bool ChangeProjection = false;


    private Vector3 cameraStart1;
    private Vector3 cameraEnd1;
    private Vector3 rotationEnd1 = Vector3.zero;
 
    private bool _changing = false;
    public bool _shiftingPosition = false;
    private float _currentT = 0.0f;
    private float _animT = 0.0f;


    void Start()
    {
        cameraGroup = TransitionAnimator.gameObject;
    }

    public void startChangingProject()
    {
        ChangeProjection = true;
    }
 
    private void Update()
    {
        if(_changing)
        {
            ChangeProjection = false;
        }
        else if(ChangeProjection)
        {
            TransitionAnimator.Play("MainCameraShiftPosition");
            _changing = true;
            _currentT = 0.0f;
        }
    }

    private void ShiftCameraPosition()
    {
        if(_animT < 1.00f)
        {
            _animT += (Time.deltaTime / ProjectionChangeTime);
            camera.transform.position = Vector3.Slerp(cameraStart1, cameraEnd1, _animT);
        }
        else // t == 1
        {
            _shiftingPosition = false;
            if (TransitionAnimator.enabled)
            {
                camera.transform.position = Vector3.Slerp(cameraStart1, cameraEnd1, 1f);
                TransitionAnimator.enabled = false;
            }
            triggerCart.GetComponent<SplineFollower>().toggleStart();
            GameObject.Find("CameraController").GetComponent<CameraSwitch>().ShowFirstPersonView();
        }
    }

     private void LateUpdate()
     {
        if(_shiftingPosition)
        {
          

            if (rotationEnd1 == Vector3.zero) // Have not initialized yet
            {
                cameraStart1  = camera.transform.position;
                rotationEnd1 = new Vector3(0, 90, 0);
                cameraEnd1 = GameObject.Find("First Person Camera").transform.position;
                Debug.Log(cameraStart1);
                Debug.Log(cameraEnd1);
            }
           
            ShiftCameraPosition();
        }
        
        if(!_changing)
        {
            return;
        }

        var currentlyOrthographic = camera.orthographic;
        Matrix4x4 orthoMat, persMat;
        if(currentlyOrthographic)
        {
            orthoMat = camera.projectionMatrix;

            camera.orthographic = false;
            camera.ResetProjectionMatrix();
            persMat = camera.projectionMatrix;
        }
        else
        {
            persMat = camera.projectionMatrix;

            camera.orthographic = true;
            camera.ResetProjectionMatrix();
            orthoMat = camera.projectionMatrix;
        }
        camera.orthographic = currentlyOrthographic;

        _currentT += (Time.deltaTime / ProjectionChangeTime);
        if(_currentT < 1.0f)
        {
            if(currentlyOrthographic)
            {
                camera.projectionMatrix = MatrixLerp(orthoMat, persMat, _currentT * _currentT);
            }
            else
            {
                camera.projectionMatrix = MatrixLerp(persMat, orthoMat, Mathf.Sqrt(_currentT));
            }
        }
        else
        {
            _changing = false;
           
            camera.orthographic = !currentlyOrthographic;
            camera.ResetProjectionMatrix();
        }
     }
 
     private Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
     {
         t = Mathf.Clamp(t, 0.0f, 1.0f);
         var newMatrix = new Matrix4x4();
         newMatrix.SetRow(0, Vector4.Lerp(from.GetRow(0), to.GetRow(0), t));
         newMatrix.SetRow(1, Vector4.Lerp(from.GetRow(1), to.GetRow(1), t));
         newMatrix.SetRow(2, Vector4.Lerp(from.GetRow(2), to.GetRow(2), t));
         newMatrix.SetRow(3, Vector4.Lerp(from.GetRow(3), to.GetRow(3), t));
         return newMatrix;
     }
 }