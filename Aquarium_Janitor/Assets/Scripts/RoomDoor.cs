using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    public Transform destination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.attachedRigidbody;

            if (rb != null)
            {
                rb.position = destination.position;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                other.transform.position = destination.position;
            }
        }
    }
}