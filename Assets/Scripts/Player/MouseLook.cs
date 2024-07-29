using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script serves as the code for the player to look around.
// Main Contributors: Olivia Lazar
public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100;

    private Transform playerBody;
    private float pitch;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // While the level is active
        if(LevelManager.IsLevelActive())
        {
            float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;  

            playerBody.Rotate(Vector3.up * moveX);

            pitch -= moveY;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
            transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }
}
