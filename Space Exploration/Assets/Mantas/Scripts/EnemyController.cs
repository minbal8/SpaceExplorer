using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;

    public float health = 25f; 

    public float movementSpeed = 12f;
    public float laserDistance = 5f;
    public float followDistance = 200f;
    public float maxLinearVelocity = 8f;

    public float minDamage = 10;
    public float maxDamage = 15;


    public Transform laserTransform1;
    public Transform laserTransform2;

    public LineRenderer laserRenderer1;
    public LineRenderer laserRenderer2;

    private AudioSource audioSource;
    public AudioClip laserSound;
    public AudioClip destroySound;
    public Transform destroyVFX;



    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.player.isInShip)
        {
            if (Vector3.Distance(transform.position, target.position) <= followDistance)
            {
                FollowTarget();

                if (Vector3.Distance(transform.position, target.position) <= laserDistance)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(laserSound);
                    }

                    //Shoot
                    RaycastHit hit1;
                    Ray ray1 = new Ray(laserTransform1.position, target.position - laserTransform1.position);

                    bool didHit1 = Physics.Raycast(ray1, out hit1, laserDistance);

                    Vector3[] positions1 = { laserTransform1.position, target.position };
                    laserRenderer1.SetPositions(positions1);

                    RaycastHit hit2;
                    Ray ray2 = new Ray(laserTransform2.position, target.position - laserTransform2.position);

                    bool didHit2 = Physics.Raycast(ray2, out hit2, laserDistance);

                    Vector3[] positions2 = { laserTransform2.position, target.position };
                    laserRenderer2.SetPositions(positions2);

                    if (didHit1 || didHit2)
                    {
                        Debug.Log("Got Hit");

                        GameManager.instance.player.TakeDamage(Random.Range(minDamage, maxDamage) * Time.fixedDeltaTime);

                    }

                }
                else
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }


                    laserRenderer1.SetPosition(0, Vector3.zero);
                    laserRenderer1.SetPosition(1, Vector3.zero);
                    laserRenderer2.SetPosition(0, Vector3.zero);
                    laserRenderer2.SetPosition(1, Vector3.zero);

                }

            }
        }
    }

    void FollowTarget()
    {
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) > laserDistance)
        {
            rb.AddRelativeForce(Vector3.forward * movementSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            if(rb.velocity.magnitude != 0f)
            {
                rb.velocity = rb.velocity - rb.velocity * Time.fixedDeltaTime;
            }
        }

        Vector3 newVelocity;

        newVelocity.x = Mathf.Clamp(rb.velocity.x, -maxLinearVelocity, maxLinearVelocity);
        newVelocity.y = Mathf.Clamp(rb.velocity.y, -maxLinearVelocity, maxLinearVelocity);
        newVelocity.z = Mathf.Clamp(rb.velocity.z, -maxLinearVelocity, maxLinearVelocity);

        rb.velocity = newVelocity;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
            Transform t = Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(t.gameObject, t.GetComponent<ParticleSystem>().main.duration);

            Destroy(gameObject);
        }
    }

}
