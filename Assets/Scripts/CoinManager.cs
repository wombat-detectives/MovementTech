using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public int coinCount;

    public TextMeshProUGUI coinText;

    //Singleton pattern, so CoinManager is available everywhere
    public static CoinManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("ERROR: more than one CoinManager in scene");
            return;
        }
        instance = this;

        //Get all coins in scene
        GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin");

        //enable or disable based on saved data
        foreach (GameObject coin in allCoins)
        {
            Debug.Log(coin.GetInstanceID());
        }
    }

    public void addCoin(int count)
    {
        coinCount += count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinText.text = coinCount.ToString();
    }
}
