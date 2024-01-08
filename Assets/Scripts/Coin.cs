using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SetScore();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            SetScore();
        }
    }

    void SetScore()
    {
        Vector3 pos = transform.position + new Vector3(0.2f, 0.2f, 0.2f);
        GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
        score.transform.position = pos;

        // ���� ǥ��
        switch (transform.tag)
        {
            case "Coin":
                score.SendMessage("SetScore", Settings.COIN_SCORE);
                ScoreManager.AddCoin();
                break;
            case "Gem":
                score.SendMessage("SetScore", Settings.GEM_SCORE);
                ScoreManager.AddGem();
                break;
        }

        // ����
        if (Settings.canSound)
        {
            AudioClip clip = Resources.Load("Audio/Coin") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, pos);
        }

        Destroy(gameObject);
    }
}
