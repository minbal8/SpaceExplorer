using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCamera : MonoBehaviour
{
    public Transform ship;
    
    [Header("Camera settings")]
    public Camera shipCamera;
    public float cameraMovementSensetivity = 100f;
    public float cameraZoomSensetivity = 50f;

    public float zoomDistance = 20f;
    public float minZoomDistance = 10f;
    public float maxZoomDistance = 100f;
    //private Vector3 cameraMovement;
    private Vector3 offsetX;
    private Vector3 offsetY;

    private Vector3 offset;
    private float yOffset = 0;
    float x = 0.0f;
    float y = 0.0f;


    private float cameraZoom;
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 angles = shipCamera.transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * cameraMovementSensetivity * zoomDistance * Time.smoothDeltaTime * 0.2f;
        y -= Input.GetAxis("Mouse Y") * cameraMovementSensetivity * Time.smoothDeltaTime;

        y = ClampAngle(y, -480, 480);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        zoomDistance = Mathf.Clamp(zoomDistance - Input.GetAxis("Mouse ScrollWheel") * 15f, minZoomDistance, maxZoomDistance);

        RaycastHit hit;
        if (Physics.Linecast(ship.position, shipCamera.transform.position, out hit))
        {
            zoomDistance -= hit.distance;
        }

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -zoomDistance);
        Vector3 position = rotation * negDistance + ship.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
