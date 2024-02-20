using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    float damage;  // 데미지 값
    float delay; // 발사 지연 시간
    float hp;    // 체력

    float time;        // 최근 발사 시각
    float power = 150; // 발사하는 힘
    Vector3 pos;       // 오브젝트 생성 위치
    
    Transform target;    // 플레이어
    Transform healthBar; // 체력바

    void Start()
    {
        InitMonster();
    }

    void Update()
    {
        if (target != null && Time.time - time > delay)
        {
            ShootCannon();
            time = Time.time;
        }
    }

    void ShootCannon()
    {
        GameObject cannon = Instantiate(Resources.Load("SpikeBall")) as GameObject;
        cannon.transform.position = pos;
        cannon.GetComponent<SpikeBall>().SetSpeedAndDamage(0, damage);

        // 플레이어의 위치 및 발사 각도
        int dir = (transform.position.x > target.position.x) ? 1 : -1;                // 플레이어가 왼쪽에 있으면 1, 오른쪽에 있으면 -1
        float z = Random.Range(20, 30f) * dir;                                        // 12시 방향 기준 20 ~ 30, 수평선 기준 60 ~ 70
        cannon.transform.eulerAngles = new Vector3(0, 0, z);                          // 발사체 회전

        // 발사 힘
        float dist = Vector2.Distance(transform.position, target.position);
        float shotPower = power * dist / 2;                                           // 거리에 따라 발사 힘을 조절
         
        cannon.GetComponent<Rigidbody2D>().AddForce(cannon.transform.up * shotPower); // 발사체의 수직 방향으로 발사
        if (Settings.canSound)
        {
            AudioClip clip = Resources.Load("Audio/CannonShot") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
        }    
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = null;
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                target.SendMessage("SetDamage", -1);
                break;
            case "Bullet":
                SetDamage();
                break;
        }
    }

    void SetDamage()
    {
        //Instantiate(Resources.Load("CannonBall"), transform.position, Quaternion.identity);

        hp--;
        healthBar.SendMessage("SetHP", hp / Enemy.Find(name).hp);

        if (hp < 0)
        {
            SendMessage("SetDestroy", transform.position);
            Destroy(gameObject);
        }
    }

    void InitMonster()
    {
        hp = Enemy.Find(name).hp;
        damage = Enemy.Find(name).damage;
        delay = Enemy.Find(name).delay;

        pos = transform.position + new Vector3(0, 1.2f, 0);

        healthBar = transform.Find("HealthBar");
    }
}
