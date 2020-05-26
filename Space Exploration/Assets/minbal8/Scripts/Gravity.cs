using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Vector3 targetVelocity;
    private Rigidbody rBody;
    public float moveSpeed;
    public float maxVelocityChange;
    public bool Grounded = false;

    public bool GravityEnabled = false;
    public float gravitySize;
    private Vector3 gravityVector;

    public LayerMask groundMask;
    private PlanetScript Planet;

    private Vector3 velocityChange;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        Move();
        rBody.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    

    private void Move()
    {
        velocityChange = rBody.velocity;

        calculateGravity();
        velocityChange += gravityVector;
    }

    private void calculateGravity()
    {

        if (!GravityEnabled)
        {
            gravityVector = Vector3.zero;
            return;
        }
        if (Grounded)
        {
            gravityVector = -transform.up;
        }
        if (Planet)
        {
            Planet.Rotate(transform);
        }
        gravityVector -= gravitySize * transform.up * Time.fixedDeltaTime;

    }


    void OnTriggerEnter(Collider theCollision)
    {
        if (theCollision.gameObject.tag == "GravitySphere")
        {
            Planet = theCollision.gameObject.GetComponentInChildren<PlanetScript>();
            rBody.velocity = Vector3.zero;
            GravityEnabled = true;
            transform.SetParent(theCollision.transform);
        }
    }

    void OnTriggerExit(Collider theCollision)
    {
        if (theCollision.gameObject.tag == "GravitySphere")
        {
            Planet = null;
            rBody.useGravity = false;
            rBody.velocity = Vector3.zero;
            GravityEnabled = false;
            transform.SetParent(null);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Grounded = false;
        }
    }
}
