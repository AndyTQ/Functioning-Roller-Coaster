using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBackgroundController : MonoBehaviour
{
    public Material darkMaterial;
    private float darkness_threshold = 0.6f;
    private float curr_darkness = 0f;
    public bool isDark = false;
    public GameObject EditorCursor;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = Instantiate(darkMaterial);
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_BumpAmt", 0f);
        isDark = false;
        curr_darkness = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        if (isDark)
        {
            if (curr_darkness < darkness_threshold)
            {
                gameObject.GetComponent<MeshRenderer>().material.SetColor(
                    "_Color", new Color(1 - curr_darkness, 1 - curr_darkness, 1 - curr_darkness, 1f)
                );
                curr_darkness += 0.05f;
            }
        }
        else
        {
            if (curr_darkness > 0f)
            {
                gameObject.GetComponent<MeshRenderer>().material.SetColor(
                    "_Color", new Color(1 - curr_darkness, 1 - curr_darkness, 1 - curr_darkness, 1f)
                );
                curr_darkness -= 0.05f;
            }
        }
        // Make the background darker
        if (EditorCursor.activeInHierarchy)
        {
            isDark = true;
        }
        else{
            isDark = false;
        }
    }
}
