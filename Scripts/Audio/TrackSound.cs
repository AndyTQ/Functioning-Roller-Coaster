using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackSound : MonoBehaviour
{
    public AudioClip sound;
    public bool isPlaying;


    private Button button {get {return GetComponent<Button>(); }}
    private AudioSource source {get {return GetComponent<AudioSource>();}}

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
    }

    public void PlaySound(string name)
    {
        source.PlayOneShot((AudioClip) Resources.Load("Audio/" + name));
    }

}
