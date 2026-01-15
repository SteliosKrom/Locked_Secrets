using System.Collections;
using UnityEngine;

public class MainDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorState currentDoorState = DoorState.Locked;

    private float doorAnimationDelay = 1f;
    private float itsLockedTextDelay = 1f;

    private bool canInteract = true;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private SmallRoomDoorInteract smallRoomDoorInteract;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATOR")]
    [SerializeField] private Animator baseDoorAnimator;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider[] doorColliders;
    #endregion

    public DoorState CurrentDoorState { get => currentDoorState; set => currentDoorState = value; }

    public void Interact()
    {
        if (currentDoorState == DoorState.Locked)
        {
            if (canInteract)
            {
                ItsLocked();
            }
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

    public void ItsLocked()
    {
        StartCoroutine(ItsLockedDelay());
    }

    private IEnumerator OpenDoor()
    {
        baseDoorAnimator.SetTrigger("Open");
        currentDoorState = DoorState.Opening;

        AudioManager.Instance.OpenDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor.source, AudioManager.Instance.OpenDoor.clip);
        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        currentDoorState = DoorState.Opening;
        EnableAllDoorColliders();
    }

    private IEnumerator CloseDoor()
    {
        baseDoorAnimator.SetTrigger("Close");
        currentDoorState = DoorState.Closing;

        AudioManager.Instance.CloseDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CloseDoor.source, AudioManager.Instance.CloseDoor.clip);
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
        smallRoomDoorInteract.ItsLockedText.SetActive(true);
        canInteract = false;

        AudioManager.Instance.LockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedDoor.source, AudioManager.Instance.LockedDoor.clip);

        yield return new WaitForSeconds(itsLockedTextDelay);

        smallRoomDoorInteract.ItsLockedText.SetActive(false);
        canInteract = true;
    }
}
