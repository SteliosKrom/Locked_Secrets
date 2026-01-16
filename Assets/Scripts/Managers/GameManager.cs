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
    OnInventoryMenu,
    OnNoteMenu,
    OnTitleMenu,
    OnMainMenu,
    OnMenuSettings,
    OnGameSettings,
    OnCategorySettings
}

public enum ItemMenuState
{
    None,
    OnRoomKeyMenu,
    OnLanternMenu,
    OnAxeMenu
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
    [SerializeField] private ItemMenuState currentItemMenuState;
    [SerializeField] private PlayerState currentPlayerState;
    [SerializeField] private ItemState currentItemState;
    #endregion

    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public MenuState CurrentMenuState { get => currentMenuState; set => currentMenuState = value; }
    public ItemMenuState CurrentItemMenuState { get => currentItemMenuState; set => currentItemMenuState = value; }
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

    public bool CanMenuInteract()
    {
        return currentMenuState == MenuState.OnInventoryMenu
            || currentMenuState == MenuState.OnNoteMenu;
    }

    public bool CanItemMenuInteract()
    {
        return currentItemMenuState == ItemMenuState.OnRoomKeyMenu
            || currentItemMenuState == ItemMenuState.OnLanternMenu
            || currentItemMenuState == ItemMenuState.OnAxeMenu;
    }
}
