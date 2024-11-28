using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerHUD;
    private Timer timer;

    void Start()
    {
    }

    private void Awake()
    {
        timer = GetComponent<Timer>();
        if (SceneManager.GetActiveScene().name != "HubWorld")
        {
            timer.ResetTimer();
            timer.StartTimer();
        } else
        {
            timer.ResetTimer();
            timer.PauseTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timerHUD != null)
            timerHUD.text = timer.GetTimeString();
    }
}
