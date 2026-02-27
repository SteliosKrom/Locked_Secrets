using System.Collections;
using UnityEngine;

public class LanternLightFlickering : MonoBehaviour
{
    private float flickerDuration = 2f;

    private float minFlickerIntensity = 0f;
    private float maxFlickerIntensity = 2f;

    private float minLanternEmissionRate = 0f;
    private float maxLanternEmissionRate = 50f;

    private float minLanternLightVolume = 0f;
    private float maxLanternLightVolume = 0.3f;

    private bool isFlickering = false;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject lanternLightParticle;
    #endregion

    #region LIGHTING
    [Header("LIGHTING")]
    [SerializeField] private Light lanternLight;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator otherDoorAnimator;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider lanternLightFlickeringTriggerCollider;
    [SerializeField] private Collider otherDoorHandleCollider;
    #endregion

    #region PARTICLES
    [Header("PARTICLES")]
    [SerializeField] private ParticleSystem lanternLightParticleEffect;
    #endregion

    public float MinLanternEmissionRate { get => minLanternEmissionRate; set => minLanternEmissionRate = value; }
    public float MaxLanternEmissionRate { get => maxLanternEmissionRate; set => maxLanternEmissionRate = value; }
    public float MinLanternLightVolume { get => minLanternLightVolume; set => minLanternLightVolume = value; }
    public float MaxLanternLightVolume { get => maxLanternLightVolume; set => maxLanternLightVolume = value; }
    public float MinFlickerIntensity { get => minFlickerIntensity; set => minFlickerIntensity = value; }
    public float MaxFlickerIntensity { get => maxFlickerIntensity; set => maxFlickerIntensity = value; }
    public GameObject LanternLightParticle { get => lanternLightParticle; }
    public ParticleSystem LanternLightParticleEffect { get => lanternLightParticleEffect; }
    public Light LanternLight { get => lanternLight; set => lanternLight = value; }

    private void Update()
    {
        if (isFlickering)
        {
            AudioManager.Instance.MainGameAudioSource.volume = Mathf.Lerp(AudioManager.Instance.MainGameAudioSource.volume, 0f, 0.5f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FlickerDelay());
        }
    }

    private IEnumerator FlickerDelay()
    {
        isFlickering = true;

        lanternLightFlickeringTriggerCollider.enabled = false;
        float elapsedTime = 0f;

        while (elapsedTime < flickerDuration)
        {
            lanternLight.intensity = Random.Range(minFlickerIntensity, maxFlickerIntensity);
            elapsedTime += 0.05f;

            yield return new WaitForSeconds(0.05f);
        }

        AudioManager.Instance.MainGameAudioSource.volume = 0f;
        AudioManager.Instance.LanternLightFlicker.source.Stop();

        lanternLight.intensity = 0f;
        lanternLightParticle.SetActive(false);

        otherDoorHandleCollider.enabled = false;
        otherDoorAnimator.SetTrigger("Close");
        AudioManager.Instance.CloseDoor.source.transform.position = otherDoorAnimator.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CloseDoor.source, AudioManager.Instance.CloseDoor.clip);

        isFlickering = false;
    }
}
