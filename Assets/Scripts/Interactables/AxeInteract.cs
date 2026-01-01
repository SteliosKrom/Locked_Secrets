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
    [SerializeField] private Animator baseEquipItemAnimator;
    #endregion

    public void Interact()
    {
        worldAxe.SetActive(false);
        playerAxe.SetActive(true);
        axeIcon.SetActive(true);
        baseEquipItemAnimator.SetTrigger("Equip");
        GameManager.Instance.CurrentItemState = ItemState.Axe;
    }
}
