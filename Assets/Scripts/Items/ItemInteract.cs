using System.Collections;
using UnityEngine;

public class ItemInteract : MonoBehaviour, IInteractable
{
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
    }
}
