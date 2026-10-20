using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWin : MonoBehaviour
{
    void Update()
    {
        CleanableTank[] tanks = FindObjectsByType<CleanableTank>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (tanks.Length == 0) return;

        foreach (CleanableTank tank in tanks)
        {
            if (!tank.IsClean) return;
        }

        SceneManager.LoadScene("EndWin");
    }
}
