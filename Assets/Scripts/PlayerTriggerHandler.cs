using UnityEngine;

/// <summary>
/// Reacts to the triggers the player runs into: anything tagged "Hazard" kills the player,
/// and the object tagged "Finish" completes the level. The LevelManager decides what happens next.
/// </summary>
public class PlayerTriggerHandler : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            levelManager.PlayerDied();
        }
        else if (other.CompareTag("Finish"))
        {
            levelManager.LevelComplete();
        }
    }
}
