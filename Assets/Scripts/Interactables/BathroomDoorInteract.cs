using System.Collections;
using UnityEngine;

public class BathroomDoorInteract : MonoBehaviour, IDoorInteractable
{
    [SerializeField] private DoorState currentDoorState;

    private float doorAnimationDelay = 1f;
    private float doorHandleColliderDelay = 1f;

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator bathroomDoorAnimator;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider[] doorColliders;
    [SerializeField] private Collider doorHandleCollider;
    #endregion

    public Animator BathroomDoorAnimator { get => bathroomDoorAnimator; }
    public DoorState CurrentDoorState { get => currentDoorState; set => currentDoorState = value; }

    private void Start()
    {
        doorHandleCollider.enabled = false;
        currentDoorState = DoorState.OpenIdle;
    }

    public void Interact()
    {
        switch (currentDoorState)
        {
            case DoorState.OpenIdle:
                StartCoroutine(CloseDoor());
                break;
            case DoorState.CloseIdle:
                StartCoroutine(OpenDoor());
                break;
            case DoorState.Locked:
                AudioManager.Instance.LockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedDoor);
                StartCoroutine(LockedBathroomDoorDelay());
                break;
            case DoorState.Unlocked:
                StartCoroutine(OpenDoor());
                break;
        }
    }

    public bool IsLocked()
    {
        return currentDoorState == DoorState.Locked;
    }

    public IEnumerator OpenDoor()
    {
        currentDoorState = DoorState.Opening;
        bathroomDoorAnimator.SetTrigger("Open");

        AudioManager.Instance.OpenDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        EnableAllDoorColliders();
        currentDoorState = DoorState.OpenIdle;
    }

    public IEnumerator CloseDoor()
    {
        currentDoorState = DoorState.Closing;
        bathroomDoorAnimator.SetTrigger("Close");

        AudioManager.Instance.CloseDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CloseDoor);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        EnableAllDoorColliders();
        currentDoorState = DoorState.CloseIdle;
    }

    public void EnableAllDoorColliders()
    {
        foreach (var collider in doorColliders)
            collider.enabled = true;
    }

    public void DisableAllDoorColliders()
    {
        foreach (var collider in doorColliders)
            collider.enabled = false;
    }

    public IEnumerator LockedBathroomDoorDelay()
    {
        doorHandleCollider.enabled = false;
        yield return new WaitForSeconds(doorHandleColliderDelay);
        doorHandleCollider.enabled = true;
    }
}
