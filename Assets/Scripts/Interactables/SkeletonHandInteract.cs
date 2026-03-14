using System.Collections;
using UnityEngine;

public class SkeletonHandInteract : MonoBehaviour, IInteractable
{
    private float skeletonHandInteractDelay = 2f;  

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject skeletonHandInformText;
    [SerializeField] private GameObject playerCross;
    [SerializeField] private GameObject crossInHand;
    [SerializeField] private GameObject axeWorld;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private BoxCollider skeletonHandCollider;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator crateAnimator;
    #endregion

    public GameObject SkeletonHandInformText { get => skeletonHandInformText; set => skeletonHandInformText = value; }

    public void Interact()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (GameManager.Instance.CurrentItemState == ItemState.None)
            {
                StartCoroutine(InteractWithHandDelay());
            }
            else if (GameManager.Instance.CurrentItemState == ItemState.Cross)
            {
                GameManager.Instance.CurrentItemState = ItemState.None;
                skeletonHandCollider.enabled = false;

                crossInHand.SetActive(true);
                playerCross.SetActive(false);

                AudioManager.Instance.PlaceItem.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.PlaceItem);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.UnlockCrateDoor);

                crateAnimator.SetTrigger("Open");
                axeWorld.SetActive(true);
            }
        }
    }

    public IEnumerator InteractWithHandDelay()
    {
        skeletonHandCollider.enabled = false;
        skeletonHandInformText.SetActive(true);
        yield return new WaitForSeconds(skeletonHandInteractDelay);
        skeletonHandInformText.SetActive(false);
        skeletonHandCollider.enabled = true;
    }
}
