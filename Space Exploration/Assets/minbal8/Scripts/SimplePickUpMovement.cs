using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickUpMovement : MonoBehaviour
{

    //adjust this to change speed
    float speed = 1f;
    //adjust this to change how high it goes
    float height = 10f;
    float offset;
    private void Start()
    {
        offset = transform.position.y;
    }
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = offset + Mathf.Sin(Time.time * speed) * height;
        //transform.position = pos;

        transform.Rotate(new Vector3(0, 1, 0) * Time.fixedDeltaTime);

    }
}
