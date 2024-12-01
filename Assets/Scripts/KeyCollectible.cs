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
    [SerializeField] private Transform player;

    public float cameraDistance = 5f;
    public Vector3 cameraOffset = new Vector3(0, 2, 0);
    public float playerRotationOffset = -90f;

    [SerializeField] private AudioClip keyPickupClip; // Variable audio clip for key pickup
    [SerializeField] private string targetSceneName = "HubWorld"; // Name of the scene to load
    [SerializeField] private float sceneLoadDelay = 3.0f; // Delay before loading the scene

    private MeshRenderer[] meshRenderers;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMaster.CollectKey(keyID);
            CoinManager.instance.SaveCoins();

            if (timer != null)
                timer.EndTimer(keyID);

            GameMaster.SaveGame();

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

            if (inputAxisController != null)
            {
                inputAxisController.enabled = false;
                Debug.Log("Camera input disabled.");
            }

            if (playerAnimations != null)
            {
                playerAnimations.PlayKeyPickupAnimation();
            }

            if (animator != null)
            {
                animator.applyRootMotion = true;
            }

            if (cinemachineCamera != null && player != null)
            {
                cinemachineCamera.Lens.NearClipPlane = 1f;
                StartCoroutine(AdjustCameraForCenteredView());
            }

            HandleMusicPlayer(); // New logic for handling music player
            HideKey();
            StartCoroutine(WaitAndLoadScene());
        }
    }

    private void HandleMusicPlayer()
    {
        // Find the music player object by searching for a name containing "MusicPlayer"
        GameObject musicPlayerObject = FindMusicPlayer();

        if (musicPlayerObject != null)
        {
            AudioSource musicPlayer = musicPlayerObject.GetComponent<AudioSource>();
            if (musicPlayer != null)
            {
                Debug.Log("Replacing music player's audio with key pickup music.");

                // Stop the current music
                musicPlayer.Stop();

                // Replace the clip with key pickup clip if desired
                musicPlayer.clip = keyPickupClip;

                // Optionally loop the new clip if it's meant to be background music
                musicPlayer.loop = true;

                // Play the new clip
                musicPlayer.Play();
            }
            else
            {
                Debug.LogWarning("No AudioSource component found on the MusicPlayer object.");
            }
        }
        else
        {
            Debug.LogWarning("No object with name containing 'MusicPlayer' found in the scene.");
        }
    }

    private GameObject FindMusicPlayer()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.name.Contains("MusicPlayer") && obj.GetComponent<AudioSource>() != null)
            {
                return obj;
            }
        }
        return null;
    }

    private void HideKey()
    {
        if (meshRenderers != null)
        {
            foreach (var renderer in meshRenderers)
            {
                renderer.enabled = false;
            }
        }

        Debug.Log("Key visuals hidden.");
    }

    private IEnumerator AdjustCameraForCenteredView()
    {
        float transitionDuration = 1.5f;
        float elapsedTime = 0f;

        RotatePlayerToFaceCamera();

        Vector3 initialPosition = cinemachineCamera.transform.position;
        Vector3 targetPosition = CalculateCameraPosition(player.position);

        float initialFOV = cinemachineCamera.Lens.FieldOfView;
        float targetFOV = 45f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            cinemachineCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(initialFOV, targetFOV, t);
            cinemachineCamera.transform.LookAt(player.position + cameraOffset);

            yield return null;
        }

        cinemachineCamera.transform.position = targetPosition;
        cinemachineCamera.Lens.FieldOfView = targetFOV;
        cinemachineCamera.transform.LookAt(player.position + cameraOffset);

        Debug.Log("Camera adjustment complete.");
    }

    private IEnumerator WaitAndLoadScene()
    {
        Debug.Log($"Waiting for {sceneLoadDelay} seconds before loading the scene...");
        yield return new WaitForSeconds(sceneLoadDelay);

        Debug.Log($"Loading scene: {targetSceneName}");
        LevelLoader.instance.LoadLevelByString(targetSceneName);
    }

    private void RotatePlayerToFaceCamera()
    {
        if (player != null && cinemachineCamera != null)
        {
            Vector3 directionToCamera = cinemachineCamera.transform.position - player.position;
            directionToCamera.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);
            Quaternion rotationOffset = Quaternion.Euler(0, playerRotationOffset, 0);
            targetRotation *= rotationOffset;

            player.rotation = targetRotation;
        }
    }

    private Vector3 CalculateCameraPosition(Vector3 playerPosition)
    {
        Vector3 directionToPlayer = (playerPosition - cinemachineCamera.transform.position).normalized;
        Vector3 adjustedCameraPosition = playerPosition - directionToPlayer * cameraDistance + cameraOffset;
        adjustedCameraPosition.y = playerPosition.y;

        return adjustedCameraPosition;
    }
}
