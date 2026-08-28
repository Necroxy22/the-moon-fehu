using UnityEngine;

public class PauseController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameOver_Manager.Instance != null && GameOver_Manager.Instance.gameObject.activeInHierarchy)
                return;

            if (PauseManager.Instance != null)
            {
                if (PauseManager.Instance.IsPaused)
                {
                    PauseManager.Instance.ResumeGame();
                }
                else
                {
                    PauseManager.Instance.PauseGame();
                }
            }
        }
    }
}
