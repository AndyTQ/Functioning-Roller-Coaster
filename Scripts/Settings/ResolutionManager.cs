using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    public void ChangeResolution()
    {
        var Dropdown = gameObject.GetComponent<TMP_Dropdown>();
        var resolutions = Dropdown.options[Dropdown.value].text.Split('x');
        SetResolution(int.Parse(resolutions[0]), int.Parse(resolutions[1]));
    }

    private void SetResolution(int width, int height)
    {
        Screen.SetResolution(width,height,Screen.fullScreen);
    }
}
