using TMPro;
using UnityEngine;

public class FPSCount : MonoBehaviour
{
    private float timer;
    private float updateInterval = 1f;
    private float fps;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private SettingsManager settingsManager;
    #endregion

    #region UI
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI fpsCountText;
    #endregion

    public float FPS => fps;
    public TextMeshProUGUI FpsCountText { get => fpsCountText; set => fpsCountText = value; }

    void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnLoading)
        {
            fpsCountText.enabled = false;
            return;
        }

        if (settingsManager.ShowFPSToggle.isOn)
            fpsCountText.enabled = true;
        else
            fpsCountText.enabled = false;

        timer += Time.unscaledDeltaTime;

        if (timer >= updateInterval)
        {
            fps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
            fpsCountText.text = fps > 999 ? "FPS: 999+" : "FPS: " + fps.ToString();
            timer = 0f;
        }
    }
}
