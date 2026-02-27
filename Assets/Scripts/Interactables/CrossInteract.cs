using UnityEngine;

public class CrossInteract : MonoBehaviour, IInteractable
{
    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private BathroomDoorInteract bathroomDoorInteract;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject playerCross;
    [SerializeField] private GameObject bathroomCross;
    [SerializeField] private GameObject crucifixPanel;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator crucifixAnimator;
    #endregion

    public void Interact()
    {
        GameManager.Instance.CurrentItemState = ItemState.Cross;
        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnCrucifixMenu;

        bathroomCross.SetActive(false);
        playerCross.SetActive(true);
        crucifixPanel.SetActive(true);
        crucifixAnimator.SetTrigger("Equip");

        GameManager.Instance.CurrentBathroomDoorState = BathroomDoorState.Unlocked;
        bathroomDoorInteract.BathroomDoorAnimator.SetTrigger("Open");

        AudioManager.Instance.OpenDoor.source.transform.position = bathroomDoorInteract.BathroomDoorAnimator.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor.source, AudioManager.Instance.OpenDoor.clip);
    }
}
