using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))] // AudioSource 자동 추가

public class FootStep : MonoBehaviour
{
    public AudioClip[] clips; // 오디오 클립을 인스펙터에서 할당하도록 public 배열로 작성
    AudioSource footAudio;

    void Start()
    {
        footAudio = GetComponent<AudioSource>();
    }

    // Foot Step Sound <- Player에서 호출
    void PlaySound (int kind)
    {
        footAudio.clip = clips[kind]; // 종류에 맞는 오디오 클립 설정
        // footAudio.Play();
        if (Settings.canSound) footAudio.Play();
    }

}
