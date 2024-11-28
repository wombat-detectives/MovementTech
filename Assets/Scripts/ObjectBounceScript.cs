using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class ObjectBounceScript : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float baseBounceForce = 10f; // Base force for the bounce
    public float velocityMultiplier = 2f; // Multiplier for downward velocity (bigger multiplier for higher bounce)
    public Vector3 bounceDirection = Vector3.up; // Direction of the bounce
    public float bounceCooldown = 0.1f; // Cooldown between bounces

    [Header("Horizontal Speed Boost Settings")]
    public float horizontalSpeedBoost = 5f; // Amount to increase the horizontal speed

    [Header("Debug Settings")]
    public bool enableDebugLogs = false; // Toggle debug logs

    private float lastBounceTime = 0f;
    private bool isPlayerInside = false; // Flag to track if the player is inside the collider

    private void OnTriggerEnter(Collider other)
    {
        // Only process bounce if the player is entering the collider and hasn't already bounced
        if (other.CompareTag("Player") && !isPlayerInside)
        {
            if (Time.time - lastBounceTime < bounceCooldown) return;

            lastBounceTime = Time.time;
            isPlayerInside = true; // Mark player as inside the collider

            // Find the Rigidbody in the object's hierarchy
            Rigidbody rb = FindRigidbodyInHierarchy(other.transform);

            if (rb != null)
            {
                // Calculate the player's downward velocity
                float downwardVelocity = Mathf.Abs(rb.linearVelocity.y);

                // Apply a larger dynamic bounce force based on the downward velocity
                float dynamicBounceForce = Mathf.Max(baseBounceForce, baseBounceForce + (downwardVelocity * velocityMultiplier));

                // Apply the bounce force
                Vector3 force = bounceDirection.normalized * dynamicBounceForce;
                rb.AddForce(force, ForceMode.Impulse);

                // Keep the player's horizontal speed (x and z) and add a boost to it
                Vector3 currentVelocity = rb.linearVelocity;
                currentVelocity.x += horizontalSpeedBoost; // Increase horizontal speed on x axis
                currentVelocity.z += horizontalSpeedBoost; // Increase horizontal speed on z axis
                rb.linearVelocity = currentVelocity;

                if (enableDebugLogs)
                {
                    Debug.Log($"Bounce applied to {other.name}: Force={force}, DownwardVelocity={downwardVelocity}");
                    Debug.Log($"New Horizontal Velocity: {currentVelocity.x}, {currentVelocity.z}");
                }
            }
            else if (enableDebugLogs)
            {
                Debug.Log($"No Rigidbody found on {other.name} or in its hierarchy.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the flag when the player leaves the collider
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private Rigidbody FindRigidbodyInHierarchy(Transform currentTransform)
    {
        // Traverse up the hierarchy to locate a Rigidbody
        while (currentTransform != null)
        {
            Rigidbody rb = currentTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"Found Rigidbody on {currentTransform.name}");
                }
                return rb;
            }
            currentTransform = currentTransform.parent; // Move up the hierarchy
        }

        return null; // No Rigidbody found
    }

    private void OnDrawGizmos()
    {
        // Visualize the bounce direction in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, bounceDirection.normalized * 2f);
    }
}
