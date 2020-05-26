using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UIButtonSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip buttonHover;
    public AudioClip buttonClick;

    public void PlayHighlightSound()
    {
        audioSource.PlayOneShot(buttonHover);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }

}
