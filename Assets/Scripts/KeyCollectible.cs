using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public int keyID;
    public Timer timer;

    private void Awake()
    {
       timer = FindFirstObjectByType<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameMaster.CollectKey(keyID);
            if(timer != null)
                timer.EndTimer(keyID);
            GameMaster.SaveGame();
        }
    }
}
