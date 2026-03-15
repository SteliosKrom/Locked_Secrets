using UnityEngine;
using UnityEngine.Rendering;

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
    [SerializeField] private GameObject crucifixInventoryItem;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator crucifixAnimator;
    #endregion

    public void Interact()
    {
        GameManager.Instance.CurrentItemState = ItemState.Cross;
        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnCrucifixMenu;
        InventoryManager.Instance.AddToInventory(crucifixInventoryItem);

        doorInteract.OtherDoorHandleCollider.enabled = true;

        bathroomCross.SetActive(false);
        playerCross.SetActive(true);
        mainGameUIManager.ControlsTutorialPanel.SetActive(false);
        mainGameUIManager.GotCrucifixPanel.SetActive(true);

        crucifixAnimator.SetTrigger("Equip");

        bathroomDoorInteract.CurrentDoorState = DoorState.OpenIdle;
        bathroomDoorInteract.BathroomDoorAnimator.SetTrigger("Open");

        AudioManager.Instance.OpenDoor.source.transform.SetParent(bathroomDoorInteract.gameObject.transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor);
    }
}
