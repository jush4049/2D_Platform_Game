using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))] // AudioSource �ڵ� �߰�

public class SFXAudio : MonoBehaviour
{
    public AudioClip[] clips; // ����� Ŭ���� �ν����Ϳ��� �Ҵ��ϵ��� public �迭�� �ۼ�
    AudioSource audioSource;
    GameObject score;  // Prefab Score

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // SFX Sound <- Player���� ȣ��
    void PlaySound(int kind)
    {
        audioSource.clip = clips[kind]; // ������ �´� ����� Ŭ�� ����
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
