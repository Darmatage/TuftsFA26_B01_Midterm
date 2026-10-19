using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {

      public PlayerHealth playerHealth;
      public GameObject heart1;
      public GameObject heart2;

      public static int gotTokens = 0;
      public static bool stairCaseUnlocked = false;
      public static string lastLevelDied;

      void Update(){
            if (playerHealth == null || heart1 == null || heart2 == null) return;

            updateStatsDisplay();
      }

      public void updateStatsDisplay(){
            heart1.SetActive(playerHealth.currentHealth >= 1);
            heart2.SetActive(playerHealth.currentHealth >= 2);
      }

      public void playerGetTokens(int newTokens){
            gotTokens += newTokens;
      }

      public void StartGame() {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level1");
      }

      public void MainMenu() {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
      }

      public void ReplayLastLevel() {
            Time.timeScale = 1f;
            SceneManager.LoadScene(lastLevelDied);
      }

      public void QuitGame() {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
      }

      public void Credits() {
            SceneManager.LoadScene("Credits");
      }
}
