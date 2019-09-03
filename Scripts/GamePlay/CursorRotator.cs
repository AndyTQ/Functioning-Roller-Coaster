using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRotator : MonoBehaviour
{
    public GameObject cursor;
   
    // Update is called once per frame
    void Update()
    {
        cursor.transform.Rotate(0,0,1f);
    }
}
