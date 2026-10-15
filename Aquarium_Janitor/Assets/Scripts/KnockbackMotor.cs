using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackMotor : MonoBehaviour
{
    public float knockbackDistance = 2f;
    public float knockbackSpeed = 8f;

    private Rigidbody2D rb2D;
    private Collider2D bodyCollider;

    private Vector2 direction;
    private float distanceRemaining;
    private float requestedStep;
    private Vector2 previousPosition;
    private bool awaitingMovement;

    public bool IsKnockedBack { get; private set; }

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
    }

    public void ApplyKnockback(Vector2 awayFromSource)
    {
        if (awayFromSource.sqrMagnitude < 0.001f)
            awayFromSource = Vector2.up;

        direction = awayFromSource.normalized;
        distanceRemaining = knockbackDistance;
        awaitingMovement = false;
        IsKnockedBack = distanceRemaining > 0f;
    }

    void FixedUpdate()
    {
        if (!IsKnockedBack)
            return;

        // Measure the previous physics movement.
        if (awaitingMovement)
        {
            float moved = Vector2.Distance(
                previousPosition, rb2D.position
            );

            distanceRemaining -= moved;

            // A collision or obstruction prevented movement.
            if (moved < requestedStep * 0.5f)
            {
                StopKnockback();
                return;
            }

            if (distanceRemaining <= 0.001f)
            {
                StopKnockback();
                return;
            }
        }

        requestedStep = Mathf.Min(
            knockbackSpeed * Time.fixedDeltaTime,
            distanceRemaining
        );

        // Check whether a wall is directly in the way.
        if (bodyCollider != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                rb2D.position,
                direction,
                requestedStep
            );

            if (hit.collider != null &&
                !hit.collider.isTrigger &&
                hit.collider.transform != transform &&
                !hit.collider.transform.IsChildOf(transform))
            {
                StopKnockback();
                return;
            }
        }

        previousPosition = rb2D.position;
        awaitingMovement = true;

        rb2D.MovePosition(
            rb2D.position + direction * requestedStep
        );
    }

    private void StopKnockback()
    {
        IsKnockedBack = false;
        awaitingMovement = false;
        distanceRemaining = 0f;
    }
}