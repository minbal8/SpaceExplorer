using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickables : MonoBehaviour
{
    public enum Effect { Destroy, PlaySound, MoveAway, Teleports };
    public Effect effect;
    public GameObject Particles;
    AudioSource audioData;
    Rigidbody rBody;
    public List<Vector3> locations;
    public float Force = 1f;

    private int currentLocation = 0;


    void Start()
    {
        audioData = GetComponent<AudioSource>();
        rBody = GetComponent<Rigidbody>();
        if (locations.Count > 0)
        {
            transform.localPosition = locations[currentLocation];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        if (other.gameObject.tag == "Player")
        {
            PlayParticles();
            switch (effect)
            {
                case Effect.Destroy:
                    EffectAsDestroy(other);
                    break;

                case Effect.PlaySound:
                    EffectAsPlaySound(other);
                    break;

                case Effect.MoveAway:
                    EffectAsMoveAway(other);
                    break;

                case Effect.Teleports:
                    EffectAsTeleports(other);
                    break;

                default:
                    {
                        Debug.Log("Error on OnTriggerEnter, Effect");
                        break;
                    }
            }
        }
    }

    private void EffectAsDestroy(Collider other)
    {
        var o = other.GetComponent<ScreenText>();
        o.AddCount();
        Destroy(gameObject);
    }
    private void EffectAsPlaySound(Collider other)
    {
        if (audioData != null)
        {
            audioData.Play(0);
        }
    }
    private void EffectAsMoveAway(Collider other)
    {
        if (rBody != null)
        {
            Vector3 dir = transform.position - other.transform.position;
            dir.y = 0;
            rBody.AddForce(Vector3.Normalize(dir), ForceMode.Impulse);

        }
    }
    private void EffectAsTeleports(Collider other)
    {
        currentLocation++;

        if (locations.Count == currentLocation)
        {
            currentLocation = 0;
        }
        transform.localPosition = locations[currentLocation];
    }

    void Update()
    {        
        if (rBody != null)
        {
            if (rBody.velocity.magnitude > 0.1)
            {
                rBody.velocity *= 0.995f;                
            }
            else
            {
                rBody.velocity *= 0;
            }
        }
    }



    private void PlayParticles()
    {
        if (Particles != null)
        {
            GameObject particle = Instantiate(Particles, transform.position, Quaternion.identity);
            particle.GetComponent<ParticleSystem>().Play();
        }
    }
}
