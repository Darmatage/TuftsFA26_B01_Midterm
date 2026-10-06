using UnityEngine;
using System;
using System.Collections;

public class PlayerMove_Rotate : MonoBehaviour {
      Rigidbody2D rb2D;
      public float moveSpeed = 5f;
      public float rotationSpeed = 720f;
      Vector3 moveDirection;
      float inputMagnitude;

      void Start(){
            rb2D = GetComponent<Rigidbody2D>();
      }

      void Update(){
            float horizontalInput = Input.GetAxis ("Horizontal");
            float verticalInput = Input.GetAxis ("Vertical");
            moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
            inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
            moveDirection.Normalize();

            if (moveDirection != Vector3.zero) {
                  Quaternion toRotation = Quaternion.LookRotation (Vector3.forward, moveDirection);
                  transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
      }

      void FixedUpdate(){
            rb2D.position = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
      }
}