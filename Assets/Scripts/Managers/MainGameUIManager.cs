using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameUIManager : MonoBehaviour
{
    public static MainGameUIManager Instance;

    private float closeNoteMenuDelay = 0.01f;
    private float noteInteractDelay = 1f;

    private bool canInteractWithNote = true;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    private NoteInteract currentNote;
    [SerializeField] private SmallRoomDoorInteract smallRoomDoorInteract;
    [SerializeField] private DoorInteract doorInteract;
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        InputForNoteMenu();
        InputToCloseRoomKeyPanel();
    }

    public void SetCurrentNote(NoteInteract note)
    {
        currentNote = note;
    }

    public void InputForNoteMenu()
    {
        if (GameManager.Instance.CurrentMenuState == MenuState.OnNoteMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && canInteractWithNote)
            {
                StartCoroutine(CloseNoteMenuDelay());
            }
            StartCoroutine(NoteInteractDelay());
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
        AudioManager.Instance.UnpauseSFX(smallRoomDoorInteract.UnlockedAudioSource);

        UnPauseAllSFX();

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

    public void UnPauseAllSFX()
    {
        foreach (AudioManager.AudioItem audioItem in AudioManager.Instance.AllSFX)
        {
            AudioManager.Instance.UnpauseSFX(audioItem.source);
        }
    }

    public IEnumerator NoteInteractDelay()
    {
        canInteractWithNote = false;
        yield return new WaitForSecondsRealtime(noteInteractDelay);
        canInteractWithNote = true;
    }

    public IEnumerator CloseNoteMenuDelay()
    {
        yield return new WaitForSecondsRealtime(closeNoteMenuDelay);
        currentNote.NoteModel.SetActive(true);
        currentNote.NoteCanvas.SetActive(false);
        GameManager.Instance.CurrentMenuState = MenuState.None;
    }
}
