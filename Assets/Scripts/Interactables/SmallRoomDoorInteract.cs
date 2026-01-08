using System.Collections;
using TMPro;
using UnityEngine;

public class SmallRoomDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorState currentDoorState = DoorState.Locked;

    private float doorAnimationDelay = 1f;
    private float itsLockedTextDelay = 2f;

    private bool canInteract = true;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private DoorInteract doorInteract;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATOR")]
    [SerializeField] private Animator baseDoorAnimator;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider[] doorColliders;
    #endregion

    #region UI
    [Header("OBJECTS")]
    [SerializeField] private GameObject itsLockedText;
    #endregion

    #region AUDIO
    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource lockedAudioSource;
    [SerializeField] private AudioSource unlockedAudioSource;
    [SerializeField] private AudioSource openDoorAudioSource;
    [SerializeField] private AudioSource closeDoorAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip lockedAudioClip;
    [SerializeField] private AudioClip unlockedAudioClip;
    [SerializeField] private AudioClip openDoorAudioClip;
    [SerializeField] private AudioClip closeDoorAudioClip;
    #endregion

    public GameObject ItsLockedText => itsLockedText;
    public AudioSource LockedAudioSource => lockedAudioSource;
    public AudioSource UnlockedAudioSource => unlockedAudioSource;
    public AudioSource OpenDoorAudioSource => openDoorAudioSource;
    public AudioSource CloseDoorAudioSource => closeDoorAudioSource;
    public AudioClip LockedAudioClip => lockedAudioClip;
    public AudioClip UnlockedAudioClip => unlockedAudioClip;
    public AudioClip OpenDoorAudioClip => openDoorAudioClip;
    public AudioClip CloseDoorAudioClip => closeDoorAudioClip;

    public void Interact()
    {
        if (GameManager.Instance.CurrentItemState == ItemState.Key)
        {
            currentDoorState = DoorState.Unlocked;
            GameManager.Instance.CurrentItemState = ItemState.None;
            AudioManager.Instance.PlaySFX(unlockedAudioSource, unlockedAudioClip);
            return;
        }

        if (currentDoorState == DoorState.Unlocked)
        {
            currentDoorState = DoorState.Idle;
        }

        if (currentDoorState == DoorState.Locked)
        {
            if (canInteract)
            {
                ItsLockedMessage();
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

    public void ItsLockedMessage()
    {
        StartCoroutine(ItsLockedDelay());
    }

    private IEnumerator OpenDoor()
    {
        baseDoorAnimator.SetTrigger("Open");
        currentDoorState = DoorState.Opening;

        AudioManager.Instance.PlaySFX(openDoorAudioSource, openDoorAudioClip);
        DisableAllDoorColliders();

        yield return new WaitForSeconds(doorAnimationDelay);

        currentDoorState = DoorState.Opening;
        EnableAllDoorColliders();
    }

    private IEnumerator CloseDoor()
    {
        baseDoorAnimator.SetTrigger("Close");
        currentDoorState = DoorState.Closing;

        AudioManager.Instance.PlaySFX(closeDoorAudioSource, closeDoorAudioClip);
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
        itsLockedText.SetActive(true);
        canInteract = false;

        AudioManager.Instance.PlaySFX(lockedAudioSource, lockedAudioClip);

        yield return new WaitForSeconds(itsLockedTextDelay);

        itsLockedText.SetActive(false);
        canInteract = true;
    }
}
