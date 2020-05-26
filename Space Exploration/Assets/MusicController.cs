using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public List<AudioClip> clips;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        int rand = Random.Range(0, clips.Count);

        Debug.Log(clips[rand].name);
        audioSource.PlayOneShot(clips[rand]);

    }



}
