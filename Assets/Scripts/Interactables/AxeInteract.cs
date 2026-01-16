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
    [SerializeField] private GameObject axeIcon;
    [SerializeField] private GameObject planksInformText;
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
        worldAxe.SetActive(false);
        playerAxe.SetActive(true);
        axeIcon.SetActive(true);
        baseAxeAnimator.SetTrigger("Equip");
        mainGameUIManager.GotAxePanel.SetActive(true);
        GameManager.Instance.CurrentItemState = ItemState.Axe;
        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnAxeMenu;
    }
}
