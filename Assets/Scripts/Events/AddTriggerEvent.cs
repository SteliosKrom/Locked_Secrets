using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddTriggerEvent : MonoBehaviour
{
    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject lanternItemMenu;
    [SerializeField] private GameObject keyItemMenu;
    [SerializeField] private GameObject crucificItemMenu;
    [SerializeField] private GameObject axeItemMenu;
    [SerializeField] private GameObject inventoryItemButtons;
    #endregion

    public GameObject InventoryItemButtons { get => inventoryItemButtons; }

    // Event Triggers on Menu Buttons
    public void PointerEnterText(TextMeshProUGUI text)
    {
        text.color = Color.red;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.HoverAudioSource);
    }
    public void PointerExitText(TextMeshProUGUI text)
    {
        text.color = Color.white;
    }
    public void PointerClickText(TextMeshProUGUI text)
    {
        text.color = Color.white;
    }

    // Event Triggers on Inventory Item Buttons
    public void PointerEnterInventoryItemButton(Image itemButtonImage)
    {
        itemButtonImage.color = new Color32(149, 149, 149, 255);
    }
    public void PointerExitInventoryItemButton(Image itemButtonImage)
    {
        itemButtonImage.color = new Color32(255, 255, 255, 255);
    }
    public void PointerClickInventoryItemButton(Image itemButtonImage)
    {
        switch (itemButtonImage.gameObject.tag)
        {
            case "Lantern":
                lanternItemMenu.SetActive(true);
                break;
            case "Key":
                keyItemMenu.SetActive(true);
                break;
            case "Crucifix":
                crucificItemMenu.SetActive(true);
                break;
            case "Axe":
                axeItemMenu.SetActive(true);
                break;
        }
        inventoryItemButtons.SetActive(false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ClickInventoryItemAudioSource);
        GameManager.Instance.CurrentMenuState = MenuState.OnInventoryItemsMenu;
        itemButtonImage.color = new Color32(120, 120, 120, 255);
    }

    // Event triggers on Quality Dropdown buttons

    public void PointerEnterQualityDropdownButton(Image image)
    {
        image.color = Color.red;
    }

    public void PointerExitQualityDropdownButton(Image image)
    {
        image.color = Color.white;
    }
}
