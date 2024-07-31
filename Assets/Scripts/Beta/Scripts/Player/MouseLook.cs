// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script serves as the code for the player to look around.
// Dependencies: Level State
// Main Contributors: Olivia Lazar
public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 150;

    private Transform playerTF;
    private float pitch;

    // Start is called before the first frame update
    void Start()
    {
        playerTF = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // While the level is active
        if(LevelManager.IsLevelActive())
        {
            float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;  

            playerTF.Rotate(Vector3.up * moveX);

            pitch -= moveY;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
            transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }
}
