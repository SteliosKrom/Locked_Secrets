using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerController : MonoBehaviour
{
    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] private CharacterController characterController;
    private Vector3 smoothMoveVelocity;
    private Vector3 velocity;

    private float horizontalInput;
    private float verticalInput;

    private float moveSpeed = 2f;
    private float gravity = -9.81f;
    private float groundedVelocity = -1f;
    #endregion

    private void Update()
    {
        HandlePlayerMovement();
    }

    public void HandlePlayerMovement()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            bool isGrounded = characterController.isGrounded;

            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (isGrounded)
               velocity.y = groundedVelocity;
            else
               velocity.y += gravity * Time.deltaTime;

            Vector3 moveDirection = (transform.forward * verticalInput + -transform.right * horizontalInput).normalized;

            Vector3 finalMovement = moveDirection * moveSpeed;

            smoothMoveVelocity = Vector3.Lerp(smoothMoveVelocity, finalMovement, 0.15f);

            finalMovement = smoothMoveVelocity;
            finalMovement.y = velocity.y;
            characterController.Move(finalMovement * Time.deltaTime);
        }
    }

    public void CheckGround()
    {
        // Here we are going to make a ground check for the player
    }
}