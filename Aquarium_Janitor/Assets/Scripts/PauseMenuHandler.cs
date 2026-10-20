using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuHandler : MonoBehaviour {

        public static bool GameisPaused = false;
        public GameObject pauseMenuUI;
        public AudioMixer mixer;
        public AnimationCurve curveMove = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        float preOffsetPos;
        float startOffset = 500f;
        public static float volumeLevel = 1.0f;
        private Slider sliderVolumeCtrl;
        float elapsedMove = 0f;
        Vector3 startButtonPos;
         

        void Awake(){
                SetLevel (volumeLevel);
                GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
                if (sliderTemp != null){
                        sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
                        sliderVolumeCtrl.value = volumeLevel;
                }
        }

        void Start(){
              preOffsetPos = pauseMenuUI.transform.position.y; //save the destination
              startButtonPos = transform.position;
              startButtonPos.y += startOffset;
              pauseMenuUI.transform.position = startButtonPos; //set the start position

              pauseMenuUI.SetActive(false);
              GameisPaused = false;
        }

        void Update(){
                if (Input.GetKeyDown(KeyCode.P)){
                        if (GameisPaused){ Resume(); }
                        else{ Pause(); }
                }
        }

        public void Pause(){
                if (!GameisPaused){
                        pauseMenuUI.SetActive(true);
                        Time.timeScale = 0f;
                        GameisPaused = true;
                        //unlock cursor
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                }
             else { Resume(); }
             //NOTE: This function is for the pause button
        }

        public void Resume() {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                GameisPaused = false;
        }

        public void SetLevel (float sliderValue){
                mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
                volumeLevel = sliderValue;
        }

        public void MainMenu(){
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
                // Please also reset all static variables here, for new games!
        }

        public void QuitGame(){
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
        }

        void FixedUpdate () {
                     // Tween Move:
                     if(startButtonPos.y >= preOffsetPos){
                            startButtonPos.y -= curveMove.Evaluate(elapsedMove) * startOffset;
                            pauseMenuUI.transform.position = startButtonPos;
                     }
                    
                     elapsedMove += (Time.deltaTime / 10f);
              
       }
}