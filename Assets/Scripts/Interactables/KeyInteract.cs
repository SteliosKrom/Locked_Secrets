using UnityEngine;

public class KeyInteract : MonoBehaviour, IInteractable
{
    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUIManager;
    [SerializeField] private PuzzleManager puzzleManager;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator keyAnimator;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject keyIcon;
    [SerializeField] private GameObject keyInventoryItem;
    #endregion

    public void Interact()
    {
        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnRoomKeyMenu;
        GameManager.Instance.CurrentItemState = ItemState.Key;

        mainGameUIManager.GotRoomKeyPanel.SetActive(true);

        puzzleManager.Key.SetActive(false);
        keyInventoryItem.SetActive(true);
        keyIcon.SetActive(true);
    }
}
