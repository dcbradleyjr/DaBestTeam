using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool intertY;

    float rotX;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {

        if (!gameManager.instance.isPaused)
        {
            //get input 
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;

            //invert look
            if (intertY)

                rotX += mouseY;

            else

                rotX -= mouseY;

            //clamp the rotation on the X axis
            rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

            //rotate the camera on the X axis
            transform.localRotation = Quaternion.Euler(rotX, 0, 0);

            //rotate the player on the Y axis
            transform.parent.Rotate(Vector3.up * mouseX); 
        }
    }
}
