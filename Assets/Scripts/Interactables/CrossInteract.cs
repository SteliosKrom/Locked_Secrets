using UnityEngine;

public class CrossInteract : MonoBehaviour, IInteractable
{
    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private BathroomDoorInteract bathroomDoorInteract;
    [SerializeField] private DoorInteract doorInteract;
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject playerCross;
    [SerializeField] private GameObject bathroomCross;
    [SerializeField] private GameObject inventoryCrucifix;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator crucifixAnimator;
    #endregion

    public void Interact()
    {
        GameManager.Instance.CurrentItemState = ItemState.Cross;
        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnCrucifixMenu;

        doorInteract.OtherDoorHandleCollider.enabled = true;

        bathroomCross.SetActive(false);
        playerCross.SetActive(true);
        inventoryCrucifix.SetActive(true);

        mainGameUIManager.Dot.SetActive(false);
        mainGameUIManager.GotCrucifixPanel.SetActive(true);

        crucifixAnimator.SetTrigger("Equip");

        GameManager.Instance.CurrentBathroomDoorState = BathroomDoorState.OpenIdle;

        bathroomDoorInteract.BathroomDoorAnimator.SetTrigger("Open");

        AudioManager.Instance.OpenDoor.source.transform.position = bathroomDoorInteract.BathroomDoorAnimator.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor.source, AudioManager.Instance.OpenDoor.clip);
    }
}
