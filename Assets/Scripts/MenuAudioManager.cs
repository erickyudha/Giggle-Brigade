using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

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
