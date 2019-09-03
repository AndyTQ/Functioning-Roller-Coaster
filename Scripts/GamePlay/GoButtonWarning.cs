using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoButtonWarning : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!gameObject.GetComponent<Toggle>().interactable)
        {
            GameObject.Find("MessageController").GetComponent<MessageController>().PlayMessage("Your roller coaster is not connected yet! Please connect your starting point to your destination.");
        }
    }
}
