using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public Transform exitPoint;

    [Header("Engines")]
    public ParticleSystem engineParticles;
    public Light engineLight;
    public AudioSource engineSound;
    public float currentThrust = 0;
    public float maxThrust = 10f;
    public float maxVelocity = 30f;

    [Header("Control")]
    public float rotateSpeed = 5f;
    public float thrustChangeSpeed = 5f;
    private float thrustChange = 0; 

    private Vector3 shipMovement;
    private Rigidbody rb;

    private float leaveDelay = 0.5f;
    private float leaveDelayInit;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        leaveDelayInit = leaveDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.player.isInShip)
        {
            GetShipMovement();
            GetShipThrust();
            ChangeThrust();                
        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.player.isInShip)
        {
            MoveShip();
            SlowShip();


            CheckIfLeftShip();
            

            leaveDelay = Mathf.Clamp(leaveDelay - Time.fixedDeltaTime, -1, leaveDelayInit);
        }
    }



    void CheckIfLeftShip()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (leaveDelay <= 0)
            {
                leaveDelay = 0.5f;
                GameManager.instance.player.isInShip = false;
            }
        }

        if (GameManager.instance.player.isInShip == false)
        {
            GetComponentInChildren<ShipCamera>().enabled = false;
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;

            GameManager.instance.player.GetComponent<PlayerMovement>().enabled = true;
            GameManager.instance.player.GetComponent<CapsuleCollider>().enabled = true;
            GameManager.instance.playerCamera.GetComponent<CameraRotation>().enabled = true;
            GameManager.instance.playerCamera.GetComponent<Camera>().enabled = true;
            GameManager.instance.playerCamera.GetComponent<AudioListener>().enabled = true;
            //GameManager.instance.player.GetComponentInChildren<Animator>().gameObject.SetActive(true);
            GameManager.instance.player.transform.GetChild(1).GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

            GameManager.instance.player.mainHand.gameObject.SetActive(true);

            GameManager.instance.player.transform.position = exitPoint.position;

            GetComponent<ShipController>().enabled = false;
        }
    }

    void ChangeThrust()
    {
        currentThrust = Mathf.Clamp(currentThrust + Time.deltaTime * thrustChange * thrustChangeSpeed, 0, maxThrust);

        if (currentThrust != 0)
        {
            if (!engineParticles.isPlaying)
            {
                engineParticles.Play();
                engineLight.enabled = true;
            }

            if (!engineSound.isPlaying)       
                engineSound.Play();
            

            engineParticles.startLifetime = Mathf.Clamp(currentThrust / maxThrust * 2, 0.5f, 2f);
        }
        else 
        {
            if (engineParticles.isPlaying)
            {
                engineParticles.Stop();
                engineLight.enabled = false;
            }

            if (engineSound.isPlaying)
            {
                engineSound.Stop();
            }
        }
    }

    void GetShipThrust()
    {
        thrustChange = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            thrustChange = 1;

        } else if (Input.GetKey(KeyCode.LeftShift))
        {
            thrustChange = -1;
        }
    }

    void Fly()
    {
        rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * currentThrust, ForceMode.VelocityChange);

        var velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, maxVelocity);
        velocity.y = Mathf.Clamp(velocity.y, -maxVelocity, maxVelocity);
        velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);

        rb.velocity = velocity;
    }



    void SlowShip()
    {
        if (currentThrust == 0 && rb.velocity.magnitude > 0)
        {
            rb.velocity = rb.velocity - rb.velocity * Time.deltaTime;
            if (rb.velocity.magnitude <= 0.1f)
            {
                rb.velocity = Vector3.zero;
            }
        }

        if (rb.angularVelocity.magnitude > 0)
        {
            rb.angularVelocity = rb.angularVelocity - rb.angularVelocity * Time.deltaTime;
            if (rb.angularVelocity.magnitude <= 0.1f)
            {
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    void MoveShip()
    {
        rb.AddRelativeTorque(shipMovement * Time.fixedDeltaTime * rotateSpeed, ForceMode.VelocityChange);
        Fly();
    }

    void GetShipMovement()
    {
        shipMovement.z = -Input.GetAxis("Horizontal");
        shipMovement.x = Input.GetAxis("Vertical");  
    }


}
