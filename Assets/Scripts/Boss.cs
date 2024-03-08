using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    float hp;              // 체력
    float damage;          // 공격 데미지
    float delay;           // 공격 지연시간

    float time;            // 발사 시각
    float ballSpeed = 10;  // 발사 속도
    float power = 300;     // 발사 힘

    Transform spawnPoint;  // 발사 오브젝트 생성 포인트
    Transform target;      // 플레이어
    int dir;           // 플레이어의 방향

    SpriteRenderer render; // 보스 SpriteRenderer
    Transform healthBar;   // 체력바

    public AudioClip[] clips; // 오디오 클립을 인스펙터에서 할당하도록 public 배열로 작성
    AudioSource bossAudioSource; // 오디오 소스

    GameObject bossHealthBar;    // 체력바
    GameObject gameAudio;        // 배경음악 오브젝트

    void Start()
    {
        InitMonster();
        bossAudioSource = GetComponent<AudioSource>();
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

        // 폭파 및 아이템 생성
        Instantiate(Resources.Load("MonsterExplosion"), pos, Quaternion.identity);
        StartCoroutine(MakeGem());

        // 폭파 사운드
        if (Settings.canSound)
        {
            /*AudioClip clip = Resources.Load("Audio/BossRemove") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, pos);*/
            BossSound(1);
        }

        // 서서히 투명하게 적용
        for (float alpha = 1; alpha > 0; alpha -= 0.01f)
        {
            render.color = new Color(1, 0, 0, alpha);
            yield return null;
        }

        // 화면에서 제거
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

            // 발사
            float power = Random.Range(300, 500f);
            obj.GetComponent<Rigidbody2D>().AddForce(obj.transform.up * power);

            yield return new WaitForSeconds(0.3f);
        }
    }

    // 아이템 생성
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

        // 아이템 생성 및 Rigidbody 설정
        GameObject obj = Instantiate(Resources.Load(item)) as GameObject;
        obj.transform.position = transform.position;
        obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        obj.GetComponent<Rigidbody2D>().gravityScale = 1;

        // Collider, Animation 비활성화
        obj.GetComponent<Collider2D>().isTrigger = false;
        //obj.GetComponent<Animation>().enabled = false;
        obj.GetComponentInChildren<Collider2D>().isTrigger = true;

        // z축으로 -20 ~ 20도 회전
        float angle = Random.Range(-20, 20);
        obj.transform.eulerAngles = new Vector3(0, 0, angle);

        return obj;
    }
    // 회전
    void FlipBoss()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    void BossSound(int kind)
    {
        bossAudioSource.clip = clips[kind]; // 종류에 맞는 오디오 클립 설정
        if (Settings.canSound) bossAudioSource.Play();
    }

    IEnumerator Attack()
    {
        MakeBall(false); // 1발 사격

        // 3 ~ 5발 랜덤 사격
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

        GameObject ball = Instantiate(Resources.Load("SkullBall")) as GameObject;
        ball.transform.position = pos;
        ball.GetComponent<SkullBall>().SetSpeedAndDamage(speed, damage);

        // 발사각
        if (isHigh)
        {
            float z = Random.Range(20, 30f) * dir;
            ball.transform.eulerAngles = new Vector3(0, 0, -z);

            // 발사 파워
            float dist = Vector2.Distance(pos, target.position);
            float shotPower = power * dist / 3;
            ball.GetComponent<Rigidbody2D>().AddForce(ball.transform.up * shotPower);
        }

        if (Settings.canSound)
        {
            BossSound(0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;

            // 플레이어 방향으로 회전
            dir = (transform.position.x > target.position.x) ? 1 : -1;

            FlipBoss();
            gameAudio.SetActive(false);
            bossHealthBar.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            target = null;
            bossHealthBar.SetActive(false);
            gameAudio.SetActive(true);
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
        Vector3 pos = transform.position;
        hp--;

        healthBar.SendMessage("SetHP", hp / Enemy.Find(name).hp);

        if (hp < 0)
        {
            Vector3 telPos = new Vector3(183, 26, 0); // 텔레포트 오브젝트 생성 좌표
            Instantiate(Resources.Load("Teleport"), telPos, Quaternion.identity);
            gameAudio.SetActive(true);
            StartCoroutine(SetDestroy());
        }
    }

    void InitMonster()
    {
        spawnPoint = transform.Find("SpawnPoint");
        render = GetComponent<SpriteRenderer>();

        // 몬스터 정보
        hp = Enemy.Find(name).hp;
        damage = Enemy.Find(name).damage;
        delay = Enemy.Find(name).delay;

        healthBar = transform.Find("BossHealthBar");
        bossHealthBar = GameObject.Find("BossHealthBar");
        bossHealthBar.SetActive(false);

        gameAudio = GameObject.Find("Audio");
    }
}
