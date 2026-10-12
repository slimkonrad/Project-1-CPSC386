using UnityEngine;

/// <summary>
/// Zooms and centers the orthographic camera so the whole level is always visible,
/// whatever the screen's shape (16:9 monitor, 16:10 laptop, browser window, etc.).
/// Runs in Awake so other scripts (like RockSpawner) see the final camera view in Start.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    [Tooltip("The Ground tilemap's TilemapRenderer. Its bounds define the level area to keep in view.")]
    [SerializeField] private Renderer levelRenderer;
    [Tooltip("Extra space around the level, in world units.")]
    [SerializeField] private float padding = 0.25f;

    private Camera cam;
    private float lastAspect;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        FitToLevel();
    }

    private void Update()
    {
        // Re-fit if the window is resized (for example, entering fullscreen in a browser).
        if (!Mathf.Approximately(cam.aspect, lastAspect))
        {
            FitToLevel();
        }
    }

    private void FitToLevel()
    {
        Bounds level = levelRenderer.bounds;

        // Center the camera on the level.
        transform.position = new Vector3(level.center.x, level.center.y, transform.position.z);

        // Pick the zoom that fits both the level's height and its width on this screen shape.
        float halfHeight = level.extents.y + padding;
        float halfWidth = level.extents.x + padding;
        cam.orthographicSize = Mathf.Max(halfHeight, halfWidth / cam.aspect);

        lastAspect = cam.aspect;
    }
}
