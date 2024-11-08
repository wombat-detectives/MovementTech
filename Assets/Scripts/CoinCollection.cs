using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    CoinManager coinManager;

    private void Start()
    {
        coinManager = CoinManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if(other != null)
            {
                Destroy(other.gameObject);
                coinManager.addCoin(1);
            }
        }
    }
}
