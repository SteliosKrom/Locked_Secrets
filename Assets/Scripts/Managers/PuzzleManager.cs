using NUnit.Framework.Internal.Filters;
using System.Collections;
using TMPro;
using UnityEngine;

public enum PuzzleRole { Chair, Lamp, Book, Radio }

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [SerializeField] private int currentStep = 0;

    [SerializeField] private bool hasMistake = false;

    private float firstPuzzleRepeatDelay = 3f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region PUZZLE ITEMS
    [Header("PUZZLE ITEMS")]
    [SerializeField] private FirstPuzzleItemInteract[] firstPuzzleItems;
    private PuzzleRole[] correctSequence = new PuzzleRole[]
    {
        PuzzleRole.Chair,
        PuzzleRole.Lamp,
        PuzzleRole.Book,
        PuzzleRole.Radio
    };
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject keyIcon;
    #endregion

    #region COLLIDERS
    [Header("BOX COLLIDERS")]
    [SerializeField] private BoxCollider[] firstPuzzleItemColliders;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnFirstPuzzleItemInteracted(FirstPuzzleItemInteract item)
    {
        if (item.PuzzleRole != correctSequence[currentStep])
        {
            hasMistake = true;
        }

        item.GetComponent<Collider>().enabled = false;
        currentStep++;

        if (currentStep >= correctSequence.Length)
        {
            if (hasMistake)
                PuzzleFailed();
            else
                PuzzleSolved();
        }
    }

    public void PuzzleSolved()
    {
        Debug.Log($"Correct step finished with {currentStep}. You solved the puzzle!");
        GameManager.Instance.CurrentMenuState = MenuState.OnRoomKeyMenu;
        GameManager.Instance.CurrentItemState = ItemState.Key;
        mainGameUIManager.GotRoomKeyPanel.SetActive(true);
        keyIcon.SetActive(true);
    }

    public void PuzzleFailed()
    {
        Debug.Log($"Correct step finished with {currentStep}. You didn't solve the puzzle! Try again!");
        StartCoroutine(FirstPuzzleRepeatDelay());
    }

    public void ResetSequencePuzzle()
    {
        currentStep = 0;
        hasMistake = false;
    }

    public void EnablePuzzleItemColliders()
    {
        foreach (BoxCollider collider in firstPuzzleItemColliders)
        {
            collider.enabled = true;
        }
    }

    public IEnumerator FirstPuzzleRepeatDelay()
    {
        yield return new WaitForSecondsRealtime(firstPuzzleRepeatDelay);
        ResetSequencePuzzle();
        EnablePuzzleItemColliders();
    }
}
