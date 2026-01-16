using UnityEngine;

public class LanternInteract : MonoBehaviour, IInteractable
{
    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject worldLantern;
    [SerializeField] private GameObject playerLantern;
    [SerializeField] private GameObject lanternIcon;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATOR")]
    [SerializeField] private Animator baseEquipItemAnimator;
    #endregion

    public void Interact()
    {
        worldLantern.SetActive(false);
        playerLantern.SetActive(true);
        lanternIcon.SetActive(true);
        baseEquipItemAnimator.SetTrigger("Equip");
        mainGameUIManager.GotLanternPanel.SetActive(true);
        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnLanternMenu;
    }
}
