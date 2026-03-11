using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField] private bool detected = false;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private SmallRoomDoorInteract smallRoomDoorInteract;
    [SerializeField] private MainDoorInteract mainDoorInteract;
    [SerializeField] private BathroomDoorInteract bathroomDoorInteract;
    #endregion

    #region LAYERS
    [Header("LAYERS")]
    [SerializeField] private LayerMask interactable;
    [SerializeField] private LayerMask obstacle;
    #endregion

    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] private Transform interactionSource;
    private float interactionRange = 1.25f;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject interactHUD;
    [SerializeField] private GameObject dot;
    [SerializeField] private GameObject lockedIcon;
    #endregion

    public bool Detected { get => detected; set => detected = value; }
    public GameObject LockedIcon => lockedIcon;

    private void Update()
    {
        Debug.DrawRay(interactionSource.position, interactionSource.forward * interactionRange, Color.red);
        DetectAndInteract();
    }

    public void DetectAndInteract()
    {
        LayerMask combinedMask = interactable | obstacle;

        if (GameManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu) return;

        if (GameManager.Instance.CurrentGameState != GameState.OnPlaying)
        {
            interactHUD.SetActive(false);
            dot.SetActive(false);
            return;
        }

        if (Physics.Raycast(interactionSource.position, interactionSource.forward, out RaycastHit hit, interactionRange, combinedMask))
        {
            int layer = hit.collider.gameObject.layer;
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (layer == LayerMask.NameToLayer("Interactable"))
            {
                bool isLocked = false;

                if (hit.collider.CompareTag("SmallRoomDoor"))
                    isLocked = (smallRoomDoorInteract.CurrentDoorState == DoorState.Locked);
                else if (hit.collider.CompareTag("BathroomDoor"))
                    isLocked = (bathroomDoorInteract.CurrentDoorState == BathroomDoorState.Locked);
                else if (hit.collider.CompareTag("MainDoor"))
                    isLocked = (mainDoorInteract.CurrentDoorState == DoorState.Locked);

                if (isLocked)
                {
                    lockedIcon.SetActive(true);
                    interactHUD.SetActive(false);
                    Debug.Log(hit.collider.tag + " is locked. Display the lock icon!");
                }

                if (hit.collider.CompareTag("OutlineInteractable"))
                {
                    Outline outline = hit.collider.GetComponent<Outline>();
                    outline.enabled = true;
                }

                if (!detected)
                {
                    detected = true;
                    interactHUD.SetActive(true);
                    dot.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.E))
                    interactable.Interact();
            }
            else if (layer == LayerMask.NameToLayer("Obstacle"))
            {
                if (detected)
                {
                    detected = false;
                    interactHUD.SetActive(false);
                    lockedIcon.SetActive(false);

                    if (GameManager.Instance.CurrentItemMenuState != ItemMenuState.None)
                        dot.SetActive(false);
                    else
                        dot.SetActive(true);

                    OutlineEffect.Instance.DisableObjectsOutlineEffect();
                }
            }
        }
        else
        {
            detected = false;
            interactHUD.SetActive(false);
            lockedIcon.SetActive(false);

            if (GameManager.Instance.CurrentItemMenuState != ItemMenuState.None)
                dot.SetActive(false);
            else
                dot.SetActive(true);

            OutlineEffect.Instance.DisableObjectsOutlineEffect();
        }
        return;
    }
}
