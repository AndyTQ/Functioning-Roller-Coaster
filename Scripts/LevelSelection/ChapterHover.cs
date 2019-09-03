using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChapterHover : MonoBehaviour
{

    public void OnMouseOver()
    {
        Debug.Log("Mouse is over GameObject");
        transform.localScale += new Vector3(2F, 2f, 2f); //adjust these values as you see fit
    }


    public void OnMouseExit()
    {
        Debug.Log("Mouse is no longer on GameObject.");
        transform.localScale = new Vector3(1, 1, 1);  // assuming you want it to return to its original size when your mouse leaves it.
    }

}
