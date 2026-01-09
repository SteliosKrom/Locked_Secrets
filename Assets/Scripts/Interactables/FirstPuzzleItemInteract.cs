using UnityEngine;

public class FirstPuzzleItemInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private PuzzleRole puzzleRole;

    public PuzzleRole PuzzleRole => puzzleRole;

    public void Interact()
    {
        AudioManager audioManager = AudioManager.Instance;

        audioManager.PlaySFX(audioManager.FirstPuzzleItemsInteractAudioSource, audioManager.FirstPuzzleItemsInteractAudioClip);

        PuzzleManager.Instance.OnFirstPuzzleItemInteracted(this);
    }
}
