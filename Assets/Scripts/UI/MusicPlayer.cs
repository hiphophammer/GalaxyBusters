using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audio;
    
    private bool paused;

    void Start()
    {
        // Fetch the AudioSource from the GameObject
        audio = GetComponent<AudioSource>();
        
        // Initialize paused to false
        paused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (paused == false)
            {
                audio.Pause();
                paused = true;
            }
            else
            {
                audio.Play();
                paused = false;
            }
        }
    }
}
