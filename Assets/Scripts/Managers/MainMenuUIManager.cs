using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    private float loadingDelay = Random.Range(5, 10);
    private float tutorialDelay = 4;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject loadingMenu;

    [SerializeField] private GameObject dot;
    [SerializeField] private GameObject[] tutorialsText;
    [SerializeField] private GameObject pressWASD;
    [SerializeField] private GameObject pressESC;
    [SerializeField] private GameObject pressC;
    [SerializeField] private GameObject pressE;
    [SerializeField] private GameObject pressI;
    #endregion

    #region CAMERAS
    [Header("CAMERAS")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;
    #endregion

    public GameObject MainMenu => mainMenu;

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPaused)
        {
            foreach (GameObject tutorialText in tutorialsText)
            {
                tutorialText.SetActive(false);
            }
        }
    }

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

        yield return new WaitForSeconds(2);
        pressWASD.SetActive(true);
        yield return new WaitForSeconds(tutorialDelay);
        pressESC.SetActive(true);
        yield return new WaitForSeconds(tutorialDelay);
        pressC.SetActive(true);
        yield return new WaitForSeconds(tutorialDelay);
        pressE.SetActive(true);
        yield return new WaitForSeconds(tutorialDelay);
        pressI.SetActive(true);
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
