using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 2;
    public int currentHealth;

    private KnockbackMotor knockbackMotor;

    public bool IsBeingKnockedBack =>
        knockbackMotor != null && knockbackMotor.IsKnockedBack;

    [Header("Health Display")]
    public TMP_Text healthText;

    [Header("Game Over")]
    public string gameOverSceneName = "EndLose";

    [Header("Player Knockback")]
    public float knockbackDistance = 0.5f;
    public float knockbackSpeed = 4f;

    [Header("Invulnerability")]
    public float iframeDuration = 3f;

    [Header("Hit Flash")]
    public float flashInterval = 0.1f;
    [Range(0f, 1f)]
    public float flashOpacity = 0.8f;

    private Rigidbody2D rb2D;
    private bool isDead = false;
    private float invulnerableUntil = 0f;
    private Coroutine knockbackCoroutine;
    private Coroutine flashCoroutine;
    private SpriteRenderer[] spriteRenderers;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UpdateHealthDisplay();

        knockbackMotor = GetComponent<KnockbackMotor>();

        // Find the player's sprite renderers, including child objects.
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damage, Vector2 sourcePosition)
    {
        if (isDead || damage <= 0)
            return;

        // Ignore damage during invulnerability.
        if (Time.time < invulnerableUntil)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthDisplay();

        // Start the invulnerability period.
        invulnerableUntil = Time.time + iframeDuration;

        // Start flashing the player's sprite.
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine());

        // Knock the player away from the enemy.
        if (knockbackMotor != null && rb2D != null)
        {
            Vector2 direction =
                (rb2D.position - sourcePosition).normalized;

            knockbackMotor.ApplyKnockback(direction);
        }

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashRoutine()
    {
        bool isFaded = false;

        while (Time.time < invulnerableUntil)
        {
            isFaded = !isFaded;

            foreach (SpriteRenderer sprite in spriteRenderers)
            {
                if (sprite == null)
                    continue;

                Color color = sprite.color;
                color.a = isFaded ? flashOpacity : 1f;
                sprite.color = color;
            }

            yield return new WaitForSeconds(flashInterval);
        }

        // Restore full opacity when invulnerability ends.
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            if (sprite == null)
                continue;

            Color color = sprite.color;
            color.a = 1f;
            sprite.color = color;
        }

        flashCoroutine = null;
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text =
                "Health: " + currentHealth + " / " + maxHealth;
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("Player has died!");

        GameHandler.lastLevelDied =
            SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(gameOverSceneName);
    }
}