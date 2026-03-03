using System.Collections;
using UnityEngine;

public class LightingEffect : MonoBehaviour
{
    private float minInterval = 40f;
    private float maxInterval = 60f;
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
        foreach (Light light in windowLights) light.enabled = true;

        foreach (ParticleSystem particle in lightingEffects)
        {
            float lifetime = particle.main.startLifetime.constant;

            particle.Play();

            if (lifetime >= effectDuration)
                effectDuration = lifetime;
        }
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

                yield return new WaitForSeconds(effectDuration);
                DisableLighting();
            }
            else
            {
                yield return null;
            }
        }
    }
}
