using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    CoinManager coinManager;
    public bool isCollected = false;

    private void Start()
    {
        gameObject.name = GetInstanceID().ToString();
        coinManager = CoinManager.instance;
        setCollected(isCollected);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other != null)
            {
                coinManager.AddCoin(1);
                setCollected(true);
            }
        }
    }

    public void setCollected(bool collected)
    {
        isCollected = collected;
        GetComponent<SphereCollider>().enabled = !collected;
        GetComponent<SpriteRenderer>().enabled = !collected;
    }
}
