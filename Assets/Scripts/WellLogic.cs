using UnityEngine;

public class WellLogic : MonoBehaviour
{
    public float lowerSpeed = 0.5f; // Speed at which the well's parent lowers
    public float lowerAmount = 5f; // Total amount to lower the well's Y position
    private int maxGoldenSandMan = 3; // Maximum allowed "GoldenSandMan" objects
    private int currentGoldenSandManCount = 0;
    private bool shouldLower = false;
    private Vector3 targetPosition;

    private void Start()
    {
        // Initialize the target position
        targetPosition = transform.parent.position;
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
                targetPosition = transform.parent.position - new Vector3(0, lowerAmount / maxGoldenSandMan, 0);
                shouldLower = true;
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
            }
        }
    }
}
