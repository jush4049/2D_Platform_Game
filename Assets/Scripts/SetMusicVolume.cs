using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetMusicVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider slider;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            slider.value = PlayerPrefs.GetFloat("musicVolume");
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

    public void OnValueChanged(float music_value)
    {
        PlayerPrefs.SetFloat("musicVolume", music_value);
    }

    public void SetMusic(float sliderValue)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 20);
    }
}
