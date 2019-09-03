using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageController : MonoBehaviour
{
    public TextMeshProUGUI textMesh; 
    public Animator messageAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMessage(string message)
    {
        textMesh.text = message;
        messageAnimator.Play("MessageFadeOut", default, 0);
    }
}
