using System.Collections;
using UnityEngine;

public class SmallCrateDoorInteract : MonoBehaviour, IInteractable
{
    private float interactDelay = 2f;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject smallCrateInformText;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private BoxCollider smallCrateCollider;
    #endregion

    public GameObject SmallCrateInformText { get => smallCrateInformText; set => smallCrateInformText = value; }

    public void Interact()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            StartCoroutine(InteractWithCrateDelay());
        }
    }

    public IEnumerator InteractWithCrateDelay()
    {
        AudioManager.Instance.LockedCrateDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedCrateDoor.source, AudioManager.Instance.LockedCrateDoor.clip);

        smallCrateCollider.enabled = false;
        smallCrateInformText.SetActive(true);

        yield return new WaitForSeconds(interactDelay);

        smallCrateInformText.SetActive(false);
        smallCrateCollider.enabled = true;
    }
}
