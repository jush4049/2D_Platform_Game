using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    void SetDestroy(Vector3 pos)
    {
        if (transform.name != "Shield")
        {
            Instantiate(Resources.Load("Gem"), pos + new Vector3(0, 0, 0), Quaternion.identity);

            // Á¡¼ö
            GameObject scoreText = Instantiate(Resources.Load("Score")) as GameObject;
            scoreText.transform.position = pos;

            int score = Enemy.Find(name).score;
            scoreText.SendMessage("SetScore", score);
            ScoreManager.AddScore(score);
        }

        if (Settings.canSound)
        {
            AudioClip clip = Resources.Load("Audio/Bullet") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }
}
