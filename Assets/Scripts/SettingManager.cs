using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider bgMusicSlider;
    public Slider sfxSlider;
    public TMP_Dropdown microphoneDropdown;
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        // Load settings when the game starts
        LoadSettings();

        // Populate resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
        }

        // Set current resolution in the dropdown
        int currentResolutionIndex = FindCurrentResolutionIndex();
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Populate microphone dropdown
        PopulateMicrophoneDropdown();
    }

    private void PopulateMicrophoneDropdown()
    {
        microphoneDropdown.ClearOptions();
        string[] microphoneDevices = Microphone.devices;

        if (microphoneDevices.Length > 0)
        {
            foreach (string device in microphoneDevices)
            {
                microphoneDropdown.options.Add(new TMP_Dropdown.OptionData(device));
            }
        }
        else
        {
            // If no microphones found, provide a default option
            microphoneDropdown.options.Add(new TMP_Dropdown.OptionData("No Microphones"));
        }
    }

    public void SetBGMusicVolume(float volume)
    {
        audioMixer.SetFloat("BG_Music_Volume", volume);
        SaveSettings();
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX_Volume", volume);
        SaveSettings();
    }

    public void SetMicrophone(int index)
    {
        string[] microphoneDevices = Microphone.devices;

        // Check if the selected index is valid
        if (index >= 0 && index < microphoneDevices.Length)
        {
            // Add logic to handle the selected microphone
            // For example, you can store the microphone device name in PlayerPrefs
            PlayerPrefs.SetString("SelectedMicrophone", microphoneDevices[index]);
            SaveSettings();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveSettings();
    }

    public void SetResolution(int index)
    {
        if (index >= 0 && index < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[index];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            SaveSettings();
        }
    }

    private int FindCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0; // Default to the first resolution if not found
    }

    private void SaveSettings()
    {
        // Save settings using PlayerPrefs
        PlayerPrefs.SetFloat("BG_Music_Volume", bgMusicSlider.value);
        PlayerPrefs.SetFloat("SFX_Volume", sfxSlider.value);
        PlayerPrefs.SetInt("Microphone_Index", microphoneDropdown.value);
        PlayerPrefs.SetString("SelectedMicrophone", Microphone.devices[microphoneDropdown.value]);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        // Load settings using PlayerPrefs
        bgMusicSlider.value = PlayerPrefs.GetFloat("BG_Music_Volume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX_Volume", 0.5f);

        // Check if the Microphone_Index key exists before trying to load it
        if (PlayerPrefs.HasKey("Microphone_Index"))
        {
            int microphoneIndex = PlayerPrefs.GetInt("Microphone_Index", 0);
            microphoneDropdown.value = microphoneIndex;
            SetMicrophone(microphoneIndex); // Set the microphone based on the loaded index
        }
        else
        {
            // Default value if the key is not found
            microphoneDropdown.value = 0;
        }

        // Check if fullscreenToggle is assigned
        if (fullscreenToggle != null)
        {
            // Check if the Fullscreen key exists before trying to load it
            if (PlayerPrefs.HasKey("Fullscreen"))
            {
                fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
            }
            else
            {
                // Default value if the key is not found
                fullscreenToggle.isOn = true;
            }
        }
        else
        {
            Debug.LogError("FullscreenToggle is not assigned in the Unity Editor!");
        }

        // Apply loaded settings
        SetBGMusicVolume(bgMusicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetFullscreen(fullscreenToggle.isOn);
    }
}
