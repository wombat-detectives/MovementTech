using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    CoinManager coinManager;
    public bool isCollected = false;
    public AudioClip pickupSound;
    public GameObject pickupEffect;

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
                SFXManager.instance.PlaySFXClip(pickupSound, transform, 1);
                GameObject particles = Instantiate(pickupEffect, transform.position, Quaternion.identity);
                Destroy(particles, 2f);
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
