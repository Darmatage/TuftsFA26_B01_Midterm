using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CleaningHUD : MonoBehaviour
{
    public RoomCleaningTracker room;
    public PlayerTankCleaner player;
    public TMP_Text counterText;
    public TMP_Text statusText;
    public TMP_Text promptText;
    public TMP_Text progressText;
    public Image progressFill;
    public GameObject progressGroup;

    void Update()
    {
        if (room == null || player == null) return;

        UpdateStatsDisplay();
        UpdateCleaningPrompt();

        progressGroup.SetActive(player.CanClean);
        progressFill.rectTransform.anchorMax = new Vector2(player.Progress, 1f);
        progressFill.rectTransform.offsetMax = Vector2.zero;
        progressText.text = Mathf.RoundToInt(player.Progress * 100f) + "%";
    }

    public void UpdateStatsDisplay()
    {
        counterText.text = "TANKS CLEANED  " + room.CleanedTanks + " / " + room.TotalTanks;

        if (room.RoomComplete)
        {
            statusText.text = "ROOM COMPLETE";
        }
        else
        {
            statusText.text = "Clean every aquarium in this room";
        }
    }

    void UpdateCleaningPrompt()
    {
        if (player.NearbyTank == null)
        {
            if (room.RoomComplete)
            {
                promptText.text = "All tanks are sparkling clean!";
            }
            else
            {
                promptText.text = "Move close to a dirty tank to clean it";
            }
        }
        else if (player.NearbyTank.IsClean)
        {
            promptText.text = player.NearbyTank.tankName + "  -  Already clean";
        }
        else if (player.IsCleaning)
        {
            promptText.text = "Cleaning " + player.NearbyTank.tankName + "... Keep holding C";
        }
        else
        {
            promptText.text = "Hold [C] for 2 seconds to clean " + player.NearbyTank.tankName;
        }
    }
}
