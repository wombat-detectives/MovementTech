using TMPro;
using UnityEngine;

public class CastleLock : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int numKeys = 0;

        foreach(bool key in GameMaster.keyCollectedStatus)
        {
            if (key)
            {
                numKeys++;
            }
        }

        if (numKeys < 5) 
        {
            text.text = numKeys.ToString() + "/5 keys";
        } else
        {
            text.text = "you beat the game yippee";
        }
    }
}
