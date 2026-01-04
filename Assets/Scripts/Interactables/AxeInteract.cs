using UnityEngine;

public class AxeInteract : MonoBehaviour, IInteractable
{
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
