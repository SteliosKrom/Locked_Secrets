using System.Collections;
using TMPro;
using UnityEngine;

public class SmallRoomDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorState currentDoorState = DoorState.Locked;

    private float doorAnimationDelay = 1f;
    private float itsLockedTextDelay = 2f;

    [SerializeField] private bool canInteract = true;

    #region ANIMATIONS
    [Header("ANIMATOR")]
    [SerializeField] private Animator baseDoorAnimator;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider[] doorColliders;
    #endregion

    #region UI
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI itsLockedText;
    #endregion

    public void Interact()
    {
        ItsLockedMessage();
        if (GameManager.Instance.CurrentItemState == ItemState.Key)
        {
            currentDoorState = DoorState.Idle;
            GameManager.Instance.CurrentItemState = ItemState.None;
        }

        switch (currentDoorState)
        {
            case DoorState.Idle:
                baseDoorAnimator.SetTrigger("Open");
                StartCoroutine(OpenDoor());
                break;
            case DoorState.Opening:
                baseDoorAnimator.SetTrigger("Close");
                StartCoroutine(CloseDoor());
                break;
        }
    }

    public void ItsLockedMessage()
    {
        StartCoroutine(ItsLockedDelay());
    }

    private IEnumerator OpenDoor()
    {
        baseDoorAnimator.SetTrigger("Open");
        currentDoorState = DoorState.Opening;

        DisableAllDoorColliders();
        yield return new WaitForSeconds(doorAnimationDelay);

        currentDoorState = DoorState.Opening;
        EnableAllDoorColliders();
    }

    private IEnumerator CloseDoor()
    {
        baseDoorAnimator.SetTrigger("Close");
        currentDoorState = DoorState.Closing;

        DisableAllDoorColliders();
        yield return new WaitForSeconds(doorAnimationDelay);

        currentDoorState = DoorState.Idle;
        EnableAllDoorColliders();
    }

    private void DisableAllDoorColliders()
    {
        foreach (var col in doorColliders)
            col.enabled = false;
    }

    private void EnableAllDoorColliders()
    {
        foreach (var col in doorColliders)
            col.enabled = true;
    }

    public IEnumerator ItsLockedDelay()
    {
        itsLockedText.enabled = true;
        //canInteract = false;
        yield return new WaitForSeconds(itsLockedTextDelay);
        itsLockedText.enabled = false;
        //canInteract = true;
    }
}
