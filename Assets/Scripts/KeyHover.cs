using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;


public class KeyHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text timeText;
    public TMP_Text nameText;
    private bool KeySelectedBool = false;
    private Animator animator;
    private Button currentButton;

    [SerializeField] private AudioClip keyAudioClip; // Clip to be used
    [SerializeField] private AudioSource audioSource; // Dedicated audio source

    private void Awake()
    {
        // Ensure the cursor is visible and unlocked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Find the MusicPlayer object in the scene
        GameObject musicPlayerObject = FindMusicPlayer();
        if (musicPlayerObject != null)
        {
            AudioSource musicPlayerAudio = musicPlayerObject.GetComponent<AudioSource>();
            if (musicPlayerAudio != null)
            {
                ReplaceMusicPlayerAudio(musicPlayerAudio, keyAudioClip);
            }
            else
            {
                Debug.LogWarning("MusicPlayer found, but no AudioSource component is attached.");
            }
        }
        else
        {
            Debug.LogWarning("No MusicPlayer object found in the scene.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject hoveredObject = eventData.pointerEnter;
        if (hoveredObject == null) return;

        currentButton = hoveredObject.GetComponent<Button>();
        if (currentButton == null)
        {
            Debug.Log($"Hovered object '{hoveredObject.name}' is not a button.");
            return;
        }

        Debug.Log($"Hovered over button: {currentButton.name}");
        currentButton.onClick.AddListener(HandleButtonClick);

        KeyInfo keyInfo = hoveredObject.GetComponent<KeyInfo>();
        if (keyInfo != null)
        {
            timeText.text = keyInfo.GetTime();
            nameText.text = keyInfo.GetName();
        }

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

        if (audioSource != null && keyAudioClip != null)
        {
            audioSource.PlayOneShot(keyAudioClip);
            Debug.Log("Audio clip played through the audio source.");
        }
    }

    private void ReplaceMusicPlayerAudio(AudioSource musicPlayerAudio, AudioClip newClip)
    {
        if (musicPlayerAudio.clip != newClip)
        {
            musicPlayerAudio.clip = newClip;
            musicPlayerAudio.Play();
            Debug.Log("MusicPlayer audio source updated and playing new clip.");
        }
        else
        {
            Debug.Log("MusicPlayer is already using the new clip.");
        }
    }

    private int ExtractKeyIndex(string objectName)
    {
        Match match = Regex.Match(objectName, @"\d+");
        return match.Success ? int.Parse(match.Value) : -1;
    }

    private GameObject FindMusicPlayer()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.name.Contains("MusicPlayer"))
            {
                return obj;
            }
        }
        return null;
    }
}
