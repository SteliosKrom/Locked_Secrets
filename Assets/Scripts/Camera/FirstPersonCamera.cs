using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    #region CAMERA
    [Header("FIRST-PERSON CAMERA")]
    private float xRotation = 0f;
    #endregion

    #region UI
    [Header("SLIDERS")]
    [SerializeField] private Slider sensitivitySlider;

    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI sensitivitySliderValue;
    #endregion

    public Slider SensitivitySlider { get { return sensitivitySlider; } set { sensitivitySlider = value; } }

    private void LateUpdate()
    {
        CameraInput();
    }

    public void CameraInput()
    {
        if (GameManager.Instance.CanInteract()) return;

        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivitySlider.value;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivitySlider.value;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }

    public void OnSensitivityChanged()
    {
        float sensitivity = sensitivitySlider.value;
        sensitivitySliderValue.text = sensitivity.ToString("0%");
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();
    }
}
