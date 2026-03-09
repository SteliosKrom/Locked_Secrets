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
    [SerializeField] private AudioSource rainAudioSource;
    [SerializeField] private AudioLowPassFilter rainAudioLowPassFilter;
    [SerializeField] private AudioLowPassFilter doorKnockLowPassFilter;

    [Header("ALL SOUNDS")]
    [SerializeField] private AudioItem[] allSFX;

    [Header("DOOR SOUNDS")]
    [SerializeField] private AudioItem openDoor;
    [SerializeField] private AudioItem closeDoor;
    [SerializeField] private AudioItem lockedDoor;
    [SerializeField] private AudioItem unlockedDoor;
    [SerializeField] private AudioItem lockedCrateDoor;
    [SerializeField] private AudioItem unlockCrateDoor;
    [SerializeField] private AudioItem unlockSmallCrateDoor;
    [SerializeField] private AudioItem creakingDoorOpening;

    [Header("TRIGGER/HORROR SOUNDS")]
    [SerializeField] private AudioItem horrorSound;
    [SerializeField] private AudioItem breathingSound;
    [SerializeField] private AudioItem footsteps;
    [SerializeField] private AudioItem doorKnock;
    [SerializeField] private AudioItem longDoorKnock;
    [SerializeField] private AudioItem behindYou;
    [SerializeField] private AudioItem instantJumpscare;
    [SerializeField] private AudioItem scream;

    [Header("ITEM/PUZZLE SOUNDS")]
    [SerializeField] private AudioItem letter;
    [SerializeField] private AudioItem lanternLightFlicker;
    [SerializeField] private AudioItem firstPuzzleInteract;
    [SerializeField] private AudioItem cutWoodPlank;
    [SerializeField] private AudioItem keypadButton;
    [SerializeField] private AudioItem keypadFailed;
    [SerializeField] private AudioItem placeItem;

    [Header("UI SOUNDS")]
    [SerializeField] private AudioItem pressAnyKeyAudioSource;
    #endregion

    public GameObject TriggerInteractable3DAudio => triggerInteractable3DAudio;
    public AudioSource MainGameAudioSource => mainGameAudioSource;
    public AudioLowPassFilter RainAudioLowPassFilter => rainAudioLowPassFilter;
    public AudioLowPassFilter DoorKnockLowPassFilter => doorKnockLowPassFilter;

    public AudioItem[] AllSFX => allSFX; 
    public AudioItem KeypadButton => keypadButton; public AudioItem KeypadFailed => keypadFailed; public AudioItem OpenDoor => openDoor; 
    public AudioItem CloseDoor => closeDoor; public AudioItem LockedDoor => lockedDoor; public AudioItem Letter => letter;
    public AudioItem FirstPuzzleInteract => firstPuzzleInteract; public AudioItem CutWoodPlank => cutWoodPlank; public AudioItem UnlockedDoor => unlockedDoor; 
    public AudioItem HorrorSound => horrorSound; public AudioItem BreathingSound => breathingSound; public AudioItem LockedCrateDoor => lockedCrateDoor; 
    public AudioItem UnlockCrateDoor => unlockCrateDoor; public AudioItem LanternLightFlicker => lanternLightFlicker; public AudioItem FootSteps => footsteps;
    public AudioItem InstantJumpscare => instantJumpscare; public AudioItem DoorKnock => doorKnock; public AudioItem Scream => scream; 
    public AudioItem UnlockSmallCrateDoor => unlockSmallCrateDoor; public AudioItem PlaceItem => placeItem; public AudioItem LongDoorKnock => longDoorKnock; 
    public AudioItem PressAnyKeyAudioSource => pressAnyKeyAudioSource; public AudioItem CreakingDoorOpening => creakingDoorOpening; public AudioItem BehindYou => behindYou;

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

    public void StopSFX(AudioSource source)
    {
        source.Stop();
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

    // PLay/Stop Rain Audio
    public void PlayRainAudio() { rainAudioSource.Play(); }
    public void StopRainAudio() { rainAudioSource.Stop(); }

    // Pause/Unpause Menu Music
    public void PauseMenuMusic() { mainMenuAudioSource.Pause(); }
    public void UnpauseMenuMusic() { mainMenuAudioSource.UnPause(); }

    // Pause/Unpause Main Game Music
    public void PauseMainGameMusic() { mainGameAudioSource.Pause(); }
    public void UnpauseMainGameMusic() { mainGameAudioSource.UnPause(); }

    //Pause/Unpause Rain Audio
    public void PauseRainAudioSound() { rainAudioSource.Pause(); }
    public void UnpauseRainAudioSound() { rainAudioSource.UnPause(); }
}
