using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))] // AudioSource �ڵ� �߰�

public class FootStep : MonoBehaviour
{
    public AudioClip[] clips; // ����� Ŭ���� �ν����Ϳ��� �Ҵ��ϵ��� public �迭�� �ۼ�
    AudioSource footAudio;

    void Start()
    {
        footAudio = GetComponent<AudioSource>();
    }

    // Foot Step Sound <- Player���� ȣ��
    void PlaySound (int kind)
    {
        footAudio.clip = clips[kind]; // ������ �´� ����� Ŭ�� ����
        // footAudio.Play();
        if (Settings.canSound) footAudio.Play();
    }

}
