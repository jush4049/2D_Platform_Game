using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    void Start()
    {
        // ¹è°æ À½¾Ç
        AudioSource music = GetComponent<AudioSource>();
        if (Settings.canMusic) music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
