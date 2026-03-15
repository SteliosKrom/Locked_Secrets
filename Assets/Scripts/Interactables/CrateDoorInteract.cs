using System.Collections;
using UnityEngine;

public class CrateDoorInteract : MonoBehaviour, IDoorInteractable
{
    [SerializeField] private DoorState currentDoorState;

    private float interactDelay = 2f;

    #region COLLIDERS
    [Header("COLLIDERS")]
    private BoxCollider crateCollider;
    #endregion

    public DoorState CurrentDoorState { get => currentDoorState; set => currentDoorState = value; }

    private void Awake()
    {
        crateCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        currentDoorState = DoorState.Locked;
    }

    public void Interact()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            StartCoroutine(InteractWithCrateDelay());
        }
    }

    public bool IsLocked()
    {
        return currentDoorState == DoorState.Locked;
    }

    public IEnumerator InteractWithCrateDelay()
    {
        AudioManager.Instance.LockedCrateDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedCrateDoor);
        crateCollider.enabled = false;

        yield return new WaitForSeconds(interactDelay);

        crateCollider.enabled = true;
    }
}
