using System.Collections;
using UnityEngine;

public class BathroomDoorInteract : MonoBehaviour, IInteractable
{
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

    private void Start()
    {
        doorHandleCollider.enabled = false;
    }

    public void Interact()
    {
        switch (GameManager.Instance.CurrentBathroomDoorState)
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
        GameManager.Instance.CurrentBathroomDoorState = BathroomDoorState.Opening;
        bathroomDoorAnimator.SetTrigger("Open");

        AudioManager.Instance.OpenDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor.source, AudioManager.Instance.OpenDoor.clip);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        EnableAllDoorColliders();
        GameManager.Instance.CurrentBathroomDoorState = BathroomDoorState.OpenIdle;
    }

    public IEnumerator CloseDoor()
    {
        GameManager.Instance.CurrentBathroomDoorState = BathroomDoorState.Closing;
        bathroomDoorAnimator.SetTrigger("Close");

        AudioManager.Instance.CloseDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CloseDoor.source, AudioManager.Instance.CloseDoor.clip);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        EnableAllDoorColliders();
        GameManager.Instance.CurrentBathroomDoorState = BathroomDoorState.CloseIdle;
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
