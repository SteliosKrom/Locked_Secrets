using System.Collections;
using UnityEngine;

public class LightingEffect : MonoBehaviour
{
    private float minInterval = 5f; // 40
    private float maxInterval = 10f; //60
    private float effectDuration = 0;

    #region LIGHTING
    [Header("LIGHTING")]
    private Light[] windowLights;
    #endregion

    #region PARTICLES
    [Header("PARTICLES")]
    private ParticleSystem[] lightingEffects;
    #endregion

    private void Awake()
    {
        windowLights = GameObject.Find("WindowLights").GetComponentsInChildren<Light>();
        lightingEffects = GameObject.Find("LightingEffects").GetComponentsInChildren<ParticleSystem>();
    }

    public void Start()
    {
        StartCoroutine(LightingEffectDelay());
    }

    public void EnableLighting()
    {
        StartCoroutine(LightningFlashSequence());

        foreach (ParticleSystem particle in lightingEffects)
        {
            float lifetime = particle.main.startLifetime.constant;

            particle.Play();

            if (lifetime >= effectDuration)
                effectDuration = lifetime;
        }
    }

    private IEnumerator LightningFlashSequence()
    {
        foreach (Light light in windowLights) { light.enabled = true; light.intensity = 15f; }
        yield return new WaitForSeconds(0.05f);

        foreach (Light light in windowLights) { light.intensity = 0f; }
        yield return new WaitForSeconds(0.02f);

        foreach (Light light in windowLights) { light.intensity = 40f; }
        yield return new WaitForSeconds(0.15f);

        foreach (Light light in windowLights) { light.enabled = false; }
    }

    public void DisableLighting()
    {
        foreach (Light light in windowLights) light.enabled = false;
        foreach (ParticleSystem particle in lightingEffects) particle.Stop();
    }

    public IEnumerator LightingEffectDelay()
    {
        while (true)
        {
            if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
            {
                DisableLighting();

                float startDelay = Random.Range(minInterval, maxInterval);
                yield return new WaitForSeconds(startDelay);

                EnableLighting();

                yield return new WaitForSeconds(effectDuration + 0.2f);
                DisableLighting();
            }
            else
            {
                yield return null;
            }
        }
    }
}
