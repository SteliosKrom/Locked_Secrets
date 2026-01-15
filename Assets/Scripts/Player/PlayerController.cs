using Shapes2D;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerController : MonoBehaviour
{
    #region TRANSFORM
    [Header("TRANSFORM")]
    [SerializeField] private Transform playerCamera;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private AxeInteract axeInteract;
    #endregion

    #region PLAYER
    [Header("WALK")]
    [SerializeField] private CharacterController characterController;
    private Vector3 smoothMoveVelocity;
    private Vector3 velocity;

    private float horizontalInput;
    private float verticalInput;

    private float gravity = -9.81f;
    private float groundedVelocity = -1f;

    [Header("CROUCH")]
    private float standingCapsuleHeight = 2f;
    private float crouchCapsuleHeight = 1f;

    private float standingCameraHeight = 0.8f;
    private float crouchCameraHeight = -0.1f;

    private float walkSpeed = 2f;
    private float crouchWalkSpeed = 1f;

    private bool isCrouching;

    [Header("SMOOTH CROUCHING")]
    private float targetCapsuleHeight;
    private float currentCapsuleHeight;

    private float targetCameraHeight;
    private float currentCameraHeight;

    [SerializeField] private float heightSmoothSpeed;
    #endregion

    private void Start()
    {
        currentCapsuleHeight = standingCapsuleHeight;
        targetCapsuleHeight = standingCapsuleHeight;

        currentCameraHeight = standingCameraHeight;
        targetCameraHeight = standingCameraHeight;
    }

    private void Update()
    {
        HandlePlayerMovement();
        HandleCrouch();
        SmoothCrouch();
    }

    public void HandlePlayerMovement()
    {
        if (GameManager.Instance.CanInteract()) return;
        if (GameManager.Instance.CurrentGameState != GameState.OnPlaying) return;
        if (axeInteract.IsCoroutineRunning) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
            Walk();
        else
            GameManager.Instance.CurrentPlayerState = PlayerState.OnIdle;
    }

    public void HandleCrouch()
    {
        if (GameManager.Instance.CurrentGameState != GameState.OnPlaying) return;
        if (axeInteract.IsCoroutineRunning) return;
        if (GameManager.Instance.CurrentMenuState == MenuState.OnNoteMenu ||
            GameManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu) return;

        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = !isCrouching;
                targetCapsuleHeight = isCrouching ? crouchCapsuleHeight : standingCapsuleHeight;
                targetCameraHeight = isCrouching ? crouchCameraHeight : standingCameraHeight;
            }
        }
    }

    public void SmoothCrouch()
    {
        float lastHeight = characterController.height;

        currentCapsuleHeight = Mathf.Lerp(currentCapsuleHeight, targetCapsuleHeight, Time.deltaTime * heightSmoothSpeed);
        currentCameraHeight = Mathf.Lerp(currentCameraHeight, targetCameraHeight, Time.deltaTime * heightSmoothSpeed);

        float heightDifference = currentCapsuleHeight - lastHeight;
        characterController.height = currentCapsuleHeight;
        characterController.center += new Vector3(0f, heightDifference / 2f, 0f);

        Vector3 camPos = playerCamera.localPosition;
        camPos.y = currentCameraHeight;
        playerCamera.localPosition = camPos;
    }

    public void Walk()
    {
        GameManager.Instance.CurrentPlayerState = PlayerState.OnWalking;
        bool isGrounded = characterController.isGrounded;

        if (isGrounded)
            velocity.y = groundedVelocity;
        else
            velocity.y += gravity * Time.deltaTime;

        Vector3 moveDirection = (transform.forward * verticalInput + (-transform.right) * horizontalInput).normalized;
        float currentSpeed = isCrouching ? crouchWalkSpeed : walkSpeed;

        Vector3 finalMovement = moveDirection * currentSpeed;
        finalMovement.y = velocity.y;

        characterController.Move(finalMovement * Time.deltaTime);
    }
}