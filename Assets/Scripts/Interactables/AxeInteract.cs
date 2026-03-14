using UnityEngine;

public class AxeInteract : MonoBehaviour, IInteractable
{
    private bool isCoroutineRunning = false;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject worldAxe;
    [SerializeField] private GameObject playerAxe;
    [SerializeField] private GameObject planksInformText;
    [SerializeField] private GameObject axeInventoryItem;
    #endregion

    #region ANIMATORS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator baseAxeAnimator;
    #endregion

    public bool IsCoroutineRunning { get => isCoroutineRunning; set => isCoroutineRunning = value; }
    public GameObject PlayerAxe => playerAxe;
    public GameObject PlanksInformText => planksInformText;
    public Animator BaseAxeAnimator => baseAxeAnimator;

    public void Interact()
    {
        InventoryManager.Instance.AddToInventory(axeInventoryItem);

        worldAxe.SetActive(false);
        playerAxe.SetActive(true);

        mainGameUIManager.GotAxePanel.SetActive(true);
        baseAxeAnimator.SetTrigger("Equip");

        GameManager.Instance.CurrentItemState = ItemState.Axe;
        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnAxeMenu;
    }
}
