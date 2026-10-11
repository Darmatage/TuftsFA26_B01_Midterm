
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private static float nextTeleportTime = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D player = other.attachedRigidbody;

        if (player == null || !player.CompareTag("Player"))
            return;

        if (destination == null)
            return;

        // Prevent rapid teleporting between doors.
        if (Time.time < nextTeleportTime)
            return;

        nextTeleportTime = Time.time + 0.5f;

        player.linearVelocity = Vector2.zero;
        player.angularVelocity = 0f;

        player.position = destination.position;
        player.transform.position = destination.position;

        Physics2D.SyncTransforms();
    }
}
