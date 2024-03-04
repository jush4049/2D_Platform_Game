using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))] // AudioSource 자동 추가

public class SFXAudio : MonoBehaviour
{
    public AudioClip[] clips; // 오디오 클립을 인스펙터에서 할당하도록 public 배열로 작성
    AudioSource audioSource;
    GameObject score;  // Prefab Score

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // SFX Sound <- Player에서 호출
    void PlaySound(int kind)
    {
        audioSource.clip = clips[kind]; // 종류에 맞는 오디오 클립 설정
        if (Settings.canSound) audioSource.Play();
    }

    void SetDamage (int damage)
    {
        int idx = (damage < 0) ? 4 : 3;
        audioSource.clip = clips[idx];
        if (Settings.canSound) audioSource.Play();

        if (damage > 0)
        {
            Vector3 pos = transform.position + new Vector3(0, 2, 0);
            GameObject score = Instantiate(Resources.Load("Score")) as GameObject;

            score.SendMessage("SetHP", -damage);
            score.transform.position = pos;
        }
    }
}
