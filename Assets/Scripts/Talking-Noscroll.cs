using UnityEngine;
using TMPro;
public class ShowText : MonoBehaviour
{
    // Dialogue vars
    public TMP_Text NPCDialogue;

    void Awake()
    {
        NPCDialogue.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !NPCDialogue.enabled)
        {
            NPCDialogue.enabled = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !NPCDialogue.enabled)
        {
            NPCDialogue.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && NPCDialogue.enabled)
        {
            NPCDialogue.enabled = false;
        }
    }

}