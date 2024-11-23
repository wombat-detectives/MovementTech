using UnityEngine;
using Debug = UnityEngine.Debug;

public class ObjectBounceScript : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float baseBounceForce = 10f;
    public float velocityMultiplier = 2f;
    private Vector3 bounceDirection = Vector3.up;

    private float lastBounceTime = 0f;
    public float bounceCooldown = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastBounceTime < bounceCooldown) return;
        lastBounceTime = Time.time;

        // Find the Rigidbody in the hierarchy
        Rigidbody rb = FindRigidbodyInHierarchy(other.transform);

        if (rb != null)
        {
            // Calculate downward velocity
            float downwardVelocity = Mathf.Max(0, -rb.linearVelocity.y);
            float dynamicBounceForce = baseBounceForce + (downwardVelocity * velocityMultiplier);

            // Apply the bounce force
            Vector3 force = bounceDirection.normalized * dynamicBounceForce;
            rb.AddForce(force, ForceMode.Impulse);

            Debug.Log($"Bounce applied: {force}, Downward Velocity: {downwardVelocity}");
        }
        else
        {
            Debug.Log($"No Rigidbody found on {other.name} or in its hierarchy.");
        }
    }

    private Rigidbody FindRigidbodyInHierarchy(Transform currentTransform)
    {
        // Traverse up the hierarchy to find a Rigidbody
        while (currentTransform != null)
        {
            Rigidbody rb = currentTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log($"Found Rigidbody on {currentTransform.name}");
                return rb;
            }
            currentTransform = currentTransform.parent;
        }

        return null; // No Rigidbody found
    }
}
