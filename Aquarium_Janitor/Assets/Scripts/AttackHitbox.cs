using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayers.value &
            (1 << other.gameObject.layer)) == 0)
            return;

        Debug.Log("Attack hit: " + other.name);

        // Add enemy damage logic here.
    }
}