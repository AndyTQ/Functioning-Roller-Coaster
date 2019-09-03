using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUpdater : MonoBehaviour
{
    public GameObject cursorSprite;

    void Update()
    {
        if (cursorSprite.activeInHierarchy)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }
}
