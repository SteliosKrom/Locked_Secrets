using NUnit.Framework.Internal.Filters;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PuzzleRole { Chair, Lamp, Book, Radio }
public enum KeypadButtonRoles { None, Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Enter }

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [SerializeField] private int currentPuzzleStep = 0;

    [SerializeField] private bool hasMistake = false;
    [SerializeField] private bool keypadPuzzleActive;

    private float firstPuzzleRepeatDelay = 1f;
    private float keypadPuzzleRepeatDelay = 0.75f;

    private float flickerDuration = 3f;
    private float minPlayerLanternIntensity = 0f;
    private float maxPlayerLanternIntensity = 2f;

    private float minTableLanternIntensity = 0f;
    private float maxTableLanternIntensity = 0.5f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainDoorInteract mainDoorInteract;
    #endregion

    #region PUZZLE ITEMS
    [Header("PUZZLE ITEMS")]
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
    [SerializeField] private GameObject keypadNumbers;
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject mainDoorHandle;
    #endregion

    #region COLLIDERS
    [Header("BOX COLLIDERS")]
    private BoxCollider[] firstPuzzleItemColliders;
    private BoxCollider[] keypadButtonColliders;
    #endregion

    #region LIGHTING
    [Header("LIGHTING")]
    [SerializeField] private Light playerLanternLight;
    [SerializeField] private Light tableLanternLight;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator smallCrateDoorAnimator;
    #endregion

    public GameObject Key => key;
    public TextMeshProUGUI KeypadDisplayText { get => keypadDisplayText; set => keypadDisplayText = value; }
    public bool HasMistake { get => hasMistake; set => hasMistake = value; }
    public bool KeypadPuzzleActive { get => keypadPuzzleActive; set => keypadPuzzleActive = value; }

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
    }

    private void Start()
    {
        keypadPuzzleActive = true;
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
        smallCrateDoorAnimator.SetTrigger("Open");
        AudioManager.Instance.PlaySFX(AudioManager.Instance.UnlockSmallCrateDoor.source, AudioManager.Instance.UnlockSmallCrateDoor.clip);

        key.SetActive(true);
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
        keypadPuzzleActive = false;
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
        keypadPuzzleActive = true;
        StartCoroutine(KeypadPuzzleRepeatDelay());
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

    public void EnableFirstPuzzleObjectColliders()
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
        float elapsedTime = 0f;

        while (elapsedTime < flickerDuration)
        {
            playerLanternLight.intensity = Random.Range(minPlayerLanternIntensity, maxPlayerLanternIntensity);
            tableLanternLight.intensity = Random.Range(minTableLanternIntensity, maxTableLanternIntensity);
            elapsedTime += 0.05f;

            yield return new WaitForSeconds(0.05f);
        }
        playerLanternLight.intensity = 2f;
        tableLanternLight.intensity = 0.5f;

        yield return new WaitForSeconds(firstPuzzleRepeatDelay);
        ResetSequencePuzzle();
        EnableFirstPuzzleObjectColliders();
    }

    public IEnumerator KeypadPuzzleRepeatDelay()
    {
        DisableKeypadButtonColliders();
        EraseKeypadDisplayText();

        yield return new WaitForSeconds(keypadPuzzleRepeatDelay);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.KeypadFailed.source, AudioManager.Instance.KeypadFailed.clip);

        ResetSequencePuzzle();
        EnableKeypadPuzzleButtonColliders();
    }
}
