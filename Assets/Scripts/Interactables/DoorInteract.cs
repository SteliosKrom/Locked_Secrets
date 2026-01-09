using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    private DoorState currentDoorState = DoorState.Idle;

    private float doorAnimationDelay = 1f;

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

    public void Interact()
    {
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

    private IEnumerator OpenDoor()
    {
        AudioManager audioManager = AudioManager.Instance;

        baseDoorAnimator.SetTrigger("Open");
        currentDoorState = DoorState.Opening;

        audioManager.PlaySFX(audioManager.OpenDoorAudioSource, audioManager.OpenDoorAudioClip);
        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        currentDoorState = DoorState.Opening;
        EnableAllDoorColliders();
    }

    private IEnumerator CloseDoor()
    {
        AudioManager audioManager = AudioManager.Instance;

        baseDoorAnimator.SetTrigger("Close");
        currentDoorState = DoorState.Closing;

        audioManager.PlaySFX(audioManager.CloseDoorAudioSource, audioManager.CloseDoorAudioClip);
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
}
