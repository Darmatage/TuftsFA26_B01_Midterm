
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveHit : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D rb2D;
    public float speed = 4f;
    private Transform target;
    private Vector2 movementTarget;
    public int damage = 1;
    public int EnemyLives = 3;
    private GameHandler gameHandler;

    public float attackRange = 10f;
    public bool isAttacking = false;
    private float scaleX;

    [Header("Knockback and Stun")]
    public float knockbackDistance = 2f;
    public float knockbackSpeed = 8f;
    public float stunDuration = 3f;

    private bool isStunned = false;
    private float stunEndTime = 0f;

    private KnockbackMotor knockbackMotor;
    private Coroutine knockbackCoroutine;

    private GridPathfinder pathfinder;
    private List<Vector2> currentPath = new List<Vector2>();
    private int pathIndex = 0;
    private float nextPathUpdate = 0f;

    [Header("Pathfinding")]
    public float pathUpdateInterval = 0.25f;
    public float waypointDistance = 0.1f;

    void Start()
    {
        pathfinder = FindObjectOfType<GridPathfinder>();
        anim = GetComponentInChildren<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        knockbackMotor = GetComponent<KnockbackMotor>();

        scaleX = transform.localScale.x;

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }

        GameObject handler = GameObject.FindWithTag("GameHandler");

        if (handler != null)
        {
            gameHandler = handler.GetComponent<GameHandler>();
        }
    }

    void Update()
    {
        if (target == null || rb2D == null)
            return;

        if (knockbackMotor != null &&
        knockbackMotor.IsKnockedBack)
        {
            return;
        }

        // Do not chase while stunned or being knocked back.
        if (isStunned)
        {
            if (Time.time < stunEndTime)
                return;

            isStunned = false;
        }

        float distToPlayer =
            Vector2.Distance(rb2D.position, target.position);

        if (distToPlayer > attackRange)
        {
            currentPath.Clear();
            pathIndex = 0;
            return;
        }

        if (pathfinder == null)
            return;

        // Recalculate the path periodically.
        if (Time.time >= nextPathUpdate)
        {
            currentPath = pathfinder.FindPath(
                rb2D.position,
                target.position
            );

            pathIndex = currentPath.Count > 1 ? 1 : 0;
            nextPathUpdate = Time.time + pathUpdateInterval;
        }

        if (pathIndex >= currentPath.Count)
            return;

        Vector2 waypoint = currentPath[pathIndex];

        // Face the direction of travel.
        float directionX = waypoint.x - rb2D.position.x;

        if (Mathf.Abs(directionX) > 0.01f)
        {
            transform.localScale = new Vector2(
                directionX > 0 ? scaleX : -scaleX,
                transform.localScale.y
            );
        }

        movementTarget = waypoint;

        if (Mathf.Abs(directionX) > 0.01f)
        {
            transform.localScale = new Vector2(
                directionX > 0 ? scaleX : -scaleX,
                transform.localScale.y
            );
        }

        if (Vector2.Distance(rb2D.position, waypoint)
            <= waypointDistance)
        {
            pathIndex++;
        }
    }

    void FixedUpdate()
    {
        if (rb2D == null || isStunned)
            return;

        if (target == null || pathIndex >= currentPath.Count)
            return;

        rb2D.MovePosition(Vector2.MoveTowards(
            rb2D.position,
            movementTarget,
            speed * Time.fixedDeltaTime
        ));
    }
    public void TakeHit()
    {
        if (target == null || knockbackMotor == null)
            return;

        Vector2 direction =
            (rb2D.position - (Vector2)target.position).normalized;

        isStunned = true;
        stunEndTime = Time.time + stunDuration;

        knockbackMotor.ApplyKnockback(direction);

        Debug.Log("Enemy knocked back and stunned.");
    }

    private IEnumerator KnockbackRoutine(Vector2 direction)
    {
        float distanceTravelled = 0f;

        while (distanceTravelled < knockbackDistance)
        {
            float step = Mathf.Min(
                knockbackSpeed * Time.fixedDeltaTime,
                knockbackDistance - distanceTravelled
            );

            Vector2 before = rb2D.position;

            rb2D.MovePosition(before + direction * step);

            yield return new WaitForFixedUpdate();

            float actualDistance = Vector2.Distance(
                before, rb2D.position
            );

            distanceTravelled += actualDistance;

            // Stop if a wall prevents further movement.
            if (actualDistance < step * 0.5f)
                break;
        }

        knockbackCoroutine = null;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        isAttacking = true;

        // A stunned enemy cannot damage the player.
        if (isStunned || Time.time < stunEndTime)
            return;

        PlayerHealth playerHealth =
            other.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1, transform.position);
        }
        else
        {
            Debug.LogWarning(
                "Player is missing the PlayerHealth component!"
            );
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
