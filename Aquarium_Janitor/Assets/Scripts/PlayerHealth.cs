using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Health Display")]
    public TMP_Text healthText;

    [Header("Game Over")]
    public string gameOverSceneName = "GameOver";

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthDisplay();

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

        SceneManager.LoadScene(gameOverSceneName);
    }
}