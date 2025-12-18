using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameUIManager : MonoBehaviour
{
    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject dot;
    #endregion

    #region TEXT
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI[] allButtonTexts;
    #endregion

    public GameObject PauseMenu => pauseMenu;
    public void Resume()
    {
        GameManager.Instance.CurrentGameState = GameState.OnPlaying;
        GameManager.Instance.CurrentMenuState = MenuState.None;

        AudioManager.Instance.UnpauseMainGameMusic();

        pauseMenu.SetActive(false);
        dot.SetActive(true);

        Time.timeScale = 1;

        DisableRedColorOnButtonText();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Settings()
    {
        GameManager.Instance.CurrentMenuState = MenuState.OnGameSettings;
        SettingsUIManager.Instance.GetBackToPauseMenu.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void DisableRedColorOnButtonText()
    {
        foreach (TextMeshProUGUI text in allButtonTexts)
        {
            text.color = Color.white;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }
}
