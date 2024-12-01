using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.Cinemachine;




public class KeyCollectible : MonoBehaviour
{
    public int keyID;
    public Timer timer;

    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject playerInputObject;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [SerializeField] private CinemachineInputAxisController inputAxisController;
    [SerializeField] private Transform player; // Reference to the player transform

    // Public variables to control camera behavior
    public float cameraDistance = 5f;
    public Vector3 cameraOffset = new Vector3(0, 2, 0); // Offset to keep the camera above the player
    public float playerRotationOffset = -90f; // Additional rotation for the player

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Key pickup logic
            GameMaster.CollectKey(keyID);
            CoinManager.instance.SaveCoins();

            if (timer != null)
                timer.EndTimer(keyID);

            GameMaster.SaveGame();

            // Disable player input
            if (playerInputObject != null)
            {
                var inputComponent = playerInputObject.GetComponent<PlayerInput>();
                if (inputComponent != null)
                {
                    inputComponent.enabled = false;
                    Debug.Log("Player input disabled due to key pickup.");
                }
                else
                {
                    Debug.LogWarning("No PlayerInput component found on the assigned GameObject.");
                }
            }

            // Disable camera input
            if (inputAxisController != null)
            {
                inputAxisController.enabled = false;
                Debug.Log("Camera input disabled.");
            }

            // Play key pickup animation
            if (playerAnimations != null)
            {
                playerAnimations.PlayKeyPickupAnimation();
            }

            // Enable root motion for animator
            if (animator != null)
            {
                animator.applyRootMotion = true;
            }

            // Adjust the camera to ensure direct line of sight and rotate the player
            if (cinemachineCamera != null && player != null)
            {
                // Adjust the near clipping plane
                cinemachineCamera.Lens.NearClipPlane = 1f;

                // Start transition coroutine
                StartCoroutine(AdjustCameraForCenteredView());
            }

            // Destroy the key object
            Destroy(gameObject);
            Debug.Log("Key object destroyed.");
        }
    }


    private IEnumerator AdjustCameraForCenteredView()
    {
        float transitionDuration = 1.5f;
        float elapsedTime = 0f;

        // Rotate the player instantly before starting the camera adjustment
        RotatePlayerToFaceCamera();

        // Store initial camera properties
        Vector3 initialPosition = cinemachineCamera.transform.position;
        Vector3 targetPosition = CalculateCameraPosition(player.position);

        float initialFOV = cinemachineCamera.Lens.FieldOfView;
        float targetFOV = 45f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // Smoothly interpolate the camera's position
            cinemachineCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Smoothly interpolate the FOV
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(initialFOV, targetFOV, t);

            // Make the camera look at the player
            cinemachineCamera.transform.LookAt(player.position + cameraOffset);

            yield return null;
        }

        // Ensure the final position, FOV, and rotation are set
        cinemachineCamera.transform.position = targetPosition;
        cinemachineCamera.Lens.FieldOfView = targetFOV;
        cinemachineCamera.transform.LookAt(player.position + cameraOffset);

        Debug.Log("Camera adjustment complete with centered view and vertical FOV set to 45.");

        // Do NOT re-enable camera input
        Debug.Log("Camera input remains disabled.");
    }


    private void RotatePlayerToFaceCamera()
    {
        if (player != null && cinemachineCamera != null)
        {
            // Calculate the direction from the player to the camera
            Vector3 directionToCamera = cinemachineCamera.transform.position - player.position;
            directionToCamera.y = 0; // Keep rotation only on the horizontal plane

            // Calculate the rotation to face the camera
            Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);

            // Apply additional rotation offset
            Quaternion rotationOffset = Quaternion.Euler(0, playerRotationOffset, 0);
            targetRotation *= rotationOffset;

            // Instantly rotate the player to the target rotation
            player.rotation = targetRotation;
        }
    }


    private Vector3 CalculateCameraPosition(Vector3 playerPosition)
    {
        // Calculate the direction vector from the camera to the player
        Vector3 directionToPlayer = (playerPosition - cinemachineCamera.transform.position).normalized;

        // Adjust the camera's Y-axis to match the player's ground level
        Vector3 adjustedCameraPosition = playerPosition - directionToPlayer * cameraDistance + cameraOffset;
        adjustedCameraPosition.y = playerPosition.y; // Set Y-axis to match player's Y

        return adjustedCameraPosition;
    }

}
