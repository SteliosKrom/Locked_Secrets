using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleMenuUIManager : MonoBehaviour
{
    private float delay = 3f;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject mainMenu;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATORS")]
    [SerializeField] private Animator titleMenuAnimator;
    [SerializeField] private Animator pressAnyKeyAnimator;
    #endregion

    #region AUDIO
    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource pressAnyKeyAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip pressAnyKeyAudioClip;
    #endregion

    private void Start()
    {
        AudioManager.Instance.PlayMenuMusic();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentMenuState == MenuState.OnTitleMenu)
        {
            PressAnyKeyToStart();
        }
    }

    public void PressAnyKeyToStart()
    {
        if (Input.anyKeyDown)
        {
            GameManager.Instance.CurrentMenuState = MenuState.OnMainMenu;
            titleMenuAnimator.SetBool("Out", true);
            pressAnyKeyAnimator.SetBool("Out", true);
            AudioManager.Instance.PlaySFX(pressAnyKeyAudioSource, pressAnyKeyAudioClip);
            StartCoroutine(EnterMainMenu());
        }
    }

    public IEnumerator EnterMainMenu()
    {
        yield return new WaitForSeconds(delay);
        mainMenu.SetActive(true);
    }
}
