using System;
using System.Collections;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private Transform priestHead;
    [SerializeField] private Transform playerCamera;

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator priestAnimator;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private BoxCollider jumpscareTriggerZone;
    #endregion

    private void LateUpdate()
    {
        if (priestAnimator.applyRootMotion)
        {
            priestHead.LookAt(playerCamera);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            priestAnimator.applyRootMotion = true;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.HorrorSound.source, AudioManager.Instance.HorrorSound.clip);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.BreathingSound.source, AudioManager.Instance.BreathingSound.clip);
            AudioManager.Instance.PauseMainGameMusic();
            jumpscareTriggerZone.enabled = false;
            StartCoroutine(Delay());
        }
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(10f);
        priestAnimator.applyRootMotion = false;
        priestAnimator.gameObject.SetActive(false);
        AudioManager.Instance.UnpauseMainGameMusic();
    }
}
