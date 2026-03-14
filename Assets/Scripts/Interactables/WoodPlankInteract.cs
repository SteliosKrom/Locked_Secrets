using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlankInteract : MonoBehaviour, IInteractable
{
    private static List<Collider> woodPlankColliders = new List<Collider>();

    private float unequipDelay = 2f;
    private float chopDelay = 2f;
    private float planksInformTextDelay = 2f;

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

    private void Awake()
    {
        mainDoorCollider = GameObject.Find("MainDoorWall").GetComponent<Collider>();
        woodPlankCollider = GetComponent<Collider>();

        if (!woodPlankColliders.Contains(woodPlankCollider))
        {
            woodPlankColliders.Add(woodPlankCollider);
        }
    }

    private void OnDestroy()
    {
        woodPlankColliders.Remove(woodPlankCollider);
    }

    public void Interact()
    {
        if (GameManager.Instance.CurrentGameState != GameState.OnPlaying) return;
        if (GameManager.Instance.CurrentPlayerState == PlayerState.OnChopping) return;

        if (GameManager.Instance.CurrentItemState == ItemState.Axe)
        {
            StartCoroutine(AxeChopDelay());
        }
        else
        {
            StartCoroutine(ShowPlanksInformTextDelay());
        }
    }

    public IEnumerator AxeChopDelay()
    {
        axeInteract.IsCoroutineRunning = true;

        GameManager.Instance.CurrentPlayerState = PlayerState.OnChopping;
        axeInteract.BaseAxeAnimator.SetTrigger("Chop");

        AudioManager.Instance.CutWoodPlank.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CutWoodPlank);

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

    public IEnumerator ShowPlanksInformTextDelay()
    {
        foreach (Collider collider in woodPlankColliders) collider.enabled = false;

        axeInteract.PlanksInformText.SetActive(true);
        yield return new WaitForSeconds(planksInformTextDelay);
        axeInteract.PlanksInformText.SetActive(false);
        woodPlankCollider.enabled = true;

        foreach (Collider collider in woodPlankColliders) collider.enabled = true;
    }
}
