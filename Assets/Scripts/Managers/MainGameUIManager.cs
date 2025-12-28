using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameUIManager : MonoBehaviour
{
    private float closeNoteMenuDelay = 0.01f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private NoteInteract noteInteract;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject dot;
    [SerializeField] private GameObject gotRoomKeyPanel;
    #endregion

    #region TEXT
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI[] allButtonTexts;
    #endregion

    public GameObject PauseMenu => pauseMenu;
    public GameObject GotRoomKeyPanel => gotRoomKeyPanel;

    private void Update()
    {
        InputForNoteMenu();
        InputToCloseRoomKeyPanel();
    }

    public void InputForNoteMenu()
    {
        if (GameManager.Instance.CurrentMenuState == MenuState.OnNoteMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(CloseNoteMenuDelay());
            }
        }
    }

    public void InputToCloseRoomKeyPanel()
    {
        if (GameManager.Instance.CurrentMenuState == MenuState.OnRoomKeyMenu)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.CurrentMenuState = MenuState.None;
                gotRoomKeyPanel.SetActive(false);
            }
        }
    }

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

    public IEnumerator CloseNoteMenuDelay()
    {
        yield return new WaitForSecondsRealtime(closeNoteMenuDelay);
        noteInteract.Note.SetActive(true);
        noteInteract.NoteCanvas.SetActive(false);
        GameManager.Instance.CurrentMenuState = MenuState.None;
    }
}
