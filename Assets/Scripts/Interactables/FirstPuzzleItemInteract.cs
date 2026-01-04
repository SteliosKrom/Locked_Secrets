using UnityEngine;

public class FirstPuzzleItemInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private PuzzleRole puzzleRole;

    public PuzzleRole PuzzleRole => puzzleRole;

    public void Interact()
    {
        PuzzleManager.Instance.OnFirstPuzzleItemInteracted(this);
    }
}
