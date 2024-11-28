using UnityEngine;

public class KeyInfo : MonoBehaviour
{
    public string keyName; 
    public string keyTime; 

    public string GetName()
    {
        return keyName;
    }

    public string GetTime()
    {
        return keyTime;
    }
}
