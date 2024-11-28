using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class KeyHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text timeText; // Reference for displaying time
    public TMP_Text nameText; // Reference for displaying name
    private bool KeySelectedBool = false; // Tracks if the key has been selected
    private Animator animator; // Reference to the dynamic animator
    private Button currentButton; // Tracks the current Button being interacted with

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Detect the hovered GameObject
        GameObject hoveredObject = eventData.pointerEnter;
        if (hoveredObject == null) return;

        // Get the Button component dynamically
        currentButton = hoveredObject.GetComponent<Button>();
        if (currentButton == null)
        {
            Debug.Log($"Hovered object '{hoveredObject.name}' is not a button.");
            return;
        }

        Debug.Log($"Hovered over button: {currentButton.name}");

        // Subscribe to the Button's onClick event
        currentButton.onClick.AddListener(HandleButtonClick);

        // Update TextMeshPro with KeyInfo
        KeyInfo keyInfo = hoveredObject.GetComponent<KeyInfo>();
        if (keyInfo != null)
        {
            timeText.text = keyInfo.GetTime();
            nameText.text = keyInfo.GetName();
        }

        // Handle animations dynamically
        int keyIndex = ExtractKeyIndex(hoveredObject.name);
        if (keyIndex != -1)
        {
            string keyObjectName = $"Key-0{keyIndex}";
            GameObject keyObject = GameObject.Find(keyObjectName);

            if (keyObject != null)
            {
                animator = keyObject.GetComponent<Animator>();
                if (animator != null && !KeySelectedBool)
                {
                    animator.CrossFade("KeySpin", 0.2f);
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Unsubscribe from the current Button's onClick event
        if (currentButton != null)
        {
            currentButton.onClick.RemoveListener(HandleButtonClick);
            Debug.Log($"Unsubscribed from button: {currentButton.name}");
        }

        if (animator != null && !KeySelectedBool)
        {
            animator.CrossFade("KeyIdle", 0.5f);
        }

        currentButton = null;
    }

    private void HandleButtonClick()
    {
        Debug.Log($"Button clicked: {currentButton?.name}");

        if (animator != null)
        {
            KeySelectedBool = true;
            animator.CrossFade("KeySelect", 0.2f);
            Debug.Log("KeySelect animation triggered.");
        }
    }

    private int ExtractKeyIndex(string objectName)
    {
        Match match = Regex.Match(objectName, @"\d+");
        return match.Success ? int.Parse(match.Value) : -1;
    }
}
