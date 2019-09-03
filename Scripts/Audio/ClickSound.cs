using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour
{
    public AudioClip sound;


    private Button button {get {return GetComponent<Button>(); }}
    private AudioSource source {get {return GetComponent<AudioSource>();}}

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
    }

    public void PlaySound()
    {
        source.PlayOneShot(sound);
    }

    public void PlaySound(string name)
    {
        source.PlayOneShot((AudioClip) Resources.Load("Audio/" + name));
    }

    public void StopSound()
    {
        source.Stop();
    }

}
