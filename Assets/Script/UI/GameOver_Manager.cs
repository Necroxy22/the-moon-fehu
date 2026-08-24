using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver_Manager : MonoBehaviour
{
    public static GameOver_Manager Instance;

    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI highscoreText;

    public string gameSceneName = "SampleScene";
    public string menuSceneName = "Title_Screen";

    private const string HighscoreKey = "Highscore";

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameObject.SetActive(true);
        
        Time.timeScale = 0f;

        float finalTime = GameTimer.Instance.GetTime();
        GameTimer.Instance.StopTimer();

        float savedHighscore = PlayerPrefs.GetFloat(HighscoreKey, 0f);

        if (finalTime > savedHighscore)
        {
            savedHighscore = finalTime;
            PlayerPrefs.SetFloat(HighscoreKey, savedHighscore);
            PlayerPrefs.Save();
        }

        finalTimeText.text = "Time: " + FormatTime(finalTime);
        highscoreText.text = "Best: " + FormatTime(savedHighscore);
    }

    private string FormatTime(float t)
    {
        int m = (int)(t / 60);
        int s = (int)(t % 60);
        return string.Format("{0:00}:{1:00}", m, s);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}