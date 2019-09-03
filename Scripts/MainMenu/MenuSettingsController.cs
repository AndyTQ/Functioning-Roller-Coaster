using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettingsController : MonoBehaviour
{
    public bool PerformingExit;
    public Animator logoAnimator;
    public Animator buttonsAnimator;
    void Update()
    {
        if (PerformingExit)
        {
            if (gameObject.GetComponent<CanvasGroup>().alpha <= 0)
            {
                PerformingExit = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        PerformingExit = true;
        gameObject.GetComponent<Animator>().Play("SettingsMenuFadeOut");
        logoAnimator.Play("MainLogoFadeIn");
        buttonsAnimator.Play("MainMenuButtonsFadeIn");
    }

}
