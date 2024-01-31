using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    float hp;              // ü��
    float damage;          // ���� ������
    float delay;           // ���� �����ð�

    float time;            // �߻� �ð�
    float ballSpeed = 10;  // �߻� �ӵ�
    float power = 300;     // �߻� ��

    Transform spawnPoint;  // �߻� ������Ʈ ���� ����Ʈ
    Transform target;      // �÷��̾�
    int dir;           // �÷��̾��� ����

    SpriteRenderer render; // ���� SpriteRenderer
    Transform healthBar;   // ü�¹�

    void Start()
    {
        InitMonster();
    }
    
    void Update()
    {
        if (target != null && Time.time - time > delay)
        {
            StartCoroutine(Attack());
            time = Time.time;
        }

        if (render != null)
        {
            float rate = hp / Enemy.Find(name).hp;
            render.color = new Color(1, rate, rate, 1);
        }
    }

    IEnumerator SetDestroy()
    {
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());

        Destroy(healthBar.gameObject);
        // GetComponent<Animator>().enabled = false;

        Vector3 pos = transform.position;

        // ���� �� ������ ����
        Instantiate(Resources.Load("Explosion"), pos, Quaternion.identity);
        StartCoroutine(MakeGem());
        
        // ���� ����
        if (Settings.canSound)
        {
            AudioClip clip = Resources.Load("Audio/BossRemove") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, pos);
        }

        // ������ �����ϰ� ����
        for (float alpha = 1; alpha > 0; alpha -= 0.01f)
        {
            render.color = new Color(1, 0, 0, alpha);
            yield return null;
        }

        // ȭ�鿡�� ����
        Destroy(render);
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    IEnumerator MakeGem()
    {
        int count = Random.Range(15, 26);
        
        for (int i = 1; i <= count; i++)
        {
            bool isEnergy = (i <= 2);
            GameObject obj = MakeCoinGem(isEnergy);

            // �߻�
            float power = Random.Range(300, 500f);
            obj.GetComponent<Rigidbody2D>().AddForce(obj.transform.up * power);

            yield return new WaitForSeconds(0.3f);
        }
    }

    // ������ ����
    GameObject MakeCoinGem (bool isEnergy)
    {
        string item;

        if (isEnergy)
        {
            item = "Energy";
        }
        else if (Random.Range(0, 4) < 3)
        {
            item = "coin";
        }
        else
        {
            item = "Gem";
        }

        // ������ ���� �� Rigidbody ����
        GameObject obj = Instantiate(Resources.Load(item)) as GameObject;
        obj.transform.position = transform.position;
        obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        obj.GetComponent<Rigidbody2D>().gravityScale = 1;

        // Collider, Animation ��Ȱ��ȭ
        obj.GetComponent<Collider2D>().isTrigger = false;
        //obj.GetComponent<Animation>().enabled = false;

        // z������ -20 ~ 20�� ȸ��
        float angle = Random.Range(-20, 20);
        obj.transform.eulerAngles = new Vector3(0, 0, angle);

        return obj;
    }
    // ȸ��
    void FlipBoss()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    IEnumerator Attack()
    {
        MakeBall(false); // 1�� ���

        // 3 ~ 5�� ���� ���
        int count = Random.Range(3, 6);
        for (int i = 1; i <= count; i++)
        {
            MakeBall(true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void MakeBall(bool isHigh)
    {
        if (target == null) return;

        int dir = (transform.position.x > target.position.x) ? -1 : 1;
        Vector3 pos = spawnPoint.position;
        float speed = (isHigh) ? 0 : ballSpeed * dir;

        GameObject ball = Instantiate(Resources.Load("SpikeBall")) as GameObject;
        ball.transform.position = pos;
        ball.GetComponent<SpikeBall>().SetSpeedAndDamage(speed, damage);

        // �߻簢
        if (isHigh)
        {
            float z = Random.Range(20, 30f) * dir;
            ball.transform.eulerAngles = new Vector3(0, 0, -z);

            // �߻� �Ŀ�
            float dist = Vector2.Distance(pos, target.position);
            float shotPower = power * dist / 3;
            ball.GetComponent<Rigidbody2D>().AddForce(ball.transform.up * shotPower);
        }

        if (Settings.canSound)
        {
            AudioClip clip = Resources.Load("Audio/BossShot") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;

            // �÷��̾� �������� ȸ��
            dir = (transform.position.x > target.position.x) ? 1 : -1;

            FlipBoss();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = null;
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
                /*if (shield == null)*/
                SetDamage();
                break;
        }
    }

    void SetDamage()
    {
        Vector3 pos = transform.position;

        hp--;

        healthBar.SendMessage("SetHP", hp / Enemy.Find(name).hp);

        if (hp < 0)
        {
            StartCoroutine(SetDestroy());
        }
    }
    void InitMonster()
    {
        spawnPoint = transform.Find("SpawnPoint");
        render = GetComponent<SpriteRenderer>();

        // ���� ����
        hp = Enemy.Find(name).hp;
        damage = Enemy.Find(name).damage;
        delay = Enemy.Find(name).delay;

        healthBar = transform.Find("HealthBar");
        // target = GameObject.Find("Player").GetComponent<Transform>();
    }
}
