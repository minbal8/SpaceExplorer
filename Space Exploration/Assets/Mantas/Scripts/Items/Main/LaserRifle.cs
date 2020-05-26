using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRifle : MonoBehaviour
{
    public float laserLength = 15f;
    public Transform laserPoint;
    public Transform directionPoint;
    public LineRenderer laser;
    public GameObject laserHitParticles;

    public AudioClip laserAudio;
    public AudioSource audioSource;

    public float minDamage = 10;
    public float maxDamage = 13;

    private bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isShooting && !GameManager.instance.onInventory)
        {
            Ray ray = new Ray(laserPoint.position, directionPoint.position - laserPoint.position);
            RaycastHit hit;
            Debug.DrawRay(laserPoint.position, (directionPoint.position - laserPoint.position).normalized, Color.red);

            if (Physics.Raycast(ray, out hit, laserLength))
            {
                laser.SetPosition(1, new Vector3(0, -hit.distance, 0));

                ParticleSystem p = Instantiate(laserHitParticles, hit.point, Quaternion.Euler(hit.normal)).GetComponentInChildren<ParticleSystem>();
                Destroy(p.transform.parent.gameObject, p.main.duration);

                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyController enemy = hit.collider.gameObject.GetComponent<EnemyController>();

                    enemy.TakeDamage(Random.Range(minDamage, maxDamage) * Time.fixedDeltaTime);
                }

                if (hit.collider.gameObject.CompareTag("Meteor"))
                {
                    Meteor meteor = hit.collider.gameObject.GetComponent<Meteor>();


                    meteor.TakeDamage(Random.Range(minDamage, maxDamage) * Time.fixedDeltaTime);
                }

                if (hit.collider.gameObject.layer == 8)
                {
                    var enemy = hit.collider.gameObject.GetComponent<ExplodingEnemy>();
                    enemy.TakeDamage(Random.Range(minDamage, maxDamage) * Time.fixedDeltaTime);
                }

            }
            else
            {
                laser.SetPosition(1, new Vector3(0, -laserLength, 0));
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isShooting = true;

            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(laserAudio);
        }
        else
        {
            isShooting = false;

            if (audioSource.isPlaying)
                audioSource.Stop();

            if (laser.GetPosition(1) != Vector3.zero)
            {
                laser.SetPosition(1, Vector3.zero);
            }
        }
    }
}
