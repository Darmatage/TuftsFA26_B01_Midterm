using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveHit : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D rb2D;
    public float speed = 4f;
    private Transform target;
    public int damage = 100;
    private float stunEndTime = 0f;
    public int EnemyLives = 3;
    private GameHandler gameHandler;

    public float attackRange = 10f;
    public bool isAttacking = false;
    private float scaleX;

    [Header("Knockback and Stun")]
    public float knockbackForce = 8f;
    public float stunDuration = 3f;

    private bool isStunned = false;
    private Coroutine stunCoroutine;

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
        scaleX = transform.localScale.x;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

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
        if (target == null)
            return;

        // Do not move while stunned.
        if (Time.time < stunEndTime)
            return;

        float distToPlayer =
            Vector2.Distance(transform.position, target.position);

        if (distToPlayer > attackRange)
        {
            currentPath.Clear();
            pathIndex = 0;
            return;
        }

        if (pathfinder == null)
            return;

        // Recalculate the path periodically as the player moves.
        if (Time.time >= nextPathUpdate)
        {
            currentPath = pathfinder.FindPath(
                transform.position,
                target.position
            );

            pathIndex = currentPath.Count > 1 ? 1 : 0;
            nextPathUpdate = Time.time + pathUpdateInterval;
        }

        // Follow the calculated path.
        if (pathIndex < currentPath.Count)
        {
            Vector2 waypoint = currentPath[pathIndex];

            transform.position = Vector2.MoveTowards(
                transform.position,
                waypoint,
                speed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, waypoint)
                <= waypointDistance)
            {
                pathIndex++;
            }
        }

        // Face the direction of travel.
        if (pathIndex < currentPath.Count)
        {
            float directionX =
                currentPath[pathIndex].x - transform.position.x;

            if (Mathf.Abs(directionX) > 0.01f)
            {
                transform.localScale = new Vector2(
                    directionX > 0 ? scaleX : -scaleX,
                    transform.localScale.y
                );
            }
        }
    }


    public void TakeHit()
    {
        if (target == null || rb2D == null)
            return;

        // Calculate the direction away from the player.
        Vector2 direction =
            ((Vector2)transform.position -
             (Vector2)target.position).normalized;

        // Start or refresh the stun timer.
        stunEndTime = Time.time + stunDuration;

        // Apply knockback.
        rb2D.linearVelocity = direction * knockbackForce;

        Debug.Log("Enemy stunned for " + stunDuration + " seconds.");
    }

    private IEnumerator StunAndKnockback(Vector2 direction)
    {
        isStunned = true;

        rb2D.linearVelocity = direction * knockbackForce;

        yield return new WaitForSeconds(stunDuration);

        rb2D.linearVelocity = Vector2.zero;
        isStunned = false;
        stunCoroutine = null;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAttacking = true;

            if (gameHandler != null)
            {
                gameHandler.playerGetHit(damage);
            }
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