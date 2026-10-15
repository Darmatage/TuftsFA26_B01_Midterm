using UnityEngine;

public class PlayerMoveAround : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private PlayerHealth playerHealth;

    private bool FaceRight = true;
    public float runSpeed = 5f;
    public float startSpeed = 10f;
    public bool isAlive = true;

    private Vector2 movement;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (!isAlive)
        {
            movement = Vector2.zero;
            return;
        }

        if (playerHealth != null &&
            playerHealth.IsBeingKnockedBack)
        {
            movement = Vector2.zero;
            return;
        }

        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if ((movement.x < 0 && FaceRight) ||
            (movement.x > 0 && !FaceRight))
        {
            playerTurn();
        }
    }

    void FixedUpdate()
    {
        if (!isAlive ||
            (playerHealth != null &&
             playerHealth.IsBeingKnockedBack))
        {
            return;
        }

        rb2D.MovePosition(
            rb2D.position + movement * runSpeed *
            Time.fixedDeltaTime
        );
    }

    private void playerTurn()
    {
        FaceRight = !FaceRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}