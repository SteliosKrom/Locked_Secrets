using System.Collections;
using UnityEngine;

public class NoteInteract : MonoBehaviour, IInteractable
{
    private static bool interacted = false;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject noteModel;
    [SerializeField] private GameObject noteCanvas;
    #endregion

    public GameObject NoteModel => noteModel;
    public GameObject NoteCanvas => noteCanvas;
    public bool IsInteracted { get => interacted; set => interacted = value; }

    public void Interact()
    {
        OnNoteInteracted(this);
    }

    public void OnNoteInteracted(NoteInteract item)
    {
        noteModel.SetActive(false);
        noteCanvas.SetActive(true);

        mainGameUIManager.ControlsTutorialPanel.SetActive(false);
        mainGameUIManager.IsControlsTutorialPanelOpen = false;

        AudioManager.Instance.Letter.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Letter.source, AudioManager.Instance.Letter.clip);

        MainGameUIManager.Instance.SetCurrentNote(this);
        GameManager.Instance.CurrentMenuState = MenuState.OnNoteMenu;
    }
}
