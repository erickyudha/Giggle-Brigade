using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public TMP_Text okText;
    public AudioLoudnessDetection audioLoudnessDetection;
    public Image micImage;

    public float typingSpeed = 0.05f; // Adjust the typing speed as needed
    public float responseWaitTime = 3f; // Wait time for player response after NPC dialogue
    public float micFadeInDuration = 1f; // Duration to fade in the mic image
    public float okTextFadeInDuration = 1f; // Duration to fade in the OK text
    public float fadeOutDuration = 1f; // Duration to fade out the mic image and OK text

    private int currentIndex = 0;
    private bool isWaitingPlayerVoice = false;
    private bool canProceed = false;
    private Coroutine typingCoroutine;
    public GameObject content;

    private void Start()
    {
        DisplayCurrentAction();
        okText.gameObject.SetActive(false);
        micImage.gameObject.SetActive(false);
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

            if (loudness > voiceThreshold)
            {
                isWaitingPlayerVoice = false;
                StartCoroutine(FadeInOkText());
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
                    StartCoroutine(WaitForPlayerResponse());
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

    private IEnumerator WaitForPlayerResponse()
    {
        // Fade in mic image while waiting
        yield return StartCoroutine(FadeInMicImage());
        isWaitingPlayerVoice = true;
    }

    private IEnumerator FadeInMicImage()
    {
        micImage.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime < micFadeInDuration)
        {
            float alpha = (Time.time - startTime) / micFadeInDuration;
            micImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeInOkText()
    {
        okText.gameObject.SetActive(true);
        yield return new WaitForSeconds(responseWaitTime);
        StartCoroutine(FadeOutMicImageAndOkText());
    }

    private IEnumerator FadeOutMicImageAndOkText()
    {
        float startTime = Time.time;
        while (Time.time - startTime < fadeOutDuration)
        {
            float alpha = 1 - (Time.time - startTime) / fadeOutDuration;
            micImage.color = new Color(1f, 1f, 1f, alpha);
            okText.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        micImage.gameObject.SetActive(false);
        okText.gameObject.SetActive(false);
        NextAction();
    }

    private void NextAction()
    {
        canProceed = false;
        currentIndex++;

        if (currentIndex < actions.Count)
        {
            DisplayCurrentAction();
        }
        else
        {
            StartCoroutine(FadeOutContentAndLoadScene());
        }
    }

    private IEnumerator FadeOutContentAndLoadScene()
    {
        // Fade out content
        float fadeOutDuration = 2f;
        float startTime = Time.time;
        while (Time.time - startTime < fadeOutDuration)
        {
            float alpha = 1 - (Time.time - startTime) / fadeOutDuration;
            content.GetComponent<CanvasGroup>().alpha = alpha;
            yield return null;
        }

        // Load the next scene
        GameManager.LoadGameSceneAsync();
    }
}
