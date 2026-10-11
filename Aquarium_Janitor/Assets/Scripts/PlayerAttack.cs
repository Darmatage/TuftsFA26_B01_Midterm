
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float cooldownTime = 3f;

    private float nextAttackTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) &&
            Time.time >= nextAttackTime)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        if (attackPrefab == null)
        {
            Debug.LogError("Attack prefab is not assigned!");
            return;
        }

        nextAttackTime = Time.time + cooldownTime;

        GameObject attack = Instantiate(
            attackPrefab,
            transform.position,
            Quaternion.identity
        );

        Destroy(attack, attackDuration);
    }
}
