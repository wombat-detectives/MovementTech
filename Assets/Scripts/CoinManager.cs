using System.Collections.Generic;
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene level, LoadSceneMode mode)
    {
        // get the coin hud display each time the scene loads
        TextMeshProUGUI[] hudObjects = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
        foreach (TextMeshProUGUI hudObject in hudObjects)
        {
            if (hudObject.name == "CoinCounter")
                coinText = hudObject;
        }

        LoadCoins();

        UpdateUI();
    }

    public void SaveCoins()
    {
        GameMaster.coins = coinCount;
    }

    public void LoadCoins()
    {
        coinCount = GameMaster.coins;
    }

    public void AddCoin(int count)
    {
        coinCount += count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinText.text = "x " + coinCount.ToString();
    }
}
