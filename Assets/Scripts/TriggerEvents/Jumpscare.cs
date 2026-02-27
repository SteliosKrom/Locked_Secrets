using System;
using System.Collections;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private Transform priestHead;
    [SerializeField] private Transform playerCamera;

    private float jumpscareDelay = 10f;

    [SerializeField] private bool jumpscareTriggerExecutedOnce = false;

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
        if (!jumpscareTriggerExecutedOnce)
        {
            if (priestAnimator.applyRootMotion)
            {
                priestHead.LookAt(playerCamera);
                AudioManager.Instance.MainGameAudioSource.volume = Mathf.Lerp(AudioManager.Instance.MainGameAudioSource.volume, 0f, 0.5f * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            priestAnimator.applyRootMotion = true;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.HorrorSound.source, AudioManager.Instance.HorrorSound.clip);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.BreathingSound.source, AudioManager.Instance.BreathingSound.clip);
            jumpscareTriggerZone.enabled = false;
            StartCoroutine(Delay());
        }
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(jumpscareDelay);

        priestAnimator.applyRootMotion = false;

        float duration = 2f;
        float t = 0f;
        float startVolume = AudioManager.Instance.MainGameAudioSource.volume;

        while (t < duration)
        {
            t += Time.deltaTime;
            AudioManager.Instance.MainGameAudioSource.volume = Mathf.Lerp(startVolume, 1f, t / duration);
            yield return null;
        }

        AudioManager.Instance.MainGameAudioSource.volume = 1f;

        jumpscareTriggerExecutedOnce = true;
        priestAnimator.gameObject.SetActive(false);
    }
}
