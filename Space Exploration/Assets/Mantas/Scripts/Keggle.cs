using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keggle : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "GroundLayer")
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }
}
