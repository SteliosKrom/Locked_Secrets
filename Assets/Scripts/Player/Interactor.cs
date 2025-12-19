using UnityEngine;

public class Interactor : MonoBehaviour
{
    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Interactable currentInteractable;
    #endregion

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
            currentInteractable = hit.collider.GetComponent<Interactable>();

            switch (GameManager.Instance.CurrentGameState)
            {
                case GameState.OnPlaying:
                    interactHUD.SetActive(true);
                    break;
                case GameState.OnPaused:
                    interactHUD.SetActive(false);
                    dot.SetActive(false);
                    break;
                default:
                    interactHUD.SetActive(false);
                    dot.SetActive(false);
                    break;
            }
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
