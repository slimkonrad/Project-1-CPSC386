using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Runs the level: counts the run timer, restarts the level when the player dies,
/// and on reaching the finish shows the win panel and saves the best time with PlayerPrefs.
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>PlayerPrefs key for the best time. MainMenu reads the same key.</summary>
    public const string BestTimeKey = "BestTime";

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winText;

    [Header("Scenes")]
    [SerializeField] private string menuSceneName = "MainMenu";

    private float elapsedTime;
    private bool isRunning = true;

    /// <summary>True once the finish has been reached, so other scripts (like the pause menu) can ignore input.</summary>
    public bool IsLevelComplete => !isRunning;

    private void Start()
    {
        Time.timeScale = 1f;
        winPanel.SetActive(false);
    }

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        timerText.text = $"Time: {elapsedTime:F1}s";
    }

    /// <summary>Called when the player touches a hazard. Restarts the level from the beginning.</summary>
    public void PlayerDied()
    {
        if (!isRunning)
        {
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Called when the player reaches the finish flag.</summary>
    public void LevelComplete()
    {
        if (!isRunning)
        {
            return;
        }

        isRunning = false;
        Time.timeScale = 0f; // Freeze the rocks behind the win panel.

        bool hasBest = PlayerPrefs.HasKey(BestTimeKey);
        float bestTime = PlayerPrefs.GetFloat(BestTimeKey);
        bool isNewBest = !hasBest || elapsedTime < bestTime;

        if (isNewBest)
        {
            bestTime = elapsedTime;
            PlayerPrefs.SetFloat(BestTimeKey, bestTime);
            PlayerPrefs.Save();
        }

        winText.text = isNewBest
            ? $"New Best Time!\n{elapsedTime:F1}s"
            : $"Time: {elapsedTime:F1}s\nBest: {bestTime:F1}s";
        winPanel.SetActive(true);
    }

    /// <summary>Called by the win panel's Play Again button.</summary>
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Called by the win panel's Main Menu button.</summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
