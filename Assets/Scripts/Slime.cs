using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    Animator anim;

    float hp;
    float damage;

    int dir = 1; // �̵� ����
    float speed = 1; // �̵� �ӵ�

    Transform target; // �÷��̾�

    Transform healthBar; // ü�� ��

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

        // �÷��̾ ��� ���⿡ �ִ��� �Ǵ�
        dir = (transform.position.x > target.position.x) ? -1 : 1;

        // �÷��̾� �������� ������
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;

        // �̵�
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

    // �÷��̾� ����
    void SetPlayerDamage()
    {
        if (target == null) return;

        target.SendMessage("SetDamage", damage);

        // ����
        target = null;
    }

    // ���� ü�� �Ҹ� �� ����
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
