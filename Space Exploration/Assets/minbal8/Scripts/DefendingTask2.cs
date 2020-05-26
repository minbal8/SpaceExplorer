using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendingTask2 : MonoBehaviour
{
    private Rigidbody rbody;
    private float force = 10;
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        rbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        force -= 1;
    }
}
