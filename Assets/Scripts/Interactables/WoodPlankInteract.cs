using UnityEngine;

public class WoodPlankInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject woodPlank;

    public void Interact()
    {
        if (GameManager.Instance.CurrentItemState == ItemState.Axe)
        {
            woodPlank.SetActive(false);
        }
    }
}
