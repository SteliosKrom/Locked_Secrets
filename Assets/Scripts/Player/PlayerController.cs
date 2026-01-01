using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;

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
    private float standingHeight = 2f;
    private float crouchHeight = 1f;

    private float walkSpeed = 2f;
    private float crouchWalkSpeed = 1f;

    private float targetHeight;
    private float currentHeight;
    [SerializeField] private float heightSmoothSpeed = 5f;

    [SerializeField] private bool isCrouching;
    #endregion

    private void Start()
    {
        currentHeight = standingHeight;
        targetHeight = standingHeight;
        characterController.height = currentHeight;
        characterController.center = new Vector3(0, currentHeight / 2f, 0);
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

        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            Walk();
        }
    }

    public void HandleCrouch()
    {
        if (GameManager.Instance.CurrentGameState != GameState.OnPlaying) return;

        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = !isCrouching;
                targetHeight = isCrouching ? crouchHeight : standingHeight;
            }
        }
    }

    public void SmoothCrouch()
    {
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * heightSmoothSpeed);

        float deltaHeight = currentHeight - characterController.height;

        characterController.height = currentHeight;
        characterController.center += new Vector3(0, deltaHeight / 2, 0);

        Vector3 camPos = playerCamera.localPosition;
        camPos.y = currentHeight;
        playerCamera.localPosition = camPos;
    }

    public void Walk()
    {
        GameManager.Instance.CurrentPlayerState = PlayerState.OnWalking;
        bool isGrounded = characterController.isGrounded;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (isGrounded)
            velocity.y = groundedVelocity;
        else
            velocity.y += gravity * Time.deltaTime;

        Vector3 moveDirection = (transform.forward * verticalInput + -transform.right * horizontalInput).normalized;

        float currentSpeed = isCrouching ? crouchWalkSpeed : walkSpeed;
        Vector3 finalMovement = moveDirection * currentSpeed;

        smoothMoveVelocity = Vector3.Lerp(smoothMoveVelocity, finalMovement, 0.15f);

        finalMovement = smoothMoveVelocity;
        finalMovement.y = velocity.y;
        characterController.Move(finalMovement * Time.deltaTime);
    }
}