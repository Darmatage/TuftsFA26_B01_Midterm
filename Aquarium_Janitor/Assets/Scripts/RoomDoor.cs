
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private static float nextTeleportTime = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Door trigger entered: " + gameObject.name);

        Rigidbody2D player = other.attachedRigidbody;

        if (player == null || !player.CompareTag("Player"))
            return;

        if (destination == null)
        {
            Debug.LogError(name + ": Destination is not assigned!");
            return;
        }

        if (Time.time < nextTeleportTime)
            return;

        nextTeleportTime = Time.time + 0.5f;

        Debug.Log(name + ": Teleporting player to " + destination.name);

        player.linearVelocity = Vector2.zero;
        player.angularVelocity = 0f;
        player.position = destination.position;
        player.transform.position = destination.position;

        Physics2D.SyncTransforms();
    }
}
