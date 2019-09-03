using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscontinuityDotController : MonoBehaviour
{
    GameObject background;
    public bool isHole = false;
    // Start is called before the first frame update
    void Start()
    {
        background = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.ParticleSystem.MainModule main = background.GetComponent<ParticleSystem>().main;
        if (isHole)
        {
            main.startColor = Color.white;
        }
        else 
        {
            main.startColor = Color.black;
        }
    }
}
