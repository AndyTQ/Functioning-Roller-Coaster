using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flasher : MonoBehaviour
{
    public bool isFlashing;
    private int offset = 0;

    // Update is called once per frame
    void Update()
    {   

        if (isFlashing)
        {
            Color oldColor = gameObject.GetComponent<Image>().color;
            gameObject.GetComponent<Image>().color = new Color(255 - offset, 255 - offset, 255 - offset, 255);
            offset += 1;

            if (offset > 70)
            {
                
            }
        }
    }
}
