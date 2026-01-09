using System.Collections;
using TMPro;
using UnityEngine;

public class SmallRoomDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorState currentDoorState = DoorState.Locked;

    private float doorAnimationDelay = 1f;
    private float itsLockedTextDelay = 1f;

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
    [SerializeField] private AudioSource unlockedAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip unlockedAudioClip;
    #endregion

    public GameObject ItsLockedText => itsLockedText;
    public AudioSource UnlockedAudioSource => unlockedAudioSource;
    public AudioClip UnlockedAudioClip => unlockedAudioClip;

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

    public IEnumerator ItsLockedDelay()
    {
        AudioManager audioManager = AudioManager.Instance;

        itsLockedText.SetActive(true);
        canInteract = false;

        audioManager.PlaySFX(audioManager.LockedAudioSource, audioManager.LockedAudioClip);

        yield return new WaitForSeconds(itsLockedTextDelay);

        itsLockedText.SetActive(false);
        canInteract = true;
    }
}
