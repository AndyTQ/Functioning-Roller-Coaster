using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Splines;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
    public GameObject cameraGroup;
    public GameObject promptGroup;
    public Camera cameraA;
    public Camera cameraB;
    public float upperBound;
    public float lowerBound;
    public float leftBound;
    public float rightBound;
    public static bool allowDrag = true;
    private Vector3 dragOrigin;



    void Update()
    {   
        Vector3 pos;
        Vector3 move;
        GameObject gridCursor;
        foreach (Transform spline in GameObject.Find("Splines").transform)
        {
            if (spline.gameObject.GetComponent<Spline>().onEdit)
            {
                return;
            }
        }
        if (GameObject.Find("EditorCursor") == null)
        {
            return;
        }
        if ((gridCursor = GameObject.Find("GridCursor")) != null)
        {
            return;
        }
        else if (GridCursor.selected)
        {
            return;
        }
        if (allowDrag)
        {
            var d = Input.mouseScrollDelta.y;
  
            if (d > 0f && cameraA.orthographicSize > lowerBound)
            {
                cameraA.orthographicSize --;
                cameraB.orthographicSize --;
                // scroll up
            
            }
            else if (d < 0f && cameraA.orthographicSize < upperBound)
            {
                cameraA.orthographicSize ++;
                cameraB.orthographicSize ++;
                // scroll down
            }
            

            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(0))
            {
                return;
            }

            pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            move = new Vector3(-pos.x, -pos.y, 0);
            bool xExceed = cameraA.transform.position.x + move.x > rightBound || cameraA.transform.position.x + move.x < leftBound;
            bool yExceed = cameraA.transform.position.y + move.y > upperBound || cameraA.transform.position.y + move.y < lowerBound;
          
            if (!xExceed && yExceed)
            {
                cameraGroup.transform.Translate(new Vector3(move.x, 0, 0), Space.World);  
            }
            else if (xExceed && !yExceed)
            {
                cameraGroup.transform.Translate(new Vector3(0, move.y, 0), Space.World);  
            }
            else if (xExceed && yExceed)
            {
                cameraGroup.transform.Translate(new Vector3(0, 0, 0), Space.World);  
            }
            else 
            {
                cameraGroup.transform.Translate(new Vector3(move.x, move.y, 0), Space.World);  
            }
        }
    }
 
 

}
