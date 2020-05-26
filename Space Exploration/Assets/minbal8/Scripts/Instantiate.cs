using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    public GameObject obj;
    public GameObject loc;

    float timer = 1f;
    AudioSource audioSource;
    Transform cam;
    private void Start()
    {
        cam = transform.parent;
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Fire1") && timer <= 0)
        {
            timer = 1f;
            audioSource.Play();

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.layer == 10)
                {
                    var enemy = hit.transform.GetComponent<ExplodingEnemy>();
                    int dmg = Random.Range(20, 30);
                    Debug.Log(dmg);
                    enemy.TakeDamage(dmg);

                }
            }

        }

    }

}
