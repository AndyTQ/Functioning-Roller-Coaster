 using UnityEngine;
 using UnityEngine.UI;
 using UnityEngine.EventSystems;
 using System.Collections;
 
 public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {    
    
 
    public void OnPointerEnter( PointerEventData ped ) {
        GameObject.Find("Hover").GetComponent<ClickSound>().PlaySound("main_hover");
    }
 
    public void OnPointerDown( PointerEventData ped ) {
        GameObject.Find("Hover").GetComponent<ClickSound>().PlaySound("main_start");
    }    
 }