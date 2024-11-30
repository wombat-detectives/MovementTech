using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMaster
{

    public static float[] times = new float[4];

    //Save Data
    public static bool tutorialComplete;
    public static bool[] keyCollectedStatus = new bool[4];
    public static int coins = 0;

    public static void NewGame()
    {
        tutorialComplete = false;
        for (int i = 0; i < times.Length; i++)
        {
            times[i] = Mathf.Infinity;
            keyCollectedStatus[i] = false;
        }

        coins = 0;
    }

    public static void CollectKey(int key)
    {
        keyCollectedStatus[key] = true;

        //TODO: Send back to hubworld after showing end screen
    }

    public static void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public static void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data == null)
        {
            NewGame();
            return;
        }

        tutorialComplete = data.tutorialCompleted;
        keyCollectedStatus[0] = data.key0Collected;
        keyCollectedStatus[1] = data.key1Collected;
        keyCollectedStatus[2] = data.key2Collected;
        keyCollectedStatus[3] = data.key3Collected;

        coins = data.coins;

        times[0] = data.key0Time;
        times[1] = data.key1Time;
        times[2] = data.key2Time;
        times[3] = data.key3Time;
    }

}
