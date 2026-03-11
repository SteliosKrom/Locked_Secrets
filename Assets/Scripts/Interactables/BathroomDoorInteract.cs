using System.Collections;
using UnityEngine;

public class BathroomDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private BathroomDoorState currentDoorState;

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
    public BathroomDoorState CurrentDoorState { get => currentDoorState; set => currentDoorState = value; }

    private void Start()
    {
        doorHandleCollider.enabled = false;
        currentDoorState = BathroomDoorState.OpenIdle;
    }

    public void Interact()
    {
        switch (currentDoorState)
        {
            case BathroomDoorState.OpenIdle:
                StartCoroutine(CloseDoor());
                break;
            case BathroomDoorState.CloseIdle:
                StartCoroutine(OpenDoor());
                break;
            case BathroomDoorState.Locked:
                AudioManager.Instance.LockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedDoor.source, AudioManager.Instance.LockedDoor.clip);
                StartCoroutine(LockedBathroomDoorDelay());
                break;
            case BathroomDoorState.Unlocked:
                StartCoroutine(OpenDoor());
                break;
        }
    }

    public IEnumerator OpenDoor()
    {
        currentDoorState = BathroomDoorState.Opening;
        bathroomDoorAnimator.SetTrigger("Open");

        AudioManager.Instance.OpenDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor.source, AudioManager.Instance.OpenDoor.clip);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        EnableAllDoorColliders();
        currentDoorState = BathroomDoorState.OpenIdle;
    }

    public IEnumerator CloseDoor()
    {
        currentDoorState = BathroomDoorState.Closing;
        bathroomDoorAnimator.SetTrigger("Close");

        AudioManager.Instance.CloseDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CloseDoor.source, AudioManager.Instance.CloseDoor.clip);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        EnableAllDoorColliders();
        currentDoorState = BathroomDoorState.CloseIdle;
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
