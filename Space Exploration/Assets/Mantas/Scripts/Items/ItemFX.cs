using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ItemFX : MonoBehaviour
{

    public AudioClip pickupFX;

    void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(pickupFX, transform.position);
    }
}
