using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    [SerializeField] private Light candleLight;
    [SerializeField] private float minIntensity = 1.8f;
    [SerializeField] private float maxIntensity = 2.5f;
    [SerializeField] private float flickerSpeed = 0.1f;

    void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPaused) return;
        candleLight.intensity = Mathf.Lerp(candleLight.intensity, Random.Range(minIntensity, maxIntensity), flickerSpeed);
    }
}
