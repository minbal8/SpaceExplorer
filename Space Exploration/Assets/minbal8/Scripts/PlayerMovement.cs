using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector3 targetVelocity;
    private Rigidbody rBody;
    public float moveSpeed;
    public float maxVelocityChange;
    public bool Grounded = false;

    public bool GravityEnabled = true;
    public float SpeedOutsideOfGravity = 5.0f;
    public float gravitySize;
    public float jumpHeight = 2f;
    private Vector3 gravityVector;

    public LayerMask groundMask;
    public Animator CharacterAnim;
    public Animator RailgunAnim;
    private PlanetScript Planet;

    private Vector3 velocityChange;
    private float jumped = 1f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
       // Move();
        updateAnimator();
        if (jumped > 0)
        {
            jumped -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (jumped > 0)
        {
            jumped -= Time.fixedDeltaTime;
        }
        Move();
        rBody.AddForce(velocityChange, ForceMode.VelocityChange);

        if (!Planet)
            RotateTowards();
    }

    private void updateAnimator()
    {
        CharacterAnim.SetFloat("Speed", targetVelocity.magnitude);
        RailgunAnim.SetBool("Run", targetVelocity != Vector3.zero);

        CheckShooting();

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    private void Move()
    {
        float forward = Input.GetAxis("Vertical");
        float right = Input.GetAxis("Horizontal");

        targetVelocity = transform.forward * forward + transform.right * right;
        targetVelocity *= moveSpeed;

        Vector3 velocity = rBody.velocity;
        velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);

        calculateGravity();
        Jump();
        velocityChange += gravityVector;
    }

    private void calculateGravity()
    {

        if (!GravityEnabled)
        {
            gravityVector = Vector3.zero;
            return;
        }
        if (Grounded && jumped <= 0)
        {
            gravityVector = -transform.up;
        }
        if (Planet)
        {
            Planet.Rotate(transform);
        }
        gravityVector -= gravitySize * transform.up * Time.fixedDeltaTime;

    }

    private float CalculateJumpSpeed(float h, float gravity)
    {
        return Mathf.Sqrt(2 * h * gravity);
    }

    public void LaunchIntoAir(float height)
    {
        gravityVector += transform.up * CalculateJumpSpeed(height, gravitySize);
    }

    private void Jump()
    {

        if (GravityEnabled)
        {
            if (Input.GetButtonDown("Jump") && Grounded && jumped <= 0)
            {
                jumped = 1f;

                gravityVector += transform.up * CalculateJumpSpeed(jumpHeight, gravitySize);
            }
        }
        else
        {
            if (Input.GetButton("Jump"))
            {
                gravityVector = transform.up * SpeedOutsideOfGravity;
            }
            if (Input.GetButton("Fire3"))
            {
                gravityVector = -transform.up * SpeedOutsideOfGravity;
            }
        }
    }

    private void CheckShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RailgunAnim.SetTrigger("Shoot");
        }

        if (Input.GetButton("Fire2"))
        {
            RailgunAnim.SetBool("IsAiming", true);
        }
        else
        {
            RailgunAnim.SetBool("IsAiming", false);
        }
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

    private void RotateTowards()
    {
        var localRotation = transform.localRotation;
        localRotation.eulerAngles = new Vector3(0, localRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, Time.deltaTime);

    }





}
