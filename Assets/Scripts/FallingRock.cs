using UnityEngine;

/// <summary>
/// A pooled falling rock. When it lands on Ground (or falls out of the level) it switches
/// itself off, which returns it to the pool so the spawner can reuse it.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class FallingRock : MonoBehaviour
{
    [SerializeField] private float despawnY = -10f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // A reused rock still remembers its old speed, so reset it every time it comes out of the pool.
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void Update()
    {
        // Safety net in case a rock ever misses the ground.
        if (transform.position.y < despawnY)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}
