using UnityEngine;

public class FootstepsSystem : MonoBehaviour
{
    private float radius = 0.2f;

    [SerializeField] private Transform sphereTransform;
    [SerializeField] private LayerMask groundLayers;

    private void Update()
    {
        string groundType = IsGrounded();

        if (groundType == "WoodGround")
            Debug.Log("Its wood ground");
        else if (groundType == "StoneGround")
            Debug.Log("Its stone ground");
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
