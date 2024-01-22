using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class CalibrationManager : MonoBehaviour
{
    public Slider voiceSensitivitySlider;
    public Slider voiceThresholdSlider;
    public Slider voiceVisualizer;
    public Slider visualizerRef;
    public GameObject jumpIndicator;
    public AudioLoudnessDetection detector;
    private Image indicatorImage;
    private TMP_Text indicatorText;

    void Start()
    {
        // Load calibration settings when the game starts
        LoadCalibrationSettings();

        voiceVisualizer.interactable = false;
        visualizerRef.interactable = false;

        indicatorImage = jumpIndicator.GetComponent<Image>();
        indicatorText = jumpIndicator.GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        float loudness = detector.GetLoudnessFromMic() * voiceSensitivitySlider.value;
        voiceVisualizer.value = loudness;

        if (loudness >= voiceThresholdSlider.value)
        {
            Color colorGreen = HexToColor("#44EA55");
            indicatorImage.color = colorGreen;
            indicatorText.text = "JUMPING";
        }
        else
        {
            Color colorRed = HexToColor("#FF9292");
            indicatorImage.color = colorRed;
            indicatorText.text = "NOT JUMPING";
        }
    }

    Color HexToColor(string hex)
    {
        Color color = Color.white;

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogError("Invalid hex color: " + hex);
            return Color.white;
        }
    }

    public void SetVoiceSensitivity(float sensitivity)
    {
        // Handle voice sensitivity setting
        // You can implement your logic here
        SaveCalibrationSettings();
    }

    public void SetVoiceThreshold(float threshold)
    {
        // Handle voice threshold setting
        // You can implement your logic here
        visualizerRef.value = threshold;

        SaveCalibrationSettings();
    }

    private void SaveCalibrationSettings()
    {
        // Save calibration settings using PlayerPrefs or other persistent storage
        PlayerPrefs.SetFloat("VoiceSensitivity", voiceSensitivitySlider.value);
        PlayerPrefs.SetFloat("VoiceThreshold", voiceThresholdSlider.value);
        PlayerPrefs.Save();
    }

    private void LoadCalibrationSettings()
    {
        float voiceThreshold = PlayerPrefs.GetFloat("VoiceThreshold", 0f);
        float voiceSensitivity = PlayerPrefs.GetFloat("VoiceSensitivity", 50f);

        voiceThresholdSlider.value = voiceThreshold;
        voiceSensitivitySlider.value = voiceSensitivity;

        SetVoiceThreshold(voiceThresholdSlider.value);
        SetVoiceSensitivity(voiceSensitivitySlider.value);
    }


    public void HandleContinue()
    {
        SaveCalibrationSettings();
        SceneManager.LoadScene("MainMenu");
    }
}
