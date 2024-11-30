using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Collections;


public class KeyCollectible : MonoBehaviour
{
    public int keyID;
    public Timer timer;

    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject playerInputObject;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

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

            // Smoothly transition the Cinemachine camera settings
            if (virtualCamera != null)
            {
                StartCoroutine(SmoothCameraTransition());
            }
        }
    }

    private IEnumerator SmoothCameraTransition()
    {
        float transitionDuration = 1.5f;
        float elapsedTime = 0f;

        // Store initial values
        float initialFOV = virtualCamera.m_Lens.FieldOfView;
        float targetFOV = 15f;

        var orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        float initialHorizontalAxis = orbitalTransposer.m_XAxis.Value;
        float targetHorizontalAxis = -50f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // Interpolate FOV
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, targetFOV, t);

            // Interpolate horizontal axis
            orbitalTransposer.m_XAxis.Value = Mathf.Lerp(initialHorizontalAxis, targetHorizontalAxis, t);

            yield return null;
        }

        // Ensure final values are set
        virtualCamera.m_Lens.FieldOfView = targetFOV;
        orbitalTransposer.m_XAxis.Value = targetHorizontalAxis;
    }
}
