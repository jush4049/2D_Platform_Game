using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetSFXVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider slider;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            slider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            slider.value = 0.5f;
        }
    }

    void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float sfx_value)
    {
        PlayerPrefs.SetFloat("sfxVolume", sfx_value);
    }

    public void SetSfx(float sliderValue)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sliderValue) * 20);
    }
}
