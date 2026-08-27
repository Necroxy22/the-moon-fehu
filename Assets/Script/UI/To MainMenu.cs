using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public string menuSceneName = "Title_Screen";
    public GameObject panelAkhir;

    void Update()
    {
        // Panah kiri hanya berfungsi ketika Panel Akhir muncul
        if (panelAkhir.activeSelf && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            BackMenu();
        }
    }

    public void BackMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}