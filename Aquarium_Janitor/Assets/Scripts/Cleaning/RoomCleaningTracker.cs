using UnityEngine;


public class RoomCleaningTracker : MonoBehaviour
{
    public CleanableTank[] roomTanks;
    public int TotalTanks => roomTanks == null ? 0 : roomTanks.Length;
    public int CleanedTanks
    {
        get
        {
            int cleaned = 0;
            if (roomTanks != null)
                foreach (CleanableTank tank in roomTanks)
                    if (tank != null && tank.IsClean) cleaned++;
            return cleaned;
        }
    }
    public bool RoomComplete => TotalTanks > 0 && CleanedTanks == TotalTanks;

    public CleanableTank NearestTank(Vector2 position, float range)
    {
        CleanableTank nearest = null;
        float closest = range;
        if (roomTanks == null) return null;
        foreach (CleanableTank tank in roomTanks)
        {
            if (tank == null || !tank.isActiveAndEnabled) continue;
            float distance = tank.DistanceTo(position);
            if (distance <= closest)
            {
                closest = distance;
                nearest = tank;
            }
        }
        return nearest;
    }
}
