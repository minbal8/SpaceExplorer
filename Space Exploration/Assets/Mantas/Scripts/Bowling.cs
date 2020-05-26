using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowling : MonoBehaviour
{

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Debug.Log("COLLISION!");
            Vector3 cur = transform.position;
            Vector3 player = collider.transform.position;
            cur.z = player.z = 0f;

            rb.AddRelativeForce(-(collider.transform.position - transform.position) * 10f * rb.mass, ForceMode.Impulse);

        }
    }

}
