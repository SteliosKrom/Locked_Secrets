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

    #region PARTICLES
    [Header("PARTICLES")]
    [SerializeField] private ParticleSystem rainEffect;
    #endregion

    private void Start()
    {
        rainEffect.Play();
        AudioManager.Instance.PlayMenuMusic();
        AudioManager.Instance.PlayRainAudio();
        AudioManager.Instance.RainAudioLowPassFilter.cutoffFrequency = 2250f;
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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.PressAnyKeyAudioSource);
            StartCoroutine(EnterMainMenu());
        }
    }

    public IEnumerator EnterMainMenu()
    {
        yield return new WaitForSeconds(delay);
        mainMenu.SetActive(true);
    }
}
