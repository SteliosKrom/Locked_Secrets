using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private bool isInventoryOpen = false;
    [SerializeField] private bool canOpenInventory = true;

    private float inventoryInputDelay = 1f;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject inventory;
    #endregion

    private void Update()
    {
        InventoryInput();
    }

    public void InventoryInput()
    {
        if (GameManager.Instance.CurrentMenuState == MenuState.OnNoteMenu) return;
        if (GameManager.Instance.CurrentGameState == GameState.OnEnding) return;
        if (GameManager.Instance.CanItemMenuInteract()) return;

        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (Input.GetKeyDown(KeyCode.I) && canOpenInventory)
            {
                if (isInventoryOpen)
                {
                    inventory.SetActive(false);
                    isInventoryOpen = false;
                    GameManager.Instance.CurrentMenuState = MenuState.None;
                }
                else
                {
                    inventory.SetActive(true);
                    isInventoryOpen = true;
                    GameManager.Instance.CurrentMenuState = MenuState.OnInventoryMenu;
                }
                StartCoroutine(InventoryInputDelay());
            }
        }
    }

    public IEnumerator InventoryInputDelay()
    {
        canOpenInventory = false;
        yield return new WaitForSecondsRealtime(inventoryInputDelay);
        canOpenInventory = true;
    }
}
