using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    public static SettingsUIManager Instance;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainMenuUIManager mainMenuUIManager;
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region UI
    [Header("MENUS")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject controlsMenu;

    [Header("BACK BUTTONS")]
    [SerializeField] private GameObject backToMenu;
    [SerializeField] private GameObject backToSettings;
    [SerializeField] private GameObject backToSettingsFromGame;
    [SerializeField] private GameObject backToPauseMenu;
    #endregion

    public GameObject SettingsMenu => settingsMenu;
    public GameObject GetBackToSettings => backToSettings;
    public GameObject GetBackToMenu => backToMenu;
    public GameObject GetBackToSettingsFromGame => backToSettingsFromGame;
    public GameObject GetBackToPauseMenu => backToPauseMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("SettingsUIManager instance already exists, destroying duplicate!");
            Destroy(gameObject);
        }
    }

    public void OpenAudio()
    {
        OpenCategory(audioMenu);
        GameManager.Instance.CurrentMenuState = MenuState.OnCategorySettings;
    }

    public void OpenDisplay()
    {
        OpenCategory(displayMenu);
        GameManager.Instance.CurrentMenuState = MenuState.OnCategorySettings;
    }

    public void OpenGraphics()
    {
        OpenCategory(graphicsMenu);
        GameManager.Instance.CurrentMenuState = MenuState.OnCategorySettings;
    }

    public void OpenControls()
    {
        OpenCategory(controlsMenu);
        GameManager.Instance.CurrentMenuState = MenuState.OnCategorySettings;
    }

    public void OpenCategory(GameObject category)
    {
        settingsMenu.SetActive(false);
        category.SetActive(true);

        switch(GameManager.Instance.CurrentMenuState)
        {
            case MenuState.OnMenuSettings:
                backToMenu.SetActive(false);
                backToSettings.SetActive(true);
                break;
            case MenuState.OnGameSettings:
                backToSettingsFromGame.SetActive(true);
                backToPauseMenu.SetActive(false);
                break;
        }
    }

    public void BackToMenu()
    {
        GameManager.Instance.CurrentMenuState = MenuState.OnMainMenu;
        CloseAll();
        mainMenuUIManager.MainMenu.SetActive(true);
        backToMenu.SetActive(false);
    }

    public void BackToSettings()
    {
        GameManager.Instance.CurrentMenuState = MenuState.OnMenuSettings;
        HideAllCatagories();
        settingsMenu.SetActive(true);
        backToSettings.SetActive(false);
        backToMenu.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        GameManager.Instance.CurrentMenuState = MenuState.OnPausedMenu;
        CloseAll();
        mainGameUIManager.PauseMenu.SetActive(true);
        backToPauseMenu.SetActive(false);
    }

    public void BackToSettingsFromGame()
    {
        GameManager.Instance.CurrentMenuState = MenuState.OnGameSettings;
        CloseAll();
        settingsMenu.SetActive(true);
        backToPauseMenu.SetActive(true);
        backToSettingsFromGame.SetActive(false);
    }

    public void HideAllCatagories()
    {
        audioMenu.SetActive(false);
        displayMenu.SetActive(false);
        graphicsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void CloseAll()
    {
        settingsMenu.SetActive(false);
        HideAllCatagories();
    }
}
