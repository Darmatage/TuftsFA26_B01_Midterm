
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private static float nextTeleportTime = 0f;

    // Unity can keep static fields when Enter Play Mode disables domain reload.
    // The game clock restarts, so a cooldown from the last run must not survive.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetTeleportCooldown()
    {
        nextTeleportTime = 0f;
    }

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
