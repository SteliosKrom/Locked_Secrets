using UnityEngine;

public enum GameState
{
    None,
    OnPlaying,
    OnPaused,
    OnLoading
}

public enum MenuState
{
    None,
    OnPausedMenu,
    OnTitleMenu,
    OnMainMenu,
    OnMenuSettings,
    OnGameSettings,
    OnCategorySettings
}

public enum PlayerState
{
    OnIdle,
    OnWalking,
    OnCrouching
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private MenuState currentMenuState;
    #endregion

    public GameState CurrentGameState { get { return currentGameState; } set { currentGameState = value; } }
    public MenuState CurrentMenuState { get { return currentMenuState; } set { currentMenuState = value; } }
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

    private void Start()
    {
        currentGameState = GameState.None;
        currentMenuState = MenuState.OnTitleMenu;
    }
}
