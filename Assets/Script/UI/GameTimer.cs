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

    private int lastSeconds = -1;

    void Awake()
    {
        Instance = this;

        if (DifficultyManager.Instance == null)
        {
            GameObject dmObj = new GameObject("DifficultyManager");
            dmObj.AddComponent<DifficultyManager>();
        }
    }

    void Update()
    {
        if (!isRunning || timerText == null) return;

        currentTime += Time.deltaTime;

        int totalSeconds = (int)currentTime;
        if (totalSeconds != lastSeconds)
        {
            lastSeconds = totalSeconds;
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = string.Format("TIME: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer() => isRunning = false;
    public float GetTime() => currentTime;
}
