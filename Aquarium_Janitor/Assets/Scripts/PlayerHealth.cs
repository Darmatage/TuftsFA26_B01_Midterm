
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

    private Rigidbody2D rb2D;
    private bool isDead = false;
    private float invulnerableUntil = 0f;
    private Coroutine knockbackCoroutine;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UpdateHealthDisplay();
        knockbackMotor = GetComponent<KnockbackMotor>();
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

        // Start the 3-second invulnerability period.
        invulnerableUntil = Time.time + iframeDuration;

        // Knock the player away from the enemy.
        if (knockbackMotor != null)
        {
            Vector2 direction =
                (rb2D.position - sourcePosition).normalized;

            knockbackMotor.ApplyKnockback(direction);
        }
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
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

        GameHandler.lastLevelDied = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(gameOverSceneName);
    }
}
