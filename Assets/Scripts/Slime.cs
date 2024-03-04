using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    Animator anim;

    float hp;
    float damage;

    int dir = 1; // 이동 방향
    float speed = 1; // 이동 속도

    Transform target; // 플레이어

    Transform healthBar; // 체력 바

    void Start()
    {
        hp = Enemy.Find(name).hp;
        damage = Enemy.Find(name).damage;

        anim = GetComponent<Animator>();
        healthBar = transform.Find("HealthBar");
    }

    void Update()
    {
        if (target == null) return;

        // 플레이어가 어느 방향에 있는지 판단
        dir = (transform.position.x > target.position.x) ? -1 : 1;

        // 플레이어 방향으로 뒤집기
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;

        // 이동
        float amount = speed * dir * Time.deltaTime;
        transform.Translate(Vector3.right * amount);
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
        if (target != null && other.tag == "Player")
        {
            target = null;
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                SetPlayerDamage();
                break;
            case "Bullet":
                SetDamage();
                break;
        }
    }

    // 플레이어 제거
    void SetPlayerDamage()
    {
        if (target == null) return;

        target.SendMessage("SetDamage", damage);

        // 정지
        target = null;
    }

    // 몬스터 체력 소모 및 제거
    void SetDamage()
    {
        hp--;
        healthBar.SendMessage("SetHP", hp / Enemy.Find(name).hp);
        if (hp < 0)
        {
            SendMessage("SetDestroy", transform.position);
            Destroy(gameObject);
        }
    }
}
