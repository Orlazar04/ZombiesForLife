using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author : Tarif Khan
// This script represents the player's movement.
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10;
    CharacterController controller;
    Vector3 input, moveDirection;
    public float jumpHeight = 10;
    public float gravity = 9.81f;
    public float airControl = 10;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        input *= speed;

        if (controller.isGrounded || Mathf.Approximately(transform.position.y, 0))
        {
            moveDirection = input;
            // we can jump
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }


        if (transform.position.y > 0)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }




        controller.Move(moveDirection * Time.deltaTime);

        if (transform.position.y <= 0)
        {
            Vector3 correctedPosition = transform.position;
            correctedPosition.y = 0;
            transform.position = correctedPosition;
        }
    }
}