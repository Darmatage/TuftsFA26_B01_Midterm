using UnityEngine;

public class WaterPuddle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Water trigger entered by: " + other.name);

        PlayerMoveAround player =
            other.GetComponentInParent<PlayerMoveAround>();

        if (player != null)
        {
            player.EnterWater();
            Debug.Log("Player entered water.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Water trigger exited by: " + other.name);

        PlayerMoveAround player =
            other.GetComponentInParent<PlayerMoveAround>();

        if (player != null)
        {
            player.ExitWater();
            Debug.Log("Player exited water.");
        }
    }
}