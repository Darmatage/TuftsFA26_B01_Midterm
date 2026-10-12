using System.Collections;
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

    void Start()
    {
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

        // Do not chase while stunned.
        if (Time.time < stunEndTime)
            return;

        float distToPlayer =
            Vector2.Distance(transform.position, target.position);

        if (distToPlayer <= attackRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

            if (target.position.x > transform.position.x)
            {
                transform.localScale = new Vector2(
                    scaleX, transform.localScale.y);
            }
            else
            {
                transform.localScale = new Vector2(
                    scaleX * -1, transform.localScale.y);
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