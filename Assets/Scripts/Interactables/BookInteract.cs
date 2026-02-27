using System.Collections;
using UnityEngine;

public class BookInteract : MonoBehaviour, IInteractable
{
    private float openLanternLightDuration = 1f;

    private bool footstepsStarted = false;
    [SerializeField] private bool firstTimeInteracted;

    [SerializeField] private Transform bathroomDoorPoint;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private LanternLightFlickering lanternLightFlickering;
    #endregion

    #region LIGHTING
    [Header("LIGHTING")]
    [SerializeField] private Light pointLightEye;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider bookCollider;
    [SerializeField] private Collider crossCollider;
    [SerializeField] private Collider bathroomDoorHandleCollider;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator bathroomDoorAnimator;
    [SerializeField] private Animator monsterAnimator;
    #endregion

    private void Update()
    {
        if (footstepsStarted)
        {
            Vector3 targetPoint = Vector3.MoveTowards(AudioManager.Instance.FootSteps.source.transform.position, bathroomDoorPoint.transform.position, 0.8f * Time.deltaTime);
            AudioManager.Instance.FootSteps.source.transform.position = targetPoint;
        }
    }

    public void Interact()
    {
        switch (firstTimeInteracted)
        {
            case true:
                StartCoroutine(DemonDelay());
                break;
            case false:
                StartCoroutine(LanternFlickeringDelay());
                break;
        }
        firstTimeInteracted = false;
        bookCollider.enabled = false;
        bathroomDoorHandleCollider.enabled = true;
        pointLightEye.enabled = false;
    }

    public IEnumerator DemonDelay()
    {
        GameManager.Instance.CurrentBathroomDoorState = BathroomDoorState.Locked;

        bathroomDoorAnimator.SetTrigger("Close");
        AudioManager.Instance.CloseDoor.source.transform.position = bathroomDoorAnimator.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CloseDoor.source, AudioManager.Instance.CloseDoor.clip);
        yield return new WaitForSeconds(AudioManager.Instance.CloseDoor.clip.length + 3f);

        footstepsStarted = true;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.FootSteps.source, AudioManager.Instance.FootSteps.clip);
        yield return new WaitForSeconds(AudioManager.Instance.FootSteps.clip.length + 3f);

        footstepsStarted = false;
        AudioManager.Instance.DoorKnock.source.volume = 0.5f;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.DoorKnock.source, AudioManager.Instance.DoorKnock.clip);
        yield return new WaitForSeconds(AudioManager.Instance.DoorKnock.clip.length + 3f);

        AudioManager.Instance.DoorKnock.source.volume = 1f;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.DoorKnock.source, AudioManager.Instance.DoorKnock.clip);
        yield return new WaitForSeconds(AudioManager.Instance.DoorKnock.clip.length + 5f);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.Scream.source, AudioManager.Instance.Scream.clip);
        yield return new WaitForSeconds(AudioManager.Instance.Scream.clip.length + 3f);

        pointLightEye.enabled = true;
        bookCollider.enabled = true;
    }

    public IEnumerator LanternFlickeringDelay()
    {
        lanternLightFlickering.LanternLightParticle.SetActive(true);
        AudioManager.Instance.LanternLightFlicker.source.Play();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.InstantJumpscare.source, AudioManager.Instance.InstantJumpscare.clip);

        ParticleSystem.EmissionModule emission = lanternLightFlickering.LanternLightParticleEffect.emission;
        float elapsedTime = 0;

        while (elapsedTime < openLanternLightDuration)
        {
            AudioManager.Instance.LanternLightFlicker.source.volume = Random.Range(lanternLightFlickering.MinLanternLightVolume, lanternLightFlickering.MaxLanternLightVolume);
            lanternLightFlickering.LanternLight.intensity = Random.Range(lanternLightFlickering.MinFlickerIntensity, lanternLightFlickering.MaxFlickerIntensity);
            emission.rateOverTime = Random.Range(lanternLightFlickering.MinLanternEmissionRate, lanternLightFlickering.MaxLanternEmissionRate);

            monsterAnimator.SetTrigger("Down");

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lanternLightFlickering.LanternLight.intensity = 2f;
        AudioManager.Instance.LanternLightFlicker.source.volume = 0.3f;
        emission.rateOverTime = 50f;

        crossCollider.enabled = true;
    }
}
