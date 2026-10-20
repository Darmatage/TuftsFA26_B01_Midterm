using UnityEngine;

public class PlayerMoveAround : MonoBehaviour
{
    [Header("Water Movement")]
    public float waterSpeedMultiplier = 0.5f;

    private int waterCount = 0;
    private Rigidbody2D rb2D;
    private PlayerHealth playerHealth;

    private bool FaceRight = true;
    public float runSpeed = 5f;
    public float startSpeed = 10f;
    public bool isAlive = true;

    private Vector2 movement;

	public AudioSource WalkMetalSFX;
	public AudioSource WalkWaterSFX;
	AudioSource WalkSFX;
	public bool isInWater = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
		WalkSFX = WalkMetalSFX;
    }

    void Update()
    {
        if (!isAlive){
            movement = Vector2.zero;
            return;
        }
		else {
			//audio SFX	
			if (isInWater){WalkSFX = WalkWaterSFX;}
			else {WalkSFX = WalkMetalSFX;}
			if ((movement.x != 0) || (movement.y != 0)){
				if (!WalkSFX.isPlaying){WalkSFX.Play();}
			} else {WalkSFX.Stop();}
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

        float currentSpeed = runSpeed;

        if (waterCount > 0)
        {
            currentSpeed *= waterSpeedMultiplier;
        }

        rb2D.MovePosition(
            rb2D.position + movement * currentSpeed *
            Time.fixedDeltaTime
        );
    }

    public void EnterWater()
    {
        waterCount++;
    }

    public void ExitWater()
    {
        waterCount = Mathf.Max(0, waterCount - 1);
    }

    private void playerTurn()
    {
        FaceRight = !FaceRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}