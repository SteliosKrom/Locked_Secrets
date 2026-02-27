using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public struct AudioItem
    {
        public AudioSource source;
        public AudioClip clip;
    }

    #region AUDIO MANAGER
    [Header("OBJECTS")]
    [SerializeField] private GameObject triggerInteractable3DAudio;

    [Header("MAIN AUDIO SOURCES")]
    [SerializeField] private AudioSource mainGameAudioSource;
    [SerializeField] private AudioSource mainMenuAudioSource;

    [Header("ALL SOUNDS")]
    [SerializeField] private AudioItem[] allSFX;

    [Header("DOOR SOUNDS")]
    [SerializeField] private AudioItem openDoor;
    [SerializeField] private AudioItem closeDoor;
    [SerializeField] private AudioItem lockedDoor;
    [SerializeField] private AudioItem unlockedDoor;
    [SerializeField] private AudioItem lockedCrateDoor;
    [SerializeField] private AudioItem unlockCrateDoor;

    [Header("TRIGGER/HORROR SOUNDS")]
    [SerializeField] private AudioItem horrorSound;
    [SerializeField] private AudioItem breathingSound;
    [SerializeField] private AudioItem lanternLightFlicker;
    [SerializeField] private AudioItem footsteps;
    [SerializeField] private AudioItem doorKnock;
    [SerializeField] private AudioItem instantJumpscare;
    [SerializeField] private AudioItem scream;

    [Header("ITEM/PUZZLE SOUNDS")]
    [SerializeField] private AudioItem letter;
    [SerializeField] private AudioItem firstPuzzleInteract;
    [SerializeField] private AudioItem cutWoodPlank;
    [SerializeField] private AudioItem keypadButton;
    [SerializeField] private AudioItem keypadFailed;
    #endregion

    public GameObject TriggerInteractable3DAudio => triggerInteractable3DAudio;
    public AudioSource MainGameAudioSource => mainGameAudioSource;

    public AudioItem[] AllSFX => allSFX;
    public AudioItem KeypadButton => keypadButton; public AudioItem KeypadFailed => keypadFailed;
    public AudioItem OpenDoor => openDoor; public AudioItem CloseDoor => closeDoor;
    public AudioItem LockedDoor => lockedDoor; public AudioItem Letter => letter;
    public AudioItem FirstPuzzleInteract => firstPuzzleInteract; public AudioItem CutWoodPlank => cutWoodPlank;
    public AudioItem UnlockedDoor => unlockedDoor; public AudioItem HorrorSound => horrorSound;
    public AudioItem BreathingSound => breathingSound; public AudioItem LockedCrateDoor => lockedCrateDoor;
    public AudioItem UnlockCrateDoor => unlockCrateDoor; public AudioItem LanternLightFlicker => lanternLightFlicker;
    public AudioItem FootSteps => footsteps; public AudioItem InstantJumpscare => instantJumpscare;
    public AudioItem DoorKnock => doorKnock; public AudioItem Scream => scream;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate singleton instance detected. Destroying the new one.");
            Destroy(gameObject);
        }
    }

    // Playing SFX one time only
    public void PlaySFX(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    // Pause SFX
    public void PauseSFX(AudioSource source)
    {
        source.Pause();
    }

    // Unpause SFX
    public void UnpauseSFX(AudioSource source)
    {
        source.UnPause();
    }

    // Play/Stop Menu Music
    public void PlayMenuMusic() { mainMenuAudioSource.Play(); }
    public void StopMenuMusic() { mainMenuAudioSource.Stop(); }

    // Play/Stop Main Game Music
    public void PlayMainGameMusic() { mainGameAudioSource.Play(); }
    public void StopMainGameMusic() { mainGameAudioSource.Stop(); }

    // Pause/Unpause Menu Music
    public void PauseMenuMusic() { mainMenuAudioSource.Pause(); }
    public void UnpauseMenuMusic() { mainMenuAudioSource.UnPause(); }

    // Pause/Unpause Main Game Music
    public void PauseMainGameMusic() { mainGameAudioSource.Pause(); }
    public void UnpauseMainGameMusic() { mainGameAudioSource.UnPause(); }
}
