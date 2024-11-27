using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class KeySelectScreen : MonoBehaviour
{
    public GameObject defaultSelectedButton; // Assign the default button in the Inspector
    private EventSystem eventSystem;

    [SerializeField] private GameObject playerPrefab; // Player prefab to spawn
    [SerializeField] private Transform spawnPoint; // Transform for the player to spawn here

    private void Awake()
    {
        // Ensure EventSystem exists
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("No EventSystem found in the scene. Please add one.");
        }

        // Automatically select the default button
        if (defaultSelectedButton != null)
        {
            eventSystem.SetSelectedGameObject(defaultSelectedButton);
        }
    }

    private void Update()
    {
        // Re-select button if nothing is selected (prevents losing focus)
        if (eventSystem.currentSelectedGameObject == null && defaultSelectedButton != null)
        {
            eventSystem.SetSelectedGameObject(defaultSelectedButton);
        }

        // Check for interaction
        if (Input.GetButtonDown("Submit")) // Replace with your input system method for "Submit"
        {
            var currentButton = eventSystem.currentSelectedGameObject?.GetComponent<Button>();
            currentButton?.onClick.Invoke(); // Trigger the button's action
        }

        // Cancel button for exiting menu
        if (Input.GetButtonDown("Cancel")) // Replace with your input system method for "Cancel"
        {
            // Handle pause menu exit logic here
            Debug.Log("Cancel pressed - Handle menu exit");
        }
    }

    public void KeySelect(int keyValue)
    {
        // Example: Log and perform actions based on keyValue
        Debug.Log("Key selected: " + keyValue);

        // Set canvas alpha to 0
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        // Store the selected key value globally (can use a static variable or a game manager)
        GlobalGameManager.Instance.SelectedKey = keyValue;

        // Spawn player
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Player spawned at: " + spawnPoint.position);
        }
        else
        {
            Debug.LogError("Player prefab or spawn point is not assigned.");
        }
    }
}
