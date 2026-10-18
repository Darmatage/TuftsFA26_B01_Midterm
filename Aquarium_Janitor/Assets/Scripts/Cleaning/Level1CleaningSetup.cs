using UnityEngine;

public class Level1CleaningSetup : MonoBehaviour
{
    public PlayerTankCleaner player;
    public CleaningHUD hud;
    public RoomCleaningTracker room1;
    public RoomCleaningTracker room2;
    public Vector2 room1Center;
    public Vector2 room2Center;

    void Start()
    {
        RefreshRoom();
    }

    void Update()
    {
        RefreshRoom();
    }

    public void RefreshRoom()
    {
        if (player == null || hud == null) return;

        float distanceToRoom1 = Vector2.Distance(player.transform.position, room1Center);
        float distanceToRoom2 = Vector2.Distance(player.transform.position, room2Center);

        // Use the cleaning count and tanks for the room closest to the player.
        if (distanceToRoom1 < distanceToRoom2)
        {
            player.room = room1;
            hud.room = room1;
        }
        else
        {
            player.room = room2;
            hud.room = room2;
        }
    }
}
