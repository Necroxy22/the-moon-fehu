using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGamelay : MonoBehaviour
{
    public string gameSceneName = "SampleScene";

    void Update()
    {
        // Panah kanan → masuk ke Game
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayGame();
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}