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
    [SerializeField] private NoteInteract noteInteract;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject dot;
    [SerializeField] private GameObject gotRoomKeyPanel;
    [SerializeField] private GameObject gotLanternPanel;
    [SerializeField] private GameObject gotAxePanel;
    #endregion

    #region TEXT
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI[] allButtonTexts;
    #endregion

    public GameObject PauseMenu => pauseMenu;
    public GameObject GotRoomKeyPanel => gotRoomKeyPanel;
    public GameObject GotLanternPanel => gotLanternPanel;
    public GameObject GotAxePanel => gotAxePanel;

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
        InputToCloseItemPanel();
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

    public void InputToCloseItemPanel()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (GameManager.Instance.CurrentItemMenuState)
            {
                case ItemMenuState.OnRoomKeyMenu:
                    gotRoomKeyPanel.SetActive(false);
                    break;
                case ItemMenuState.OnLanternMenu:
                    gotLanternPanel.SetActive(false);
                    break;
                case ItemMenuState.OnAxeMenu:
                    gotAxePanel.SetActive(false);
                    break;
            }
            GameManager.Instance.CurrentItemMenuState = ItemMenuState.None;
        }
    }

    public void Resume()
    {
        GameManager.Instance.CurrentGameState = GameState.OnPlaying;
        GameManager.Instance.CurrentMenuState = MenuState.None;

        AudioManager.Instance.UnpauseMainGameMusic();
        AudioManager.Instance.UnpauseSFX(AudioManager.Instance.UnlockedDoor.source);

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

        if (!noteInteract.IsInteracted)
        {
            PuzzleManager.Instance.EnableFirstPuzzleItemColliders();
            OutlineEffect.Instance.EnableFirstPuzzleItemsOutlineEffect();
            noteInteract.IsInteracted = true;
        }

        GameManager.Instance.CurrentMenuState = MenuState.None;
    }
}
