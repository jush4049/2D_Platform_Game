using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    float damage;  // ������ ��
    float delay; // �߻� ���� �ð�
    float hp;    // ü��

    float time;        // �ֱ� �߻� �ð�
    float power = 150; // �߻��ϴ� ��
    Vector3 pos;       // ������Ʈ ���� ��ġ
    
    Transform target;    // �÷��̾�
    Transform healthBar; // ü�¹�

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

        // �÷��̾��� ��ġ �� �߻� ����
        int dir = (transform.position.x > target.position.x) ? 1 : -1;                // �÷��̾ ���ʿ� ������ 1, �����ʿ� ������ -1
        float z = Random.Range(20, 30f) * dir;                                        // 12�� ���� ���� 20 ~ 30, ���� ���� 60 ~ 70
        cannon.transform.eulerAngles = new Vector3(0, 0, z);                          // �߻�ü ȸ��

        // �߻� ��
        float dist = Vector2.Distance(transform.position, target.position);
        float shotPower = power * dist / 2;                                           // �Ÿ��� ���� �߻� ���� ����
         
        cannon.GetComponent<Rigidbody2D>().AddForce(cannon.transform.up * shotPower); // �߻�ü�� ���� �������� �߻�
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
