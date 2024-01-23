using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // If the AudioSource component doesn't exist, add it
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Add a default AudioClip (you can also set this in the Unity Editor)
        // audioSource.clip = yourDefaultClip;
    }

    // Function to play the click sound
    public void PlayClickSound()
    {
        // Play the assigned AudioClip when called
        audioSource.Play();
    }

    // Example: Attach this function to the button's click event in the Unity Editor
    public void OnButtonClick()
    {
        // Call the PlayClickSound function when the button is clicked
        PlayClickSound();
    }
}
