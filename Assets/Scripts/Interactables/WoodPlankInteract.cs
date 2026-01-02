using System.Data.Common;
using UnityEngine;

public class WoodPlankInteract : MonoBehaviour, IInteractable
{
    private static int planksLeft = 3;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject woodPlank;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    private Collider mainDoorCollider;
    #endregion

    private void Start()
    {
        mainDoorCollider = GameObject.Find("MainDoorWall").GetComponent<Collider>();
    }

    public void Interact()
    {
        if (GameManager.Instance.CurrentItemState == ItemState.Axe)
        {
            woodPlank.SetActive(false);
            planksLeft--;
            Debug.Log("Planks left: " + planksLeft);

            if (planksLeft == 0)
            {
                mainDoorCollider.enabled = false;
            }
        }
    }
}
