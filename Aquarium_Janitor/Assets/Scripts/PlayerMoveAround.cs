using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerMoveAround : MonoBehaviour {

      //public Animator anim;
      //public AudioSource WalkSFX;
      private Rigidbody2D rb2D;
      private bool FaceRight = true; // determine which way player is facing.
      public static float runSpeed = 10f;
      public float startSpeed = 10f;
      public bool isAlive = true;
      Vector3 movement;

      void Start(){
           //anim = gameObject.GetComponentInChildren<Animator>();
           rb2D = transform.GetComponent<Rigidbody2D>();
      }

      void Update(){
            // INPUTS: Horizontal axis: [a] / left arrow is -1, [d] / right arrow is 1
            // Vertical axis: [w] / up arrow, [s] / down arrow
           movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

           if (isAlive == true){
                  // Animation and audio:
                  if ((movement.x != 0) || (movement.y != 0)){
                  //     anim.SetBool ("Walk", true);
                  //     if (!WalkSFX.isPlaying){
                  //           WalkSFX.Play();
                  //     }
                  } else {
                  //     anim.SetBool ("Walk", false);
                  //     WalkSFX.Stop();
                 }

                  // Turning. Reverse if input is moving the Player right and Player faces left.
                 if ((movement.x <0 && !FaceRight) || (movement.x >0 && FaceRight)){
                        playerTurn();
                  }
            }
      }

      void FixedUpdate(){
           if (isAlive == true){
                  // Move the character (all physics goes into FixedUpdate())
                  rb2D.position = transform.position + movement * runSpeed * Time.fixedDeltaTime;
            }
      }

      private void playerTurn(){
            // NOTE: Switch player facing label
            FaceRight = !FaceRight;

            // NOTE: Multiply player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
      }
}