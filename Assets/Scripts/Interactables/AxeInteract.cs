using UnityEngine;

public class AxeInteract : MonoBehaviour, IInteractable
{
    private bool isCoroutineRunning = false;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject worldAxe;
    [SerializeField] private GameObject playerAxe;
    [SerializeField] private GameObject axeIcon;
    #endregion

    #region ANIMATORS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator baseAxeAnimator;
    #endregion

    public bool IsCoroutineRunning { get => isCoroutineRunning; set => isCoroutineRunning = value; }
    public GameObject PlayerAxe => playerAxe;
    public Animator BaseAxeAnimator => baseAxeAnimator;

    public void Interact()
    {
        worldAxe.SetActive(false);
        playerAxe.SetActive(true);
        axeIcon.SetActive(true);
        baseAxeAnimator.SetTrigger("Equip");
        GameManager.Instance.CurrentItemState = ItemState.Axe;
    }
}
