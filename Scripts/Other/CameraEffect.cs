using Unity.VisualScripting;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    [SerializeField] private Camera secondaryCamera;

    private Vector3 initialPos;

    private float movementSpeed = 1;
    private float movementRange = 0.025f;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        if (secondaryCamera.enabled)
        {
            float offsetX = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            float offsetY = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            float offsetZ = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            secondaryCamera.transform.position = initialPos + new Vector3(offsetX, offsetY, offsetZ);
        }
    }
}
