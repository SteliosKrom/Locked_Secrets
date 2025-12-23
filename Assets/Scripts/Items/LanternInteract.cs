using UnityEngine;

public class LanternInteract : MonoBehaviour, IInteractable 
{
    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject worldLantern;
    [SerializeField] private GameObject playerLantern;
    [SerializeField] private GameObject lanternIcon;
    #endregion

    public void Interact()
    {
        worldLantern.SetActive(false);
        playerLantern.SetActive(true);
        lanternIcon.SetActive(true);
    }
}
