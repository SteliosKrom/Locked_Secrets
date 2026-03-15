using System.Collections;
using TMPro;
using UnityEngine;

public class SmallRoomDoorInteract : MonoBehaviour, IDoorInteractable
{
    [SerializeField] private DoorState currentDoorState;

    private float doorAnimationDelay = 1f;
    private float itsLockedTextDelay = 1f;
    private float doorHandleColliderEnableDelay = 1f;
    private float unlockDoorDelay = 1.5f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private DoorInteract doorInteract;
    [SerializeField] private LanternInteract lanternInteract;
    [SerializeField] private KeyInteract keyInteract;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATOR")]
    [SerializeField] private Animator baseDoorAnimator;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider[] doorColliders;
    [SerializeField] private Collider doorHandleCollider;
    #endregion

    #region UI
    [Header("OBJECTS")]
    [SerializeField] private GameObject needLanternText;
    [SerializeField] private GameObject keyDoor;
    #endregion

    public DoorState CurrentDoorState { get => currentDoorState; set => currentDoorState = value; }
    public GameObject NeedLanternText => needLanternText;

    private void Start()
    {
        currentDoorState = DoorState.Locked;
    }

    public void Interact()
    {
        switch (currentDoorState)
        {
            case DoorState.Locked:
                TryInlock();
                break;
            case DoorState.Idle:
                StartCoroutine(OpenDoor());
                break;
            case DoorState.Opening:
                StartCoroutine(CloseDoor());
                break;
        }
    }

    public bool IsLocked()
    {
        return currentDoorState == DoorState.Locked;
    }

    public void TryInlock()
    {
        if (GameManager.Instance.CurrentItemState != ItemState.Key)
        {
            ItsLockedMessage();
            return;
        }
        if (!lanternInteract.HasLantern)
        {
            ShowNeedLanternText();
            return;
        }
        UnlockDoor();
    }

    public void UnlockDoor()
    {
        currentDoorState = DoorState.Idle;
        GameManager.Instance.CurrentItemState = ItemState.None;
        GameManager.Instance.IsDoorUnlocking = true;

        keyDoor.SetActive(true);
        StartCoroutine(UnlockDoorDelay());

        StartCoroutine(EnableDoorHandleColliderDelay());
        return;
    }

    public void ShowNeedLanternText()
    {
        StartCoroutine(NeedLanternDelay());
    }

    public void ItsLockedMessage()
    {
        StartCoroutine(ItsLockedDelay());
    }

    private void EnableAllDoorColliders()
    {
        foreach (var col in doorColliders)
            col.enabled = true;
    }

    private void DisableAllDoorColliders()
    {
        foreach (var col in doorColliders)
            col.enabled = false;
    }

    private IEnumerator EnableDoorHandleColliderDelay()
    {
        doorHandleCollider.enabled = false;
        yield return new WaitForSeconds(doorHandleColliderEnableDelay);
        doorHandleCollider.enabled = true;
    }

    private IEnumerator OpenDoor()
    {
        baseDoorAnimator.SetTrigger("Open");
        currentDoorState = DoorState.Opening;

        AudioManager.Instance.OpenDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenDoor);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        currentDoorState = DoorState.Opening;
        EnableAllDoorColliders();
    }

    private IEnumerator CloseDoor()
    {
        baseDoorAnimator.SetTrigger("Close");
        currentDoorState = DoorState.Closing;

        AudioManager.Instance.CloseDoor.source.transform.SetParent(transform, false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CloseDoor);

        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        currentDoorState = DoorState.Idle;
        EnableAllDoorColliders();
    }

    public IEnumerator ItsLockedDelay()
    {
        doorHandleCollider.enabled = false;

        AudioManager.Instance.LockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedDoor);

        yield return new WaitForSeconds(itsLockedTextDelay);

        doorHandleCollider.enabled = true;
    }

    public IEnumerator UnlockDoorDelay()
    {
        yield return new WaitForSeconds(unlockDoorDelay);
        keyDoor.SetActive(false);
        GameManager.Instance.IsDoorUnlocking = false;
    }

    public IEnumerator NeedLanternDelay()
    {
        doorHandleCollider.enabled = false;
        needLanternText.SetActive(true);

        yield return new WaitForSeconds(itsLockedTextDelay);

        needLanternText.SetActive(false);
        doorHandleCollider.enabled = true;
    }
}
