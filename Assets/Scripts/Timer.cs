using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static float timer;
    public static bool runTimer;

    private void Update()
    {
        if (runTimer)
            timer += Time.deltaTime;
    }

    public void StartTimer()
    {
        runTimer = true;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public void PauseTimer()
    {
        runTimer = false;
    }
    public void EndTimer(int key)
    {
        runTimer = false;
        if(timer < GameMaster.times[key] || GameMaster.times[key] == 0)
        {
            GameMaster.times[key] = timer;
        }
        Debug.Log("Best Time: " + GetTimeString(GameMaster.times[key]) + " Current time: " + GetTimeString(timer));
    }

    public string GetTimeString()
    {
        float minutes = (float)Math.Floor(timer / 60);
        string seconds = string.Format("{0:00.00}", (timer % 60));
        return minutes + ":" + seconds;
    }

    public string GetTimeString(float time)
    {
        float minutes = (float)Math.Floor(time / 60);
        string seconds = string.Format("{0:00.00}", (time % 60));
        
        return minutes + ":" + seconds;
    }
}
