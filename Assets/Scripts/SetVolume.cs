using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMusic(float sliderValue)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSfx(float sliderValue)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sliderValue) * 20);
    }
}
