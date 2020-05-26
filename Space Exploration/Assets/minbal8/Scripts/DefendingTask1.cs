using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendingTask1 : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();


    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.forward * -200);
        }
    }

}
