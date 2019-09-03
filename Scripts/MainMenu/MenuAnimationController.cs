using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimationController : MonoBehaviour
{
    public GameObject buttons;
    public GameObject logo;
    public void FadeOutLogoAndButtons()
    {
        buttons.GetComponent<Animator>().Play("MainMenuButtonsFadeOut");
        logo.GetComponent<Animator>().Play("MainLogoFadeOut");
    }
}
