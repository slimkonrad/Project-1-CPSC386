using UnityEngine;

/// <summary>
/// Drops rocks from just above the top of the screen at random horizontal positions.
/// Rocks come from a SimplePool instead of being created and destroyed.
/// </summary>
public class RockSpawner : MonoBehaviour
{
    [SerializeField] private SimplePool rockPool;
    [SerializeField] private float spawnInterval = 0.8f;
    [Tooltip("How far in from the screen's left and right edges rocks can appear, so they don't spawn inside the walls.")]
    [SerializeField] private float edgeMargin = 1.5f;

    private float timer;
    private float minX;
    private float maxX;
    private float spawnY;

    private void Start()
    {
        // Work out the spawn area from what the camera can see.
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        minX = bottomLeft.x + edgeMargin;
        maxX = topRight.x - edgeMargin;
        spawnY = topRight.y + 1f;
    }

    private void Update()
    {
        // Time.deltaTime is 0 while paused, so rocks stop spawning automatically.
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnRock();
        }
    }

    private void SpawnRock()
    {
        float x = Random.Range(minX, maxX);
        rockPool.GetFromPool(new Vector3(x, spawnY, 0f));
    }
}
