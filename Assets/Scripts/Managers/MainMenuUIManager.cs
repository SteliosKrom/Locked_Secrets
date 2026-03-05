using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    private float loadingDelay;
    private float controlsTutorialPanelDelay = 1f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject loadingMenu;

    [SerializeField] private GameObject dot;
    #endregion

    #region CAMERAS
    [Header("CAMERAS")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;
    #endregion

    public GameObject MainMenu => mainMenu;

    private void Start()
    {
        mainGameUIManager.ControlsTutorialPanel.SetActive(false);
        loadingDelay = Random.Range(1, 2); // 5, 10
    }

    public void Play()
    {
        StartCoroutine(LoadingDelay());
    }

    public IEnumerator LoadingDelay()
    {
        GameManager.Instance.CurrentGameState = GameState.OnLoading;

        AudioManager.Instance.StopSFX(AudioManager.Instance.PressAnyKeyAudioSource.source);
        AudioManager.Instance.StopMenuMusic();
        AudioManager.Instance.StopRainAudio();

        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);

        yield return new WaitForSeconds(loadingDelay);

        GameManager.Instance.CurrentGameState = GameState.OnPlaying;
        GameManager.Instance.CurrentMenuState = MenuState.None;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.TableLanternFlicker.source, AudioManager.Instance.TableLanternFlicker.clip);
        AudioManager.Instance.PlayMainGameMusic();
        AudioManager.Instance.PlayRainAudio();

        AudioManager.Instance.RainAudioLowPassFilter.cutoffFrequency = 1000f;

        loadingMenu.SetActive(false);
        dot.SetActive(true);
        secondaryCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        mainGameUIManager.IsControlsTutorialPanelOpen = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(controlsTutorialPanelDelay);

        mainGameUIManager.ControlsTutorialPanel.SetActive(true);
        mainGameUIManager.ControlsPanelAnimator.SetTrigger("Open");
        mainGameUIManager.IsControlsTutorialPanelOpen = true;
    }

    public void Settings()
    {
        GameManager.Instance.CurrentMenuState = MenuState.OnMenuSettings;
        SettingsUIManager.Instance.GetBackToMenu.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackToMenuFromCredits()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
