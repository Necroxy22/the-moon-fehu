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

    private CanvasGroup panelCanvasGroup;

    void Awake()
    {
        Instance = this;

        if (pausePanel == null)
        {
            pausePanel = this.gameObject;
        }

        panelCanvasGroup = pausePanel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
        {
            panelCanvasGroup = pausePanel.AddComponent<CanvasGroup>();
        }

        HideUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameOver_Manager.Instance != null && GameOver_Manager.Instance.IsGameOver)
                return;
            
            if (!isPaused)
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        ShowUI();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        HideUI();
    }

    private void ShowUI()
    {
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 1f;
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.blocksRaycasts = true;
        }
        else if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    private void HideUI()
    {
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }
        else if (pausePanel != null)
        {
            pausePanel.SetActive(false);
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
