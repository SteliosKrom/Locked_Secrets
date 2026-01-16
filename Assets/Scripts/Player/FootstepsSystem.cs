using UnityEngine;

public class FootstepsSystem : MonoBehaviour
{
    private float timeBetweenSteps = 0.4f;
    private float timeSinceLastStep = 0f;
    private float radius = 0.2f;

    [SerializeField] private Transform sphereTransform;
    [SerializeField] private LayerMask groundLayers;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerController playerController;
    #endregion

    #region AUDIO
    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource footstepsAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip woodGroundClip;
    [SerializeField] private AudioClip stoneGroundClip;
    #endregion

    private void Update()
    {
        if (GameManager.Instance.CanMenuInteract()) return;

        if (GameManager.Instance.CanItemMenuInteract()) return;

        string groundType = IsGrounded();
        Vector2 moveInput = new Vector2(playerController.HorizontaInput, playerController.VerticalInput);

        if (moveInput.magnitude > 0.1f)
        {
            timeSinceLastStep += Time.deltaTime;

            timeBetweenSteps = playerController.IsCrouching ? 1f : 0.6f;

            if (GameManager.Instance.CurrentPlayerState == PlayerState.OnWalking)
            {
                if (timeSinceLastStep >= timeBetweenSteps)
                {
                    PlayWalkFootstepsSound(groundType);
                    timeSinceLastStep = 0f;
                }
            }
        }
        else
        {
            timeSinceLastStep = 0f;
        }
    }

    public void PlayWalkFootstepsSound(string groundType)
    {
        if (playerController.IsCrouching)
            footstepsAudioSource.pitch = Random.Range(0.7f, 0.9f);
        else
            footstepsAudioSource.pitch = Random.Range(1f, 1.2f);

        if (groundType == "WoodGround")
            footstepsAudioSource.PlayOneShot(woodGroundClip);
        else if (groundType == "StoneGround")
            footstepsAudioSource.PlayOneShot(stoneGroundClip);
    }

    public string IsGrounded()
    {
        Collider[] hits = Physics.OverlapSphere(sphereTransform.position, radius, groundLayers);

        foreach (Collider hit in hits)
        {
            int layer = hit.gameObject.layer;
            if (layer == LayerMask.NameToLayer("WoodGround")) return "WoodGround";
            if (layer == LayerMask.NameToLayer("StoneGround")) return "StoneGround";
        }
        return "None";
    }
}
