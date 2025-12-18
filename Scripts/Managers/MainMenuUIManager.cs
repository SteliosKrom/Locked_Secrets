using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    private float loadingDelay = 1; // Add random values later

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
    public void Play()
    {
        StartCoroutine(LoadingDelay());       
    }

    public IEnumerator LoadingDelay()
    {
        GameManager.Instance.CurrentGameState = GameState.OnLoading;
        AudioManager.Instance.StopMenuMusic();
        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);

        yield return new WaitForSeconds(loadingDelay);

        GameManager.Instance.CurrentGameState = GameState.OnPlaying;
        GameManager.Instance.CurrentMenuState = MenuState.None;
        AudioManager.Instance.PlayMainGameMusic();

        loadingMenu.SetActive(false);
        dot.SetActive(true);
        secondaryCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
