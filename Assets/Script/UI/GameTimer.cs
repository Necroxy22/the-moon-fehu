using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    public TextMeshProUGUI timerText;
    public bool isRunning = true;

    private float currentTime = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime += Time.deltaTime;

        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer() => isRunning = false;
    public float GetTime() => currentTime;
}
