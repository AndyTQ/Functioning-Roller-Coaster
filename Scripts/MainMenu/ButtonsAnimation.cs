using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator SelectAnimation;
    public bool selected;
    // Start is called before the first frame update
  

     // The mesh goes red when the mouse is over it...
    public void OnPointerEnter(PointerEventData eventData)
    {  
            selected = true;
            SelectAnimation.Play("MainMenuOnSelect");
  
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
        SelectAnimation.Play("MainMenuDeselect");
    }

}
