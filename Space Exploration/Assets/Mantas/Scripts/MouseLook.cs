using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed;

    public bool pointerEnabled = false;
    public KeyCode pointerToggleKey = KeyCode.LeftAlt;
    private float mouseX;
    private float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseToggle();
        MoveMouse();
    }

    public void GetMouseToggle()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            pointerEnabled = !pointerEnabled; 
    } 

    private void MoveMouse()
    {
        if (pointerEnabled)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            player.Rotate(new Vector3(0, player.rotation.x + mouseX * rotationSpeed * Time.deltaTime, 0));

            Camera.main.transform.eulerAngles -= new Vector3(mouseY, 0, 0);
        }
    }

}
