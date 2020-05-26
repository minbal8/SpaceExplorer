using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour
{
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    public void Step1()
    {
        Debug.Log("Step1");
        source.Play();
    }

    public void Step2()
    {
        source.Play();
        Debug.Log("Step2");
    }
}
