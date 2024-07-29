using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script represents the player's movement.
// Main Contributors: Olivia Lazar
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpHeight = 2;
    public float airControl = 10;
    public float gravity = 9.81f;

    private CharacterController controller;
    private Vector3 input, moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {   
        // While the level is active
        if(LevelManager.IsLevelActive())
        {
            // Get movement input
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
            input *= moveSpeed;

            // Sprinting
            if(Input.GetKey(KeyCode.LeftShift))
            {
                input *= 2;
            }

            // Jump Controls
            if(controller.isGrounded)
            {
                moveDirection = input;
                if(Input.GetButton("Jump"))
                {
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                }
                else
                {
                    moveDirection.y = 0;
                }
            }
            else
            {
                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
            }

            // Gravity
            moveDirection.y -= gravity * Time.deltaTime;        

            // Move player
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}