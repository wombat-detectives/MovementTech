
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SlowYVelocity : MonoBehaviour
{
    public float slowForce = 5f; // Force applied to counter upward movement
    public float maxYVelocity = 2f; // Maximum allowed upward velocity

    private void OnTriggerStay(Collider other)
    {
        // Check if the parent or the object itself has a Rigidbody
        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            // Only apply the force if the Y velocity exceeds the max allowed velocity
            if (rb.linearVelocity.y > maxYVelocity)
            {
                Debug.Log("slowing down y velocity");
                Vector3 force = new Vector3(0, -slowForce, 0); // Downward force
                rb.AddForce(force, ForceMode.Acceleration);   // Apply the force
            }
        }
    }
}
