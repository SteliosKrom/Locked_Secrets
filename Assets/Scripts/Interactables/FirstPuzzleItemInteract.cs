using UnityEngine;

public class FirstPuzzleItemInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private PuzzleRole puzzleRole;

    public PuzzleRole PuzzleRole => puzzleRole;

    public void Interact()
    {
        AudioManager.Instance.FirstPuzzleInteractAudioSource.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        PuzzleManager.Instance.OnFirstPuzzleItemInteracted(this);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.FirstPuzzleInteractAudioSource, AudioManager.Instance.FirstPuzzleItemsInteractAudioClip);
    }
}
