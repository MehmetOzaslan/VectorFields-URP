﻿using UnityEngine;

public class CameraOrbit : MonoBehaviour
{

    protected Transform cameraTransform;

    //Set in editor.
    public Transform target;

    //Target Rotation.
    protected Vector3 LocalRotation;
    protected float CameraDistance = 10f;

    public float MouseSensitivity = 4;
    public float ScrollSensitivity = 2f;

    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraControlDisabled = true;


    private void Start()
    {
        cameraTransform = transform;
    }

    private void Update()
    {


        // Exit Application  
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        // Lock cursor when middle mouse button pressed
        if (Input.GetMouseButtonDown(2))
        {
            CameraControlDisabled = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        // Unlock cursor when middle mouse button released
        if (Input.GetMouseButtonUp(2))
        {
            CameraControlDisabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }


    //Late Update to avoid popping issues.
    void LateUpdate()
    {

        //Zooming Input from our Mouse Scroll Wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

            //Easing with magic number
            ScrollAmount *= (CameraDistance * 0.3f);

            CameraDistance += ScrollAmount * -1f;

            CameraDistance = Mathf.Clamp(CameraDistance, 1.5f, 100f);
        }

        if (!CameraControlDisabled)
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

                //Clamp the y Rotation
                LocalRotation.y = Mathf.Clamp(LocalRotation.y, -90, 90);
            }

            //Rotation of the camera based on keyboard input.

            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                LocalRotation.x += Input.GetAxis("Horizontal") * MouseSensitivity;
                LocalRotation.y += Input.GetAxis("Vertical") * MouseSensitivity;

                LocalRotation.y = Mathf.Clamp(LocalRotation.y, -90, 90);
            }

        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(LocalRotation.y, LocalRotation.x, 0);
        target.rotation = Quaternion.Lerp(target.rotation, QT, Time.deltaTime * OrbitDampening);

        if (cameraTransform.localPosition.z != CameraDistance * -1f)
        {
            cameraTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(cameraTransform.localPosition.z, CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }
}


