using System.Collections;
using UnityEngine;

public class NoteInteract : MonoBehaviour, IInteractable
{
    private static bool interacted = false;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject noteModel;
    [SerializeField] private GameObject noteCanvas;
    #endregion

    public GameObject NoteModel => noteModel;
    public GameObject NoteCanvas => noteCanvas;

    public void Interact()
    {
        OnNoteInteracted(this);
    }

    public void OnNoteInteracted(NoteInteract item)
    {
        AudioManager audioManager = AudioManager.Instance;

        noteModel.SetActive(false);
        noteCanvas.SetActive(true);

        audioManager.PlaySFX(audioManager.LetterAudioSource, audioManager.LetterAudioClip);

        if (!interacted)
        {
            PuzzleManager.Instance.EnableFirstPuzzleItemColliders();
            interacted = true;
        }

        MainGameUIManager.Instance.SetCurrentNote(this);
        GameManager.Instance.CurrentMenuState = MenuState.OnNoteMenu;
    }
}
