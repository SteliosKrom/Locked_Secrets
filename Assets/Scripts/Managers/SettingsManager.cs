using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public const string masterVol = "MasterVol";
    public const string menuVol = "MenuVol";
    public const string gameVol = "GameVol";
    public const string sfxVol = "SFXVol";

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERNCES")]
    [SerializeField] private FirstPersonCamera firstPersonCamera;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioMixer myAudioMixer;
    #endregion

    #region UI
    [Header("SLIDERS")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider menuVolumeSlider;
    [SerializeField] private Slider gameVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI masterVolumeSliderValue;
    [SerializeField] private TextMeshProUGUI menuVolumeSliderValue;
    [SerializeField] private TextMeshProUGUI gameVolumeSliderValue;
    [SerializeField] private TextMeshProUGUI sfxVolumeSliderValue;

    [Header("TOGGLE")]
    [SerializeField] private Toggle displayModeToggle;

    [Header("DROPDOWN")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    #endregion

    private void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;

        // Audio saved values
        float savedSliderMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float savedActualMasterVolume = PlayerPrefs.GetFloat(masterVol, 5f);

        float savedSliderMenuVolume = PlayerPrefs.GetFloat("MenuVolume", 0.50f);
        float savedActualMenuVolume = PlayerPrefs.GetFloat(menuVol, -4f);

        float savedSliderGameVolume = PlayerPrefs.GetFloat("GameVolume", 0.50f);
        float savedActualGameVolume = PlayerPrefs.GetFloat(gameVol, -4f);

        float savedSliderSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        float savedActualSFXVolume = PlayerPrefs.GetFloat(sfxVol, 1);

        // Display saved values
        int savedResolutionWidth = PlayerPrefs.GetInt("ScreenWidth", currentWidth);
        int savedResolutionHeight = PlayerPrefs.GetInt("ScreenHeight", currentHeight);

        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);

        bool savedDisplayMode = PlayerPrefs.GetInt("DisplayMode", 1) != 0;

        //Graphics saved values
        int savedQualityLevel = PlayerPrefs.GetInt("QualityLevel", 1);
        int savedQualityDropdown = PlayerPrefs.GetInt("QualityDropdown", 1);

        // Audio
        masterVolumeSlider.value = savedSliderMasterVolume;
        menuVolumeSlider.value = savedSliderMenuVolume;
        gameVolumeSlider.value = savedSliderGameVolume;
        sfxVolumeSlider.value = savedSliderSFXVolume;

        myAudioMixer.SetFloat(masterVol, savedActualMasterVolume);
        myAudioMixer.SetFloat(menuVol, savedActualMenuVolume);
        myAudioMixer.SetFloat(gameVol, savedActualGameVolume);
        myAudioMixer.SetFloat(sfxVol, savedActualSFXVolume);

        // Display
        Screen.SetResolution(savedResolutionWidth, savedResolutionHeight, savedDisplayMode);
        displayModeToggle.isOn = savedDisplayMode;
        firstPersonCamera.SensitivitySlider.value = savedSensitivity;

        // Graphics
        QualitySettings.SetQualityLevel(savedQualityLevel);
        qualityDropdown.value = savedQualityDropdown;
    }

    public void MasterVolumeSlider()
    {
        float masterVolume = masterVolumeSlider.value;
        float dB;
        masterVolumeSliderValue.text = masterVolume.ToString("0%");

        if (masterVolume <= 0.0001f)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(masterVolume) * 20f;
            float boostAmount = 5f;
            dB += boostAmount * masterVolume;
        }

        myAudioMixer.SetFloat(masterVol, dB);
        PlayerPrefs.SetFloat(masterVol, dB);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
    }

    public void MenuVolumeSlider()
    {
        float menuVolume = menuVolumeSlider.value;
        float dB;
        menuVolumeSliderValue.text = menuVolume.ToString("0%");

        if (menuVolume <= 0.0001f)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(menuVolume) * 20f;
            float boostAmount = 5f;
            dB += boostAmount * menuVolume;
        }

        myAudioMixer.SetFloat(menuVol, dB);
        PlayerPrefs.SetFloat(menuVol, dB);
        PlayerPrefs.SetFloat("MenuVolume", menuVolume);
    }

    public void GameVolumeSlider()
    {
        float gameVolume = gameVolumeSlider.value;
        float dB;
        gameVolumeSliderValue.text = gameVolume.ToString("0%");

        if (gameVolume <= 0.0001f)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(gameVolume) * 20f;
            float boostAmount = 5f;
            dB += boostAmount * gameVolume;
        }

        myAudioMixer.SetFloat(gameVol, dB);
        PlayerPrefs.SetFloat(gameVol, dB);
        PlayerPrefs.SetFloat("GameVolume", gameVolume);
    }

    public void SFXVolumeSlider()
    {
        float sfxVolume = sfxVolumeSlider.value;
        float dB;
        sfxVolumeSliderValue.text = sfxVolume.ToString("0%");

        if (sfxVolume <= 0.0001f)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(sfxVolume) * 20f;
            float boostAmount = 5f;
            dB += boostAmount * sfxVolume;
        }

        myAudioMixer.SetFloat(sfxVol, dB);
        PlayerPrefs.SetFloat(sfxVol, dB);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void SetQualityLevels()
    {
        switch (qualityDropdown.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
        }
        PlayerPrefs.SetInt("QualityLevel", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetInt("QualityDropdown", qualityDropdown.value);
    }

    public void SetDisplayMode()
    {
        if (displayModeToggle.isOn)
        {
            SetFullscreenMode();
        }
        else
        {
            SetWindowedMode();
        }
        PlayerPrefs.Save();
    }

    public void SetFullscreenMode()
    {
        FullScreenMode fullscreenMode = FullScreenMode.FullScreenWindow;

        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;

        Screen.SetResolution(currentWidth, currentHeight, fullscreenMode);

        PlayerPrefs.SetInt("ScreenWidth", currentWidth);
        PlayerPrefs.SetInt("ScreenHeight", currentHeight);
        PlayerPrefs.SetInt("DisplayMode", 1);
    }

    public void SetWindowedMode()
    {
        FullScreenMode windowedMode = FullScreenMode.Windowed;

        int currentWidth = 1260;
        int currentHeight = 720;

        Screen.SetResolution(currentWidth, currentHeight, windowedMode);

        PlayerPrefs.SetInt("ScreenWidth", currentWidth);
        PlayerPrefs.SetInt("ScreenHeight", currentHeight);
        PlayerPrefs.SetInt("DisplayMode", 0);
    }
}
