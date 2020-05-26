using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider effectsSlider;

    public void UpdateSound()
    {
        SetEffectsVolume(effectsSlider.value);
        SetMusicVolume(musicSlider.value);
    }

    public void SetEffectsVolume(float dB)
    {
        if (dB <= -50)
        {
            audioMixer.SetFloat("Music", -240);
            audioMixer.SetFloat("Bass", -240);

            return;
        }

        audioMixer.SetFloat("Effects", dB);
    }

    public void SetMusicVolume(float dB)
    {
        if(dB <= -50)
        {
            audioMixer.SetFloat("Music", -240);
            audioMixer.SetFloat("Bass", -240);

            return;
        }

        audioMixer.SetFloat("Music", dB);
        audioMixer.SetFloat("Bass", dB);
    }

}
