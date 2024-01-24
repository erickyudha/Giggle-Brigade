using System.Collections;
using UnityEngine;

public class RandomSoundManager : MonoBehaviour
{
    public AudioClip[] soundEffects;
    private AudioSource audioSource;

    public float minInterval = 2f;  // Minimum interval between sound effects
    public float maxInterval = 5f;  // Maximum interval between sound effects

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSoundEffect());
    }

    private IEnumerator PlayRandomSoundEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            if (!audioSource.isPlaying)
            {
                PlayRandomClip();
            }
        }
    }

    private void PlayRandomClip()
    {
        if (soundEffects.Length > 0)
        {
            int randomIndex = Random.Range(0, soundEffects.Length);
            AudioClip randomClip = soundEffects[randomIndex];
            audioSource.clip = randomClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No sound effects assigned to the RandomSoundManager.");
        }
    }
}
