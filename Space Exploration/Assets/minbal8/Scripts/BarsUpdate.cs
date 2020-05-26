using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarsUpdate : MonoBehaviour
{
    public PlayerMovement playerMovement;


    public float healthChange = 10;
    public float oxygenChange = 10;

    public float health = 100;
    public float oxygen = 100;

    public Slider OxygenSlider;
    public Slider HealthSlider;


    public AudioClip healthDmg;
    public AudioClip breatheOut;
    public AudioClip death;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private int lastBreath = 0;
    private float dmgPlayed = 2f;
    bool deathPlayed = false;

    private void Update()
    {

        if (dmgPlayed > 0)
        {
            dmgPlayed -= Time.deltaTime;
        }

        if (!playerMovement.GravityEnabled)
        {
            lastBreath = -1;
            oxygen -= Time.deltaTime * oxygenChange;
        }
        else
        {
            if (lastBreath == -1)
            {
                audioSource.PlayOneShot(breatheOut);
            }
            lastBreath = 1;
            oxygen += Time.deltaTime * oxygenChange;
        }

        oxygen = Mathf.Clamp(oxygen, 0, 100);
        if (oxygen == 0)
        {
            if (dmgPlayed <= 0)
            {
                dmgPlayed = 2f;
                audioSource.PlayOneShot(healthDmg);
            }
            health -= Time.deltaTime * healthChange;
        }
        health = Mathf.Clamp(health, 0, 100);
        if (health <= 0 && !deathPlayed)
        {
            deathPlayed = true;
            audioSource.PlayOneShot(death);
        }



        HealthSlider.value = health;
        OxygenSlider.value = oxygen;


    }


}
