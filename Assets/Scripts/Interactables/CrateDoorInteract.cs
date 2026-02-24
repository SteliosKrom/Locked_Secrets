using System.Collections;
using UnityEngine;

public class CrateDoorInteract : MonoBehaviour, IInteractable
{
    private float interactDelay = 1f;

    private bool canInteract = true;

    [SerializeField] private GameObject crateInformText;

    public void Interact()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (canInteract)
            {
                crateInformText.SetActive(true);
            }
            else
            {
                crateInformText.SetActive(false);
            }
            StartCoroutine(InteractWithCrateDelay());
        }
    }

    public IEnumerator InteractWithCrateDelay()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactDelay);
        canInteract = true;
    }
}
