using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        CheckPlayer(other.transform);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        CheckPlayer(other.transform);
    }

    void CheckPlayer (Transform other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("AddHP");
            MakeEnergy();
        }
    }

    void MakeEnergy()
    {
        // 점수 표시
        GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
        score.transform.position = transform.position;
        score.SendMessage("SetHP", Settings.HP_ADD);

        // 사운드 및 제거
        if (Settings.canSound)
        {
            AudioClip clip = Resources.Load("Audio/Coin") as AudioClip; // 오디오 찾아서 넣기
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
        Destroy(gameObject);
    }
}
