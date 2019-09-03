using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{   
    public GameObject firstPersonCamera;
    public GameObject MainCamera;
    public GameObject EditorUIs;

 

    public void Show2DView() {
        firstPersonCamera.SetActive(false);
        MainCamera.SetActive(true);
    }
    
    public void ShowFirstPersonView() {
        firstPersonCamera.SetActive(true);
        MainCamera.SetActive(false);
        EditorUIs.SetActive(false);
    }
}
