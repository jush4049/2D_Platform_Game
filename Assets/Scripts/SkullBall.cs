using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBall : MonoBehaviour
{
    float speed;
    float damage = -60;

    void Awake()
    {
        LayerMask enemy = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemy, enemy);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                other.transform.SendMessage("SetDamage", damage);
                DestroySelf();
                break;
            /*case "Bullet":
                DestroySelf();
                break;*/
        }
    }

    void DestroySelf()
    {
        /*Vector3 pos = transform.position;
        Instantiate(Resources.Load(""), pos, Quaternion.identity);*/

        /*if (Settings.canSound)
        {
            AudioClip clip = Resources.Load("Audio/") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, pos);
        }*/

        Destroy(gameObject);
    }

    public void SetSpeedAndDamage(float _speed, float _damage)
    {
        speed = _speed;
        damage = _damage;

        // 속도가 없으면 중력 사용
        GetComponent<Rigidbody2D>().gravityScale = (speed == 0) ? 1 : 0;
    }
}
