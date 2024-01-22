using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceController : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    private float loudnessSensibility;
    private float threshold = 30f;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        threshold = PlayerPrefs.GetFloat("VoiceThreshold", 0f);
        loudnessSensibility = PlayerPrefs.GetFloat("VoiceSensitivity", 50f);
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMic() * loudnessSensibility;
        Debug.Log(loudness);

        if (loudness >= threshold)
        {
            // Voice is above the threshold, trigger player jump
            player.TriggerJump();
        }

    }
}
