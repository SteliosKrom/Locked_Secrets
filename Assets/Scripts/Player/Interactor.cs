using UnityEngine;

public class Interactor : MonoBehaviour
{
    private IInteractable currentInteractable;

    #region LAYERS
    [Header("LAYERS")]
    [SerializeField] private LayerMask interactable;
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
        DetectInteractable();
        InputForInteraction();
    }

    public void InputForInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    public void DetectInteractable()
    {
        Ray ray = new Ray(interactionSource.position, interactionSource.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactable))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();

            if (GameManager.Instance.CurrentGameState != GameState.OnPlaying)
            {
                interactHUD.SetActive(false);
                dot.SetActive(false);
                return;
            }
            interactHUD.SetActive(true);
            dot.SetActive(false);
        }
        else
        {
            if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
            {
                interactHUD.SetActive(false);
                dot.SetActive(true);
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red);
    }
}
