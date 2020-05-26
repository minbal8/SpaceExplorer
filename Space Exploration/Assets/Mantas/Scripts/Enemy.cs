using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioSource audioSource;
    public ParticleSystem particleSystem;
    public float health = 100f;
    public bool triggered = false;

    void OnCollisionEnter(Collision collision)
    {
        //if (!triggered)
        //{            
        //    if (collision.gameObject.tag == "Player")
        //    {
        //        triggered = true;
        //        Debug.Log("Player touched me");
       
        //            audioSource.Play();

        //        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        //        player.score += 1f;

        //        Invoke("InstantiateParticles", audioSource.clip.length - 0.1f);
        //        Destroy(gameObject, audioSource.clip.length);
                
        //    }

        //}
    }

    void InstantiateParticles()
    {
        //Vector3 offset = new Vector3(0, 1f, 0);
        //ParticleSystem newParticleSystem = Instantiate(particleSystem, transform.position + offset, Quaternion.identity);
        //Destroy(newParticleSystem, newParticleSystem.main.duration);
    }
}
