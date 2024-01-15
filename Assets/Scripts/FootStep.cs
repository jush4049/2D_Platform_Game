using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))] // AudioSource �ڵ� �߰�

public class FootStep : MonoBehaviour
{
    public AudioClip[] clips; // ����� Ŭ���� �ν����Ϳ��� �Ҵ��ϵ��� public �迭�� �ۼ�
    AudioSource footAudio;
    public GameObject score;  // Prefab Score

    void Start()
    {
        footAudio = GetComponent<AudioSource>();
    }

    // Foot Step Sound <- Player���� ȣ��
    void PlaySound (int kind)
    {
        footAudio.clip = clips[kind]; // ������ �´� ����� Ŭ�� ����
        if (Settings.canSound) footAudio.Play();
    }

    void SetDamage (int damage)
    {
        int idx = (damage < 0) ? 4 : 3;
        footAudio.clip = clips[idx];
        if (Settings.canSound) footAudio.Play();

        if (damage > 0)
        {
            Vector3 pos = transform.position + new Vector3(0, 2, 0);
            GameObject score = Instantiate(Resources.Load("Score")) as GameObject;

            score.SendMessage("SetHP", -damage);
            score.transform.position = pos;
        }
    }

}
