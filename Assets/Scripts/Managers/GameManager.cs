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
    OnRoomKeyMenu,
    OnInventoryMenu,
    OnNoteMenu,
    OnTitleMenu,
    OnMainMenu,
    OnMenuSettings,
    OnGameSettings,
    OnCategorySettings
}

public enum ItemState
{
    None,
    Key,
    Axe
}

public enum DoorState
{
    None,
    Idle,
    Opening,
    Closing,
    Locked,
    Unlocked
}

public enum PlayerState
{
    OnIdle,
    OnWalking,
    OnChopping
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private MenuState currentMenuState;
    [SerializeField] private PlayerState currentPlayerState;
    [SerializeField] private ItemState currentItemState;
    #endregion

    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public MenuState CurrentMenuState { get => currentMenuState; set => currentMenuState = value; }
    public PlayerState CurrentPlayerState { get => currentPlayerState; set => currentPlayerState = value; }
    public ItemState CurrentItemState { get => currentItemState; set => currentItemState = value; }

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
        currentItemState = ItemState.None;
    }

    public bool CanInteract()
    {
        return currentMenuState == MenuState.OnInventoryMenu
            || currentMenuState == MenuState.OnNoteMenu
            || currentMenuState == MenuState.OnRoomKeyMenu;
    }
}
