using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGamelay : MonoBehaviour
{
    public string gameSceneName = "SampleScene";
    public GameObject panelAkhir;

    void Update()
    {
        // Panah kanan hanya berfungsi ketika Panel Akhir sudah muncul
        if (panelAkhir.activeSelf && Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayGame();
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;

        VideoToPanel.sudahPernahMain = true;

        SceneManager.LoadScene(gameSceneName);
    }
}