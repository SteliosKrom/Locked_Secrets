using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class WoodPlankInteract : MonoBehaviour, IInteractable
{
    private float unequipDelay = 2f;
    private static int planksLeft = 3;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private AxeInteract axeInteract;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private MeshRenderer woodPlank;
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
            woodPlank.enabled = false;
            planksLeft--;
            Debug.Log("Planks left: " + planksLeft);

            if (planksLeft == 0)
            {
                mainDoorCollider.enabled = false;
                StartCoroutine(UnequipDelay());
            }
        }
    }

    public IEnumerator UnequipDelay()
    {
        axeInteract.BaseAxeAnimator.SetTrigger("Unequip");
        yield return new WaitForSecondsRealtime(unequipDelay);
        axeInteract.PlayerAxe.SetActive(false);
    }
}
