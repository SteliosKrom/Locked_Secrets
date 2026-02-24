using System.Collections;
using UnityEngine;

public class SkeletonHandInteract : MonoBehaviour, IInteractable
{
    private float skeletonHandInteractDelay = 2f;  
    private float openCrateDoorSoundDelay = 2f;

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
                crossInHand.SetActive(true);
                playerCross.SetActive(false);
                StartCoroutine(PlayOpenCrateDoorSoundDelay());
                axeWorld.SetActive(true);
            }
        }
    }

    public IEnumerator PlayOpenCrateDoorSoundDelay()
    {
        yield return new WaitForSeconds(openCrateDoorSoundDelay);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.UnlockCrateDoor.source, AudioManager.Instance.UnlockCrateDoor.clip);
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
