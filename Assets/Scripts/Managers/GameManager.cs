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

public enum DoorState
{
    Idle,
    Opened,
    Closed,
    Locked,
    Unlocked
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
    [SerializeField] private PlayerState currentPlayerState;
    [SerializeField] private DoorState currentDoorState;
    #endregion

    public GameState CurrentGameState { get { return currentGameState; } set { currentGameState = value; } }
    public MenuState CurrentMenuState { get { return currentMenuState; } set { currentMenuState = value; } }
    public PlayerState CurrentPlayerState { get { return currentPlayerState; } set { currentPlayerState = value; } }
    public DoorState CurrentDoorState { get { return currentDoorState; } set { currentDoorState = value; } }

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
        currentPlayerState = PlayerState.OnIdle;
        currentDoorState = DoorState.Idle;
    }
}
