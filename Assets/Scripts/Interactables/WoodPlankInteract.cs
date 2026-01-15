using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class WoodPlankInteract : MonoBehaviour, IInteractable
{
    private float unequipDelay = 2f;
    private float chopDelay = 2f;

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
    private Collider woodPlankCollider;
    #endregion

    private void Start()
    {
        mainDoorCollider = GameObject.Find("MainDoorWall").GetComponent<Collider>();
        woodPlankCollider = GetComponent<Collider>();
    }

    public void Interact()
    {
        if (GameManager.Instance.CurrentGameState != GameState.OnPlaying) return;

        if (GameManager.Instance.CurrentItemState == ItemState.Axe)
        {
            StartCoroutine(AxeChopDelay());
        }
    }

    public IEnumerator AxeChopDelay()
    {
        axeInteract.IsCoroutineRunning = true;

        GameManager.Instance.CurrentPlayerState = PlayerState.OnChopping;
        axeInteract.BaseAxeAnimator.SetTrigger("Chop");

        AudioManager.Instance.CutWoodPlank.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CutWoodPlank.source, AudioManager.Instance.CutWoodPlank.clip);

        yield return new WaitForSeconds(chopDelay);

        woodPlank.enabled = false;
        woodPlankCollider.enabled = false;
        planksLeft--;
        Debug.Log("Planks left: " + planksLeft);

        if (planksLeft == 0)
        {
            mainDoorCollider.enabled = false;
            StartCoroutine(UnequipDelay());
        }

        GameManager.Instance.CurrentPlayerState = PlayerState.OnIdle;

        axeInteract.IsCoroutineRunning = false;
    }

    public IEnumerator UnequipDelay()
    {
        axeInteract.BaseAxeAnimator.SetTrigger("Unequip");
        yield return new WaitForSeconds(unequipDelay);
        axeInteract.PlayerAxe.SetActive(false);
    }
}
