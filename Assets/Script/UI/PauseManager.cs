using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Pause UI Settings")]
    [Tooltip("Panel UI Pause (misal panel buku/popup)")]
    public GameObject pausePanel;

    [Header("Scenes")]
    public string menuSceneName = "Title_Screen";
    public string gameSceneName = "SampleScene";

    private bool isPaused = false;
    public bool IsPaused => isPaused;

    void Awake()
    {
        Instance = this;
        
        if (pausePanel == null)
        {
            pausePanel = this.gameObject;
        }

        // Pastikan saat mulai game, panel langsung tertutup
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        // Dengarkan input P atau Escape
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            // Jangan bisa pause kalau di layar Game Over
            if (GameOver_Manager.Instance != null && GameOver_Manager.Instance.gameObject.activeInHierarchy)
                return;

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
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
