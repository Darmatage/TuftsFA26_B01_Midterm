using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attack touched: " + other.name);

        EnemyMoveHit enemy =
            other.GetComponentInParent<EnemyMoveHit>();

        if (enemy != null)
        {
            Debug.Log("Enemy hit!");
            enemy.TakeHit();
        }
    }
}