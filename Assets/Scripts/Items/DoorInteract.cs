using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    #region ANIMATIONS
    [Header("ANIMATOR")]
    [SerializeField] private Animator doorAnimator;
    #endregion

    public void Interact()
    {
        doorAnimator.SetBool("Opened", true);
    }
}
