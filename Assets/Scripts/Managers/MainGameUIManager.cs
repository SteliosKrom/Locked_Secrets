using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameUIManager : MonoBehaviour
{
    public static MainGameUIManager Instance;

    private float closeNoteMenuDelay = 0.01f;
    private float noteInteractDelay = 1f;
    private float canPressTabDelay = 0.5f;

    private bool canInteractWithNote = true;
    [SerializeField] private bool canPressTab = true;
    [SerializeField] private bool isControlsTutorialPanelOpen = false;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    private NoteInteract currentNote;
    [SerializeField] private SmallRoomDoorInteract smallRoomDoorInteract;
    [SerializeField] private DoorInteract doorInteract;
    [SerializeField] private NoteInteract noteInteract;
    [SerializeField] private Interactor interactor;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject controlsTutorialPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject dot;
    [SerializeField] private GameObject gotRoomKeyPanel;
    [SerializeField] private GameObject gotLanternPanel;
    [SerializeField] private GameObject gotAxePanel;
    [SerializeField] private GameObject gotCrucifixPanel;
    #endregion

    #region TEXT
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI[] allButtonTexts;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator controlsPanelAnimator;
    #endregion

    public GameObject Dot => dot;
    public GameObject ControlsTutorialPanel { get => controlsTutorialPanel; set => controlsTutorialPanel = value; }
    public GameObject PauseMenu => pauseMenu; public GameObject GotRoomKeyPanel => gotRoomKeyPanel;
    public GameObject GotLanternPanel => gotLanternPanel; public GameObject GotAxePanel => gotAxePanel;
    public GameObject GotCrucifixPanel => gotCrucifixPanel;
    public Animator ControlsPanelAnimator => controlsPanelAnimator;
    public bool IsControlsTutorialPanelOpen { get => isControlsTutorialPanelOpen; set => isControlsTutorialPanelOpen = value; }
    public bool CanPressTab { get => canPressTab; set => canPressTab = value; }

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
        InputForControlsTutorialPanel();
    }

    public void SetCurrentNote(NoteInteract note)
    {
        currentNote = note;
    }

    public void InputForControlsTutorialPanel()
    {
        if (GameManager.Instance.CurrentGameState != GameState.OnPlaying) return;
        if (GameManager.Instance.CurrentMenuState == MenuState.OnNoteMenu) return;


        if (Input.GetKeyDown(KeyCode.Tab) && canPressTab)
        {
            isControlsTutorialPanelOpen = !isControlsTutorialPanelOpen;

            if (isControlsTutorialPanelOpen)
                controlsPanelAnimator.SetTrigger("Open");
            else
                controlsPanelAnimator.SetTrigger("Close");

            StartCoroutine(PreventSpamControlsTutorialPanelDelay());
        }
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
                case ItemMenuState.OnCrucifixMenu:
                    gotCrucifixPanel.SetActive(false);
                    break;
            }
            dot.SetActive(true);
            GameManager.Instance.CurrentItemMenuState = ItemMenuState.None;
        }
    }

    public void Resume()
    {
        GameManager.Instance.CurrentGameState = GameState.OnPlaying;
        GameManager.Instance.CurrentMenuState = MenuState.None;

        interactor.Detected = false;

        AudioManager.Instance.UnpauseRainAudioSound();
        AudioManager.Instance.UnpauseMainGameMusic();
        AudioManager.Instance.UnpauseSFX(AudioManager.Instance.UnlockedDoor.source);

        UnPauseAllSFX();

        pauseMenu.SetActive(false);
        dot.SetActive(true);
        controlsTutorialPanel.SetActive(true);
        isControlsTutorialPanelOpen = false;

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
        noteInteract.IsInteracted = false;
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

        controlsTutorialPanel.SetActive(true);
        isControlsTutorialPanelOpen = false;

        if (!noteInteract.IsInteracted)
        {
            PuzzleManager.Instance.EnableFirstPuzzleObjectColliders();
            noteInteract.IsInteracted = true;
        }

        GameManager.Instance.CurrentMenuState = MenuState.None;
    }

    public IEnumerator PreventSpamControlsTutorialPanelDelay()
    {
        canPressTab = false;
        yield return new WaitForSecondsRealtime(canPressTabDelay);
        canPressTab = true;
    }
}
