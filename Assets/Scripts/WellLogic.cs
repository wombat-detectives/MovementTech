using UnityEngine;
using System.Collections.Generic; // For Queue

public class WellLogic : MonoBehaviour
{
    public float lowerSpeed = 0.5f; // Speed at which the well's parent lowers
    public float lowerAmount = 5f; // Total amount to lower the well's Y position
    private int maxGoldenSandMan = 3; // Maximum allowed "GoldenSandMan" objects
    private int currentGoldenSandManCount = 0;
    private bool shouldLower = false;
    private Vector3 targetPosition;

    private Queue<Vector3> positionQueue = new Queue<Vector3>(); // Queue to store pending target positions
    private BoxCollider boxCollider; // Reference to the BoxCollider

    private void Start()
    {
        // Initialize the target position
        targetPosition = transform.parent.position;

        // Get the BoxCollider attached to this object
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            Debug.LogWarning("No BoxCollider found on the WellLogic object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object's name contains "GoldenSandMan"
        if (other.name.Contains("GoldenSandMan") && currentGoldenSandManCount < maxGoldenSandMan)
        {
            currentGoldenSandManCount++;
            Destroy(other.gameObject); // Destroy the "GoldenSandMan" object

            if (currentGoldenSandManCount <= maxGoldenSandMan)
            {
                // Calculate the new target position
                Vector3 newTargetPosition = transform.parent.position - new Vector3(0, lowerAmount / maxGoldenSandMan, 0);

                // Add the new target position to the queue
                positionQueue.Enqueue(newTargetPosition);

                // If no lowering operation is currently active, start lowering
                if (!shouldLower)
                {
                    ProcessNextLowering();
                }

                // Disable the BoxCollider if all "GoldenSandMan" objects are collected
                if (currentGoldenSandManCount == maxGoldenSandMan && boxCollider != null)
                {
                    boxCollider.enabled = false;
                    Debug.Log("All GoldenSandMan collected. BoxCollider disabled.");
                }
            }
        }
    }

    private void Update()
    {
        if (shouldLower)
        {
            // Gradually move the parent object downwards
            transform.parent.position = Vector3.MoveTowards(
                transform.parent.position,
                targetPosition,
                lowerSpeed * Time.deltaTime
            );

            // Stop lowering if the target position is reached
            if (transform.parent.position == targetPosition)
            {
                shouldLower = false;

                // Process the next target position in the queue
                ProcessNextLowering();
            }
        }
    }

    private void ProcessNextLowering()
    {
        if (positionQueue.Count > 0)
        {
            // Get the next target position from the queue
            targetPosition = positionQueue.Dequeue();
            shouldLower = true;
        }
    }
}
