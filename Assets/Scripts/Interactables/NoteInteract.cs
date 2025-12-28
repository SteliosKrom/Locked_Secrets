using UnityEngine;

public class NoteInteract : MonoBehaviour, IInteractable
{
    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject note;
    [SerializeField] private GameObject noteCanvas;
    #endregion

    public GameObject Note { get { return note; } }
    public GameObject NoteCanvas { get { return noteCanvas; } }

    public void Interact()
    {
        note.SetActive(false);
        noteCanvas.SetActive(true);

        PuzzleManager.Instance.EnablePuzzleItemColliders();

        GameManager.Instance.CurrentMenuState = MenuState.OnNoteMenu;
    }    
}
