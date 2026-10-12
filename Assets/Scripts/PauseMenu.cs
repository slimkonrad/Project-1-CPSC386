using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Opens and closes the pause menu with the Pause action (Escape).
/// Pausing sets Time.timeScale to 0, which freezes physics, falling rocks, and the timer.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private string menuSceneName = "MainMenu";

    private bool isPaused;

    private void OnEnable()
    {
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
    }

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        // No pausing once the win panel is showing.
        if (!pauseAction.action.WasPressedThisFrame() || levelManager.IsLevelComplete)
        {
            return;
        }

        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    /// <summary>Called by the Resume button, or by pressing Escape again.</summary>
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    /// <summary>Called by the Quit to Menu button.</summary>
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}