using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private bool isInventoryOpen = false;
    [SerializeField] private bool canOpenInventory = true;
    [SerializeField] private bool isInventoryEmpty = true;

    private float inventoryInputDelay = 1f;

    private int nextEmptySlot;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private AddTriggerEvent addTriggerEvent;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject[] inventoryItems;
    [SerializeField] private GameObject[] inventorySlots;
    [SerializeField] private GameObject[] inventoryItemMenus;
    [SerializeField] private GameObject inventory;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Start()
    {
        isInventoryEmpty = true;
        nextEmptySlot = 0;
    }

    private void Update()
    {
        InventoryInput();
    }

    public void AddToInventory(GameObject item)
    {
        while (nextEmptySlot <= inventoryItems.Length)
        {
            if (isInventoryEmpty)
            {
                inventoryItems[nextEmptySlot] = item;
                item.transform.position = inventorySlots[nextEmptySlot].transform.position;
                item.transform.position = inventorySlots[nextEmptySlot].transform.position;
                item.SetActive(true);
                nextEmptySlot++;
                return;
            }
        }
        Debug.Log("Inventory is full!");
        isInventoryEmpty = false;
    }

    public void InventoryInput()
    {
        if (GameManager.Instance.CurrentMenuState == MenuState.OnNoteMenu) return;
        if (GameManager.Instance.CurrentGameState == GameState.OnEnding) return;
        if (GameManager.Instance.CanItemMenuInteract()) return;
        if (GameManager.Instance.IsDoorUnlocking == true) return;

        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (Input.GetKeyDown(KeyCode.I) && canOpenInventory)
            {
                if (GameManager.Instance.CurrentMenuState == MenuState.OnInventoryItemsMenu)
                {
                    DisableInventoryItemMenus();
                    ResetInventoryItemsColor();
                    addTriggerEvent.InventoryItemButtons.SetActive(true);
                    GameManager.Instance.CurrentMenuState = MenuState.OnInventoryMenu;
                    return;
                }

                if (GameManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu && isInventoryOpen)
                {
                    inventory.SetActive(false);
                    isInventoryOpen = false;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.CloseInventoryAudioSource);
                    GameManager.Instance.CurrentMenuState = MenuState.None;
                }
                else
                {
                    inventory.SetActive(true);
                    isInventoryOpen = true;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.OpenInventoryAudioSource);
                    GameManager.Instance.CurrentMenuState = MenuState.OnInventoryMenu;
                }
                StartCoroutine(InventoryInputDelay());
            }
        }
    }

    public void ResetInventoryItemsColor()
    {
        foreach (GameObject item in inventoryItems)
        {
            item.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void DisableInventoryItemMenus()
    {
        foreach (GameObject item in inventoryItemMenus)
        {
            item.SetActive(false);
        }
    }

    public IEnumerator InventoryInputDelay()
    {
        canOpenInventory = false;
        yield return new WaitForSecondsRealtime(inventoryInputDelay);
        canOpenInventory = true;
    }
}
