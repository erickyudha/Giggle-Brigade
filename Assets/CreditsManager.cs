using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public Image creditsImage;
    public float scrollSpeed = 50f;
    public float fastForwardSpeed = 150f;

    void Update()
    {
        // Scroll the credits image
        float currentSpeed = Input.GetKey(KeyCode.Space) ? fastForwardSpeed : scrollSpeed;
        creditsImage.rectTransform.anchoredPosition += Vector2.up * currentSpeed * Time.deltaTime;

        // Check for fast-forward input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            // Fast forward to the end
            FastForwardToEnd();
        }

        // Check if the credits have reached the end
        if (creditsImage.rectTransform.anchoredPosition.y > creditsImage.rectTransform.rect.height)
        {
            // Load the MainMenu scene
            SceneManager.LoadScene("MainMenu");
        }
    }

    void FastForwardToEnd()
    {
        float remainingDistance = creditsImage.rectTransform.rect.height - creditsImage.rectTransform.anchoredPosition.y;
        float fastForwardTime = remainingDistance / fastForwardSpeed;
        creditsImage.rectTransform.anchoredPosition += Vector2.up * remainingDistance;

        // Wait for the remaining fast forward time
        Invoke("LoadMainMenuScene", fastForwardTime);
    }

    void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
