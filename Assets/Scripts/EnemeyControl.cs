using UnityEngine;

public class EnemeyControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public GameObject pointA;     // Left or starting point
    public GameObject pointB;     // Right or ending point
    public float speed = 3f;      // Movement speed

    private Rigidbody2D rb;
    private Vector2 targetPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        // Start moving toward point B first
        if (pointB != null)
            targetPosition = pointB.transform.position;
    }

    private void FixedUpdate()
    {
        if (pointA == null || pointB == null) return;

        // Move toward the current target
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        // If close to the target, switch direction
        if (Vector2.Distance(transform.position, targetPosition) <= 0.1f)
        {
            if (targetPosition == (Vector2)pointB.transform.position)
            {
                targetPosition = pointA.transform.position;
                FlipX();
            }
            else
            {
                targetPosition = pointB.transform.position;
                FlipX();
            }
        }
    }

    private void FlipX()
    {
        // Flip horizontally to face the opposite direction
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }
}
