using UnityEngine;

/// <summary>
/// Drops rocks from just above the top of the screen at random horizontal positions inside the level.
/// Rocks come from a SimplePool instead of being created and destroyed.
/// </summary>
public class RockSpawner : MonoBehaviour
{
    [SerializeField] private SimplePool rockPool;
    [Tooltip("The Ground tilemap's TilemapRenderer. Rocks only spawn above this area.")]
    [SerializeField] private Renderer levelRenderer;
    [SerializeField] private float spawnInterval = 0.8f;
    [Tooltip("How far in from the level's left and right edges rocks can appear, so they don't spawn inside the walls.")]
    [SerializeField] private float edgeMargin = 1.5f;

    private float timer;
    private float minX;
    private float maxX;
    private float spawnY;

    private void Start()
    {
        // Spawn across the level's width, just above whatever the camera can see.
        Bounds level = levelRenderer.bounds;
        float cameraTop = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;

        minX = level.min.x + edgeMargin;
        maxX = level.max.x - edgeMargin;
        spawnY = cameraTop + 1f;
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