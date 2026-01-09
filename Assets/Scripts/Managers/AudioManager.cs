using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioSource mainGameAudioSource;
    [SerializeField] private AudioSource mainMenuAudioSource;

    [SerializeField] private AudioSource lockedAudioSource;
    [SerializeField] private AudioSource openDoorAudioSource;
    [SerializeField] private AudioSource closeDoorAudioSource;
    [SerializeField] private AudioSource letterAudioSource;
    [SerializeField] private AudioSource firstPuzzleItemsInteractAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip lockedAudioClip;
    [SerializeField] private AudioClip openDoorAudioClip;
    [SerializeField] private AudioClip closeDoorAudioClip;
    [SerializeField] private AudioClip letterAudioClip;
    [SerializeField] private AudioClip firstPuzzleItemsInteractAudioClip;
    #endregion

    public AudioSource LockedAudioSource => lockedAudioSource;
    public AudioSource OpenDoorAudioSource => openDoorAudioSource;
    public AudioSource CloseDoorAudioSource => closeDoorAudioSource;
    public AudioSource LetterAudioSource => letterAudioSource;
    public AudioSource FirstPuzzleItemsInteractAudioSource => firstPuzzleItemsInteractAudioSource;
    public AudioClip LockedAudioClip => lockedAudioClip;
    public AudioClip OpenDoorAudioClip => openDoorAudioClip;
    public AudioClip CloseDoorAudioClip => closeDoorAudioClip;
    public AudioClip LetterAudioClip => letterAudioClip;
    public AudioClip FirstPuzzleItemsInteractAudioClip => firstPuzzleItemsInteractAudioClip;

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
