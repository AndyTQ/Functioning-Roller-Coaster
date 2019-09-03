using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour
{
    public GameObject GridCursor;
    public void ToggleState() {
        GridCursor.SetActive(!GridCursor.activeInHierarchy); //this changes the state from on to off and vice-versa
    }
}
