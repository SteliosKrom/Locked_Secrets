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
        noteModel.SetActive(false);
        noteCanvas.SetActive(true);

        AudioManager.Instance.LetterAudioSource.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LetterAudioSource, AudioManager.Instance.LetterAudioClip);

        if (!interacted)
        {
            PuzzleManager.Instance.EnableFirstPuzzleItemColliders();
            interacted = true;
        }

        MainGameUIManager.Instance.SetCurrentNote(this);
        GameManager.Instance.CurrentMenuState = MenuState.OnNoteMenu;
    }
}
