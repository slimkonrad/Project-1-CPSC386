using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the Main Menu: shows the saved best time and runs the Start and Quit buttons.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Level1";
    [SerializeField] private TMP_Text bestTimeText;

    private void Start()
    {
        // Safety net: if we arrived here from a paused game, make sure time runs normally.
        Time.timeScale = 1f;

        // The best time is saved by LevelManager and survives quitting the game.
        bestTimeText.text = PlayerPrefs.HasKey(LevelManager.BestTimeKey)
            ? $"Best Time: {PlayerPrefs.GetFloat(LevelManager.BestTimeKey):F1}s"
            : "Best Time: --";
    }

    /// <summary>Called by the Start button.</summary>
    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    /// <summary>Called by the Quit button. Only works in a built game, not in the Editor.</summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}