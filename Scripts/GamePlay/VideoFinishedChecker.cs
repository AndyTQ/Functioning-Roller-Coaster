using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoFinishedChecker : MonoBehaviour
{
    private double time;
    private double currentTime;
      // Use this for initialization
    void Start () {
        time = GameObject.Find("Video Player").GetComponent<VideoPlayer>().clip.length;
    }
 
   
    // Update is called once per frame
    void Update () {
        currentTime = GameObject.Find("Video Player").GetComponent<VideoPlayer>().time;
        if ((int) currentTime >= (int) time) {
            SceneManager.LoadScene("A1");
        }
    }
}
