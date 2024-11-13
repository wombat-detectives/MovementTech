using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    CoinManager coinManager;

    private void Start()
    {
        coinManager = CoinManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other != null)
            {
                coinManager.addCoin(1);
                Destroy(gameObject);
            }
        }
    }
}
