using System.Collections;
using TMPro;
using UnityEngine;

public class SmallRoomDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorState currentDoorState = DoorState.Locked;

    private float doorAnimationDelay = 1f;
    private float itsLockedTextDelay = 1f;
    private float doorHandleColliderEnableDelay = 1f;

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
    [SerializeField] private GameObject itsLockedText;
    [SerializeField] private GameObject needLanternText;
    #endregion

    public GameObject ItsLockedText => itsLockedText;

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
        AudioManager.Instance.UnlockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.UnlockedDoor.source, AudioManager.Instance.UnlockedDoor.clip);
        keyInteract.PlayerKey.SetActive(false);
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

    public IEnumerator ItsLockedDelay()
    {
        doorHandleCollider.enabled = false;
        itsLockedText.SetActive(true);

        AudioManager.Instance.LockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedDoor.source, AudioManager.Instance.LockedDoor.clip);

        yield return new WaitForSeconds(itsLockedTextDelay);

        itsLockedText.SetActive(false);
        doorHandleCollider.enabled = true;
    }

    public IEnumerator NeedLanternDelay()
    {
        doorHandleCollider.enabled = false;
        needLanternText.SetActive(true);

        AudioManager.Instance.LockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedDoor.source, AudioManager.Instance.LockedDoor.clip);

        yield return new WaitForSeconds(itsLockedTextDelay);

        needLanternText.SetActive(false);
        doorHandleCollider.enabled = true;
    }
}
