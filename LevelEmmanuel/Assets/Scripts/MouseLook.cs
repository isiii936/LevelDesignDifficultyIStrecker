using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Transform mainCameraTransform;
    float verticalRotation;

    public float maxVerticalRotation;
    public float mouseSensitivity;

    float mouseX;
    float mouseY;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        
    }

    void Update()
    {
        if(GameController.instance.gamePlaying == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            //Rotates the player characters body 
            transform.Rotate(Vector3.up, mouseX);

            //Tilts the main camera vertically
            verticalRotation -= mouseY; //if inverted vertical rotation, add mousY instead of subtract
            verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalRotation, maxVerticalRotation);
            mainCameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }



    }

    public void SetRotation(float playerRotation, float cameraRotation)
    {
        transform.eulerAngles = new Vector3(0, playerRotation, 0);
        verticalRotation = cameraRotation;
    }
}