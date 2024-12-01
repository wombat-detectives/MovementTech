
using UnityEngine;

public class KeyInfo : MonoBehaviour
{
    public string keyName; // The name of the key
    public int keyIndex; // The index of the key in GameMaster.times array

    public string GetName()
    {
        return keyName;
    }

    public string GetTime()
    {
        // Ensure the key index is valid and within the GameMaster.times array bounds
        if (GameMaster.times != null && keyIndex >= 0 && keyIndex < GameMaster.times.Length)
        {
            float time = GameMaster.times[keyIndex];
            return Timer.GetTimeString(time);
        }

        Debug.LogWarning("Invalid key index or GameMaster.times array not initialized.");
        return "N/A"; 
    }
}
