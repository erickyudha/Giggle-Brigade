using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    // Reference to the Text component attached to the GameObject
    private Text highScoreText;

    void Start()
    {
        // Get the Text component attached to the GameObject
        highScoreText = GetComponent<Text>();

        // Display the initial high score
        UpdateHighScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        // You can add additional update logic here if needed
    }

    // Call this method to set and display a new high score
    public bool SetNewHighScore(int currentScore)
    {
        // You can set a new high score logic here if needed
        // For example, compare the current score with a new score and update if necessary
        // Save the new score to PlayerPrefs
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        bool newRecord = currentScore > highScore;
        if (newRecord)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }

        // Display the updated high score
        UpdateHighScoreText();
        return newRecord;
    }

    // Helper method to update the displayed high score text
    private void UpdateHighScoreText()
    {
        // Retrieve the high score from PlayerPrefs
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Update the text component with the high score
        highScoreText.text = "High Score: " + highScore.ToString() + "m";
    }
}
