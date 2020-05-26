using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : MonoBehaviour
{

    private Vector3 targetVelocity;
    private Rigidbody rBody;
    public float moveSpeed;
    public float maxVelocityChange;
    public bool Grounded = false;

    public bool GravityEnabled = true;
    public float gravitySize;
    private Vector3 gravityVector;

    public LayerMask groundMask;
    private PlanetScript Planet;
    private Vector3 velocityChange;
    public Transform target;

    public bool canMove = true;
    public float health = 100;
    public GameObject Particles;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rBody = GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            canMove = false;
        }

    }

    void FixedUpdate()
    {
        Move();
        rBody.AddForce(velocityChange, ForceMode.VelocityChange);

        if (canMove && Vector3.Distance(target.position, transform.position) < 5)
        {
            Explode();
        }

    }

    private void Explode()
    {
        TakeDamage(100);
        if (Particles != null)
        {
            GameObject particle = Instantiate(Particles, transform.position, Quaternion.identity);
            particle.GetComponent<ParticleSystem>().Play();
        }

        GameManager.instance.player.TakeDamage(30);
        Destroy(gameObject);
    }

    private void Move()
    {
        if (canMove)
        {
            transform.LookAt(target);
            targetVelocity = transform.forward;
            targetVelocity *= moveSpeed;

            Vector3 velocity = rBody.velocity;
            velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
        }
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
