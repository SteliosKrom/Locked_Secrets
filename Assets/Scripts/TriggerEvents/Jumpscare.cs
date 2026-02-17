using System;
using System.Collections;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private Animator priestAnimator;
    [SerializeField] private BoxCollider jumpscareTriggerZone;

    [SerializeField] private AudioSource jumpscareAudioSource;
    [SerializeField] private AudioClip jumpscareAudioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            priestAnimator.applyRootMotion = true;
            AudioManager.Instance.PlaySFX(jumpscareAudioSource, jumpscareAudioClip);
            jumpscareTriggerZone.enabled = false;
            StartCoroutine(Delay());
        }
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(10f);
        priestAnimator.applyRootMotion = false;
        priestAnimator.gameObject.SetActive(false);
    }
}
