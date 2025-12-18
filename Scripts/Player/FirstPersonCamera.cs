 using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    #region CAMERA
    [Header("FIRST-PERSON CAMERA")]
    private float mouseSens = 50f;
    private float xRotation = 0f;
    #endregion

    private void Start()
    {
        xRotation = transform.localRotation.eulerAngles.x;

        if (xRotation > 180f)
            xRotation -= 360f;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            CameraInput();
        }
    }

    public void CameraInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
