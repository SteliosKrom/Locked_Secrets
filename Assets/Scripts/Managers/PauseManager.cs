using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool canPause = true;
    private float pauseDelay = 1f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainGameUIManager mainGameUImanager;
    [SerializeField] private SmallRoomDoorInteract smallRoomDoorInteract;
    [SerializeField] private MainDoorInteract mainDoorInteract;
    [SerializeField] private DoorInteract doorInteract;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject dot;
    #endregion

    private void Update()
    {
        HandlePauseInput();
    }

    public void HandlePauseInput()
    {
        if (GameManager.Instance.CanInteract()) return;

        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            switch (GameManager.Instance.CurrentGameState)
            {
                case GameState.OnPlaying:
                    PauseGame();
                    break;
                case GameState.OnPaused:
                    CheckCurrentMenuState();
                    break;
            }
            StartCoroutine(PauseDelay());
        }
    }

    public void CheckCurrentMenuState()
    {
        switch (GameManager.Instance.CurrentMenuState)
        {
            case MenuState.OnCategorySettings:
                GameManager.Instance.CurrentMenuState = MenuState.OnGameSettings;

                SettingsUIManager.Instance.HideAllCatagories();
                SettingsUIManager.Instance.SettingsMenu.SetActive(true);
                SettingsUIManager.Instance.GetBackToPauseMenu.SetActive(true);
                SettingsUIManager.Instance.GetBackToSettingsFromGame.SetActive(false);
                break;
            case MenuState.OnGameSettings:
                GameManager.Instance.CurrentMenuState = MenuState.OnPausedMenu;

                SettingsUIManager.Instance.SettingsMenu.SetActive(false);
                SettingsUIManager.Instance.GetBackToPauseMenu.SetActive(false);
                mainGameUImanager.PauseMenu.SetActive(true);
                break;
            case MenuState.OnPausedMenu:
                UnpauseGame();
                break;
        }
        mainGameUImanager.DisableRedColorOnButtonText();
    }

    public void PauseGame()
    {
        GameManager.Instance.CurrentGameState = GameState.OnPaused;
        GameManager.Instance.CurrentMenuState = MenuState.OnPausedMenu;

        mainGameUImanager.PauseMenu.SetActive(true);
        smallRoomDoorInteract.ItsLockedText.SetActive(false);
        dot.SetActive(false);

       AudioManager.Instance.PauseMainGameMusic();

       AudioManager.Instance.PauseSFX(AudioManager.Instance.LockedAudioSource);
       AudioManager.Instance.PauseSFX(AudioManager.Instance.OpenDoorAudioSource);
       AudioManager.Instance.PauseSFX(AudioManager.Instance.CloseDoorAudioSource);
       AudioManager.Instance.PauseSFX(AudioManager.Instance.LetterAudioSource);

       AudioManager.Instance.PauseSFX(smallRoomDoorInteract.UnlockedAudioSource);

        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnpauseGame()
    {
        GameManager.Instance.CurrentGameState = GameState.OnPlaying;
        GameManager.Instance.CurrentMenuState = MenuState.None;

        mainGameUImanager.PauseMenu.SetActive(false);
        dot.SetActive(true);

        mainGameUImanager.DisableRedColorOnButtonText();

       AudioManager.Instance.UnpauseMainGameMusic();

       AudioManager.Instance.UnpauseSFX(AudioManager.Instance.LockedAudioSource);
       AudioManager.Instance.UnpauseSFX(AudioManager.Instance.OpenDoorAudioSource);
       AudioManager.Instance.UnpauseSFX(AudioManager.Instance.CloseDoorAudioSource);
       AudioManager.Instance.UnpauseSFX(AudioManager.Instance.LetterAudioSource);
       
       AudioManager.Instance.UnpauseSFX(smallRoomDoorInteract.UnlockedAudioSource);

        Time.timeScale = 1;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public IEnumerator PauseDelay()
    {
        canPause = false;
        yield return new WaitForSecondsRealtime(pauseDelay);
        canPause = true;
    }
}
