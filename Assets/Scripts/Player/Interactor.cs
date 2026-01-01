using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private bool detected = false;

    #region LAYERS
    [Header("LAYERS")]
    [SerializeField] private LayerMask interactable;
    [SerializeField] private LayerMask obstacle;
    #endregion

    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] private Transform interactionSource;
    private float interactionRange = 1.25f;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject interactHUD;
    [SerializeField] private GameObject dot;
    #endregion

    private void Update()
    {
        Debug.DrawRay(interactionSource.position, interactionSource.forward * interactionRange, Color.red);
        DetectAndInteract();
    }

    public void DetectAndInteract()
    {
        LayerMask combinedMask = interactable | obstacle;

        if (GameManager.Instance.CurrentGameState != GameState.OnPlaying)
        {
            interactHUD.SetActive(false);
            dot.SetActive(false);
            return;
        }

        if (Physics.Raycast(interactionSource.position, interactionSource.forward, out RaycastHit hit, interactionRange, combinedMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                detected = false;
                interactHUD.SetActive(false);
                dot.SetActive(true);
                return;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                detected = true;
                interactHUD.SetActive(true);
                dot.SetActive(false);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
                return;
            }
        }
        else
        {
            detected = false;
            interactHUD.SetActive(false);
            dot.SetActive(true);
        }
    }
}
