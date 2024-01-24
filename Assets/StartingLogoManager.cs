using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingLogoManager : MonoBehaviour
{
    public GameObject content;
    public float fadeInTime = 1.0f;
    public float waitTime = 2.0f;
    public float fadeOutTime = 1.0f;

    void Start()
    {
        StartCoroutine(LogoSequence());
    }

    IEnumerator LogoSequence()
    {
        // Ensure content is active
        content.SetActive(true);

        // Fade in
        yield return FadeCanvasGroup(content.GetComponent<CanvasGroup>(), 0f, 1f, fadeInTime);

        // Wait
        yield return new WaitForSeconds(waitTime);

        // Fade out
        yield return FadeCanvasGroup(content.GetComponent<CanvasGroup>(), 1f, 0f, fadeOutTime);

        // Check high score and load the appropriate scene
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScore > 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("VoiceCalibration");
        }
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float targetAlpha, float duration)
    {
        float currentTime = 0f;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);
            canvasGroup.alpha = alpha;

            currentTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha value is set
        canvasGroup.alpha = targetAlpha;
    }
}
