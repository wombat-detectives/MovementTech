using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneWayBottomClipCollider : MonoBehaviour
{
    private Collider meshCollider;

    void Start()
    {
        meshCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // Get the relative velocity of the colliding object
            Rigidbody rb = collision.rigidbody;
            if (rb == null) continue; // Skip if the object doesn't have a Rigidbody

            Vector3 relativeVelocity = rb.linearVelocity;

            // Check if the collision is coming from below
            if (Vector3.Dot(relativeVelocity, transform.up) < 0)
            {
                // Ignore collision if the object is moving upward relative to the collider's up direction
                Physics.IgnoreCollision(collision.collider, meshCollider);
            }
        }
    }
}
