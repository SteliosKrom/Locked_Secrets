using UnityEngine;

public class KeypadPuzzleButtonInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private KeypadButtonRoles keypadButtonRoles;

    public KeypadButtonRoles KeypadPuzzleButtonRoles => keypadButtonRoles;

    public void Interact()
    {
        PuzzleManager.Instance.OnKeypadPuzzleButtonInteracted(this);
    }
}
