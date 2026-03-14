using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class KeypadPuzzleButtonInteract : MonoBehaviour, IInteractable
{
    private float buttonPressDelay = 0.75f;

    [SerializeField] private KeypadButtonRoles keypadButtonRoles;

    private Animator keypadButtonAnimator;

    public KeypadButtonRoles KeypadPuzzleButtonRoles => keypadButtonRoles;

    private void Awake()
    {
        keypadButtonAnimator = GetComponentInParent<Animator>();
    }

    public void Interact()
    {
        PuzzleManager.Instance.OnKeypadPuzzleButtonInteracted(this);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.KeypadButton);

        keypadButtonAnimator.SetTrigger("Pressed");
        StartCoroutine(DisableColliderTemporarily());
    }

    public IEnumerator DisableColliderTemporarily()
    {
        PuzzleManager.Instance.DisableKeypadButtonColliders();
        yield return new WaitForSeconds(buttonPressDelay);

        if (PuzzleManager.Instance.KeypadPuzzleActive)
        {
            PuzzleManager.Instance.EnableKeypadPuzzleButtonColliders();
        }
    }
}
