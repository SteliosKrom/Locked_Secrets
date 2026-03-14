using System.Collections;
using UnityEngine;

public class CrateDoorInteract : MonoBehaviour, IInteractable
{
    private float interactDelay = 2f;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject crateInformText;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private BoxCollider crateCollider;
    #endregion

    public GameObject CrateInformText { get => crateInformText; set => crateInformText = value; } 

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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedCrateDoor);

        crateCollider.enabled = false;
        crateInformText.SetActive(true);

        yield return new WaitForSeconds(interactDelay);

        crateInformText.SetActive(false);
        crateCollider.enabled = true;
    }
}
