using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    Animation anim;
    Animator animator;
    // [SerializeField][Range(0, 2)] float animSpeed = 1;

    float hp;     // ü��
    float damage; // ������
    float delay;  // ���� ���� �ð�

    float time;       // �ֱ� �߻� �ð�
    // float speed = 10; // �߻� �ӵ�

    Transform person; // ����
    Vector3 pos;      // ������ ��ġ

    float animTime;   // �ִϸ��̼� �ߴ� ����
    string clipName;

    int dir;           // �÷��̾��� ����

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
            ShootPerson();
            time = Time.time;
        }
    }

    void ShootPerson()
    {
        // �÷��̾��� ����
        int dir = (pos.x > target.position.x) ? 1 : -1;

        /*GameObject bullet = Instantiate(Resources.Load("SpikeBall")) as GameObject;
        bullet.transform.position = pos;
        bullet.GetComponent<SpikeBall>().SetSpeedAndDamage(0, damage);*/
        /*if (Settings.canSound)
        {
            AudioClip clip = Resources.Load("Audio/???") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, pos);
        }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
            pos = person.position;

            // �÷��̾� �������� ȸ��
            // dir = (transform.position.x > target.position.x) ? 1 : -1;

            int localScaledir = (transform.position.x > target.position.x) ? -1 : 1;

            // �÷��̾� �������� ������
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * localScaledir;
            transform.localScale = scale;

            // �ִϸ��̼� �ߴ� �ð� ����
            /*animTime = anim[clipName].time;
            anim.Stop();*/
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = null;

            // �ߴ� �ð����� �ִϸ��̼� ���
            /*anim[clipName].time = animTime;
            anim.Play();*/
        }
    }

    void OnCollisionEnter2D(Collision2D other)
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
            SendMessage("SetDestroy", pos);
            Destroy(gameObject);
        }
    }

    void InitMonster()
    {
        person = transform.GetChild(0);

        anim = person.GetComponent<Animation>();
        /*clipName = anim.clip.name;
        anim[clipName].speed = animSpeed;*/

        hp = Enemy.Find(name).hp;
        damage = Enemy.Find(name).damage;
        delay = Enemy.Find(name).delay;

        pos = transform.position + new Vector3(0, 1.2f, 0);

        healthBar = transform.Find("HealthBar");
    }
}
