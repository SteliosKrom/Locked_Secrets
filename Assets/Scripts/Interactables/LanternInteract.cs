using UnityEngine;

public class LanternInteract : MonoBehaviour, IInteractable
{
    private bool hasLantern = false;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject worldLantern;
    [SerializeField] private GameObject playerLantern;
    [SerializeField] private GameObject lanternInventoryItem;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATOR")]
    [SerializeField] private Animator baseEquipItemAnimator;
    #endregion

    public bool HasLantern { get => hasLantern; set => hasLantern = value; }

    public void Interact()
    {
        InventoryManager.Instance.AddToInventory(lanternInventoryItem);
        hasLantern = true;

        worldLantern.SetActive(false);
        playerLantern.SetActive(true);

        mainGameUIManager.GotLanternPanel.SetActive(true);

        AudioManager.Instance.LanternLightFlicker.source.Play();
        baseEquipItemAnimator.SetTrigger("Equip");

        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnLanternMenu;
    }
}
