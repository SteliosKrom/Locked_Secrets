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

    [Header("DOOR SOUNDS")]
    [SerializeField] private AudioItem openDoor;
    [SerializeField] private AudioItem closeDoor;
    [SerializeField] private AudioItem lockedDoor;

    [Header("ITEM SOUNDS")]
    [SerializeField] private AudioItem letter;
    [SerializeField] private AudioItem firstPuzzleInteract;
    [SerializeField] private AudioItem cutWoodPlank;
    [SerializeField] private AudioItem keypadButton;
    #endregion

    public GameObject TriggerInteractable3DAudio => triggerInteractable3DAudio;

    public AudioSource KeypadButtonAudioSource => keypadButton.source;
    public AudioClip KeypadButtonAudioClip => keypadButton.clip;

    public AudioSource OpenDoorAudioSource => openDoor.source;
    public AudioClip OpenDoorAudioClip => openDoor.clip;

    public AudioSource CloseDoorAudioSource => closeDoor.source;
    public AudioClip CloseDoorAudioClip => closeDoor.clip;

    public AudioSource LockedAudioSource => lockedDoor.source;
    public AudioClip LockedAudioClip => lockedDoor.clip;

    public AudioSource LetterAudioSource => letter.source;
    public AudioClip LetterAudioClip => letter.clip;

    public AudioSource FirstPuzzleInteractAudioSource => firstPuzzleInteract.source;
    public AudioClip FirstPuzzleItemsInteractAudioClip => firstPuzzleInteract.clip;

    public AudioSource CutWoodPlankAudioSource => cutWoodPlank.source;
    public AudioClip CutWoodPlankAudioClip => cutWoodPlank.clip;

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
