using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueVoiceManager : MonoBehaviour
{
    [System.Serializable]
    public enum ActionType
    {
        NPCDialogue,
        PlayerResponse
    }

    [System.Serializable]
    public class Action
    {
        public ActionType type;
        public string text;
        public AudioClip[] audioClips; // Optional sound files
    }

    public List<Action> actions;
    public TMP_Text textBox;
    public AudioLoudnessDetection audioLoudnessDetection;

    public float typingSpeed = 0.05f; // Adjust the typing speed as needed

    private int currentIndex = 0;
    private bool isWaitingPlayerVoice = false;
    private bool canProceed = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        DisplayCurrentAction();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && canProceed)
        {
            NextAction();
        }

        if (isWaitingPlayerVoice)
        {
            float voiceThreshold = PlayerPrefs.GetFloat("VoiceThreshold", 0f);
            float voiceSensitivity = PlayerPrefs.GetFloat("VoiceSensitivity", 50f);
            float loudness = audioLoudnessDetection.GetLoudnessFromMic() * voiceSensitivity;
            Debug.Log(voiceThreshold);    
            if (loudness > voiceThreshold)
            {
                NextAction();
            }
        }
    }

    private void DisplayCurrentAction()
    {
        if (currentIndex < actions.Count)
        {
            Action currentAction = actions[currentIndex];

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            typingCoroutine = StartCoroutine(TypeInText(currentAction.text));

            if (currentAction.type == ActionType.PlayerResponse)
            {
                if (!isWaitingPlayerVoice)
                {
                    isWaitingPlayerVoice = true;
                }
            }
            else
            {
                canProceed = true;
            }

            // Play audio clips if any exist
            PlayAudioClips(currentAction.audioClips);
        }
    }

    private void NextAction()
    {
        canProceed = false;
        currentIndex++;
        DisplayCurrentAction();
    }

    private IEnumerator TypeInText(string textToType)
    {
        textBox.text = "";
        foreach (char c in textToType)
        {
            textBox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void PlayAudioClips(AudioClip[] audioClips)
    {
        if (audioClips != null && audioClips.Length > 0)
        {
            foreach (AudioClip clip in audioClips)
            {
                if (clip != null)
                {
                    AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
                }
            }
        }
    }
}
