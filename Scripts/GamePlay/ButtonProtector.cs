using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonProtector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        GridCursor.allowClick = false;
        SplineEditorCursor.allowClick = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GridCursor.allowClick = true;
        SplineEditorCursor.allowClick = true;
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            GridCursor.allowClick = true;
            SplineEditorCursor.allowClick = true;
        }
    }

    void OnDisable() // Important: restore allowClick for GridCursor before destroying!
    {
        GridCursor.allowClick = true;
        SplineEditorCursor.allowClick = true;
    }

    void OnDestroy() // Important: restore allowClick for GridCursor before destroying!
    {
        GridCursor.allowClick = true;
        SplineEditorCursor.allowClick = true;
    }
}
