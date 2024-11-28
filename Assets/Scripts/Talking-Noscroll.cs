using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class ShowText : MonoBehaviour
{
    // Scene stuff for reveal
    [Tooltip("Only needed if target self is false")] public string targetScene;
    public bool targetSelf = false;
    // Dialogue vars
    public TMP_Text NPCDialogue;

    void Awake()
    {
        NPCDialogue.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        NPCDialogue.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        NPCDialogue.enabled = false;
    }

}