using NUnit.Framework.Internal.Filters;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum PuzzleRole { Chair, Lamp, Book, Radio }
public enum KeypadButtonRoles { None, Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Enter }

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [SerializeField] private int currentPuzzleStep = 0;

    [SerializeField] private bool hasMistake = false;

    private float firstPuzzleRepeatDelay = 3f;
    private float keypadPuzzleRepeatDelay = 3f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUIManager;
    [SerializeField] private MainDoorInteract mainDoorInteract;
    #endregion

    #region PUZZLE ITEMS
    [Header("PUZZLE ITEMS")]
    private FirstPuzzleItemInteract[] firstPuzzleItems;
    [SerializeField] private KeypadPuzzleButtonInteract[] keypadPuzzleButtons;

    private KeypadButtonRoles[] correctButtonSequence = new KeypadButtonRoles[]
    {
        KeypadButtonRoles.One,
        KeypadButtonRoles.Four,
        KeypadButtonRoles.Eight,
        KeypadButtonRoles.Nine
    };

    private PuzzleRole[] correctItemSequence = new PuzzleRole[]
    {
        PuzzleRole.Chair,
        PuzzleRole.Lamp,
        PuzzleRole.Book,
        PuzzleRole.Radio
    };
    #endregion

    #region UI
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI keypadDisplayText;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject keyIcon;
    [SerializeField] private GameObject keypadNumbers;
    [SerializeField] private GameObject mainDoorHandle;
    #endregion

    #region COLLIDERS
    [Header("BOX COLLIDERS")]
    private BoxCollider[] firstPuzzleItemColliders;
    private BoxCollider[] keypadButtonColliders;
    #endregion

    public TextMeshProUGUI KeypadDisplayText { get => keypadDisplayText; set => keypadDisplayText = value; }
    public bool HasMistake { get => hasMistake; set => hasMistake = value; }

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
        keypadButtonColliders = GameObject.Find("Buttons").GetComponentsInChildren<BoxCollider>();
        firstPuzzleItemColliders = GameObject.Find("Items").GetComponentsInChildren<BoxCollider>();
        firstPuzzleItems = GameObject.Find("Items").GetComponentsInChildren<FirstPuzzleItemInteract>();
    }

    public void OnFirstPuzzleItemInteracted(FirstPuzzleItemInteract item)
    {
        if (item.PuzzleRole != correctItemSequence[currentPuzzleStep])
        {
            hasMistake = true;
        }

        item.GetComponent<Collider>().enabled = false;
        item.GetComponent<Outline>().enabled = false;
        currentPuzzleStep++;

        if (currentPuzzleStep >= correctItemSequence.Length)
        {
            if (hasMistake)
                FirstPuzzleFailed();
            else
                FirstPuzzleSolved();
        }
    }

    public void OnKeypadPuzzleButtonInteracted(KeypadPuzzleButtonInteract button)
    {
        if (currentPuzzleStep >= correctButtonSequence.Length && button.KeypadPuzzleButtonRoles != KeypadButtonRoles.Enter)
            return;

        if (button.KeypadPuzzleButtonRoles != KeypadButtonRoles.Enter)
        {
            if (button.KeypadPuzzleButtonRoles != correctButtonSequence[currentPuzzleStep])
            {
                hasMistake = true;
            }
            currentPuzzleStep++;
        }

        switch (button.KeypadPuzzleButtonRoles)
        {
            case KeypadButtonRoles.Zero: KeypadDisplayText.text += "0"; break;
            case KeypadButtonRoles.One: KeypadDisplayText.text += "1"; break;
            case KeypadButtonRoles.Two: KeypadDisplayText.text += "2"; break;
            case KeypadButtonRoles.Three: KeypadDisplayText.text += "3"; break;
            case KeypadButtonRoles.Four: KeypadDisplayText.text += "4"; break;
            case KeypadButtonRoles.Five: KeypadDisplayText.text += "5"; break;
            case KeypadButtonRoles.Six: KeypadDisplayText.text += "6"; break;
            case KeypadButtonRoles.Seven: KeypadDisplayText.text += "7"; break;
            case KeypadButtonRoles.Eight: KeypadDisplayText.text += "8"; break;
            case KeypadButtonRoles.Nine: KeypadDisplayText.text += "9"; break;
        }

        if (button.KeypadPuzzleButtonRoles == KeypadButtonRoles.Enter && currentPuzzleStep == correctButtonSequence.Length)
        {
            if (hasMistake)
            {
                KeypadPuzzleFailed();
            }
            else
            {
                KeypadPuzzleSolved();
            }
        }

        if (button.KeypadPuzzleButtonRoles == KeypadButtonRoles.Enter && currentPuzzleStep > 0 && currentPuzzleStep <= 3)
        {
            KeypadPuzzleFailed();
        }
    }

    public void FirstPuzzleSolved()
    {
        GameManager.Instance.CurrentItemMenuState = ItemMenuState.OnRoomKeyMenu;
        GameManager.Instance.CurrentItemState = ItemState.Key;
        mainGameUIManager.GotRoomKeyPanel.SetActive(true);
        keyIcon.SetActive(true);
        keypadNumbers.SetActive(true);
        ResetSequencePuzzle();
    }

    public void FirstPuzzleFailed()
    {
        StartCoroutine(FirstPuzzleRepeatDelay());
    }

    public void KeypadPuzzleSolved()
    {
        Debug.Log($"Correct step finished with {currentPuzzleStep}. You solved the puzzle! Try again!");
        mainDoorInteract.CurrentDoorState = DoorState.Idle;
        EraseKeypadDisplayText();
        ResetSequencePuzzle();
        DisableKeypadButtonColliders();
        AudioManager.Instance.UnlockedDoor.source.transform.position = mainDoorHandle.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.UnlockedDoor.source, AudioManager.Instance.UnlockedDoor.clip);
    }

    public void KeypadPuzzleFailed()
    {
        Debug.Log($"Correct step finished with {currentPuzzleStep}. You failed the puzzle!");
        StartCoroutine(KeypadPuzzleRepeatDelay());
        AudioManager.Instance.PlaySFX(AudioManager.Instance.KeypadFailed.source, AudioManager.Instance.KeypadFailed.clip);
    }

    public void EraseKeypadDisplayText()
    {
        keypadDisplayText.text = "";
    }

    public void ResetSequencePuzzle()
    {
        currentPuzzleStep = 0;
        hasMistake = false;
    }

    public void EnableFirstPuzzleItemColliders()
    {
        foreach (BoxCollider collider in firstPuzzleItemColliders)
        {
            collider.enabled = true;
        }
    }

    public void EnableKeypadPuzzleButtonColliders()
    {
        foreach (BoxCollider collider in keypadButtonColliders)
        {
            collider.enabled = true;
        }
    }

    public void DisableKeypadButtonColliders()
    {
        foreach (BoxCollider collider in keypadButtonColliders)
        {
            collider.enabled = false;
        }
    }

    public IEnumerator FirstPuzzleRepeatDelay()
    {
        yield return new WaitForSeconds(firstPuzzleRepeatDelay);
        ResetSequencePuzzle();
        EnableFirstPuzzleItemColliders();
        OutlineEffect.Instance.EnableFirstPuzzleItemsOutlineEffect();
    }

    public IEnumerator KeypadPuzzleRepeatDelay()
    {
        DisableKeypadButtonColliders();
        EraseKeypadDisplayText();

        yield return new WaitForSeconds(keypadPuzzleRepeatDelay);

        ResetSequencePuzzle();
        EnableKeypadPuzzleButtonColliders();
    }
}
