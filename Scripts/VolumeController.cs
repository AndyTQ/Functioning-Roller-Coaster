using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public static float volume;

    void Start()
    {
        gameObject.GetComponent<Slider>().value = AudioListener.volume;
    }
    

    // Update is called once per frame
    public void UpdateVolume()
    {
        volume = gameObject.GetComponent<Slider>().value;
    }

    void Update()
    {
        AudioListener.volume = volume;
        gameObject.GetComponent<Slider>().value = volume;
    }
}
