using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    private DoorState currentDoorState = DoorState.Idle;

    private float doorAnimationDelay = 1f;

    [SerializeField] private bool isSpecialDoor;

    #region ANIMATIONS
    [Header("ANIMATOR")]
    [SerializeField] private Animator baseDoorAnimator;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider[] doorColliders;
    private BoxCollider otherDoorHandleCollider;
    #endregion

    public Animator BaseDoorAnimator => baseDoorAnimator;
    public BoxCollider OtherDoorHandleCollider { get => otherDoorHandleCollider; }
    public DoorState CurrentDoorState { get => currentDoorState; set => currentDoorState = value; }

    private void Awake()
    {
        otherDoorHandleCollider = GameObject.Find("OtherDoorHandle").GetComponent<BoxCollider>();
    }

    public void Interact()
    {
        switch (currentDoorState)
        {
            case DoorState.Idle:
                StartCoroutine(OpenDoor());
                break;
            case DoorState.Opening:
                StartCoroutine(CloseDoor());
                break;
        }
    }

    private IEnumerator OpenDoor()
    {
        currentDoorState = DoorState.Opening;
        baseDoorAnimator.SetTrigger("Open");

        AudioManager.Instance.OpenDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor.source, AudioManager.Instance.OpenDoor.clip);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        currentDoorState = DoorState.Opening;
        EnableAllDoorColliders();

        if (isSpecialDoor)
        {
            otherDoorHandleCollider.enabled = false;
            isSpecialDoor = false;
        }
    }

    private IEnumerator CloseDoor()
    {
        baseDoorAnimator.SetTrigger("Close");
        currentDoorState = DoorState.Closing;

        AudioManager.Instance.CloseDoor.source.transform.SetParent(transform, false);
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
}
