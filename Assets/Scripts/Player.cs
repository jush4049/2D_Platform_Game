using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rbody;
    Animator anim;
    float speedRun;
    Vector2 moveDir;
    int dir = 1;

    Transform audioPoint;
    Transform gunPoint;

    float speedJump;
    float gravity;
    bool isGround = true;

    LayerMask ground; // 레이어 마스크
    Vector2 extents; // Colider2D 크기의 절반 값을 저장할 변수

    public Transform bullet;

    private GameObject contactPlatform;
    private Vector3 platformPosition;
    private Vector3 distance;

    float hp = Settings.HP;
    bool isDead = false;

    bool isGun = false;

    SpriteRenderer render;

    void Awake()
    {
        InitPlayer(); // 초기화는 별도의 함수로 분리
    }

    void Update()
    {
        JumpPlayer();
        FireGun();
    }

    void FixedUpdate() // 기능 별로 독립된 함수를 생성 후, FixedUpdate()에서 호출하는 구조
    {
        InputKeys();
        MovePlayer();
        SetAnimation();
        /*if (!isDead) return;*/

        SetHP();
    }

    // 키 입력
    void InputKeys()
    {
        float key = Input.GetAxis("Horizontal");
        moveDir.x = speedRun * key;

        FlipPlayer(key);
    }

    // 플레이어 뒤집기
    void FlipPlayer(float key)
    {
        if (key == 0) return;

        dir = (key > 0) ? 1 : -1; // 입력 키를 -1과 1로 변환
        Vector3 scale = transform.localScale;

        // 플레이어 방향 설정
        scale.x = Mathf.Abs(scale.x) * dir; // scale.x의 부호 설정
        transform.localScale = scale;
    }

    // 이동
    void MovePlayer()
    {
        moveDir.y -= gravity * Time.deltaTime; // 점프와 하강에 중력 적용

        rbody.MovePosition(rbody.position + moveDir * Time.deltaTime); // 기능 1
        // rbody.velocity = moveDir; // 기능 2

        if (moveDir.y < -20)
        {
            moveDir.y = -1;
        }
    }

    // 점프
    void JumpPlayer()
    {
        CheckGround(); // 지면 위인지 조사
        if (isGround && Input.GetButtonDown("Jump"))
        {
            isGround = false;
            transform.parent = null; // 움직이는 발판에 있을 시 부모 오브젝트 해제
            moveDir.y -= gravity * Time.deltaTime; // 점프와 하강에 중력 적용
            moveDir.y = speedJump;
            SfxSound(1);
        }
    }

    void CheckGround()
    {
        if (Physics2D.OverlapCircle(audioPoint.position, 0.1f, ground)) // footpoint 위치에 가상의 원을 설정하고 원의 내부에 ground 레이어가 있는지 조사
        {
            isGround = true;
            // moveDir.y = -1;
        }
        else
        {
            //isGround = false;
        }
        // 머리 위의 장애물 조사
        Vector3 pos = transform.position; // bounds / 2
        pos.y += extents.y;

        // 전방의 장애물 조사
        // Ray Position & 투시 거리
        pos = transform.position;
        float rayLen = gunPoint.localPosition.x + 0.2f; // Ray의 길이는 총구보다 약간 길게 설정

        // 전방으로 Raycast
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.right * dir, rayLen, ground);
        if (hit.transform == null || hit.transform.CompareTag("Crate")) return; // 예외 처리

        // hit point와 거리 차이
        float hitLen = Vector2.Distance(pos, hit.point); // 주인공 중심부터 벽체까지의 거리
        float dist = rayLen - hitLen; // Ray의 길이와 주인공 중심부터 벽체까지의 거리, 이 값이 클 수록 벽체가 더 가까운 곳에 있음

        // GunPoint가 벽을 통과한 상태이면 후진
        if (dist > 0.1f)
        {
            pos.x -= dist * dir;
            transform.position = pos;
        }

        moveDir.x = 0; // 전진 속도 클리어, 이동 중지
    }

    // 애니메이션 설정
    void SetAnimation()
    {
        anim.SetFloat("speed", Mathf.Abs(moveDir.x)); // 애니메이션의 Parameter speed는 0보다 크면 Run이므로 이동 속도의 절댓값 사용
        anim.SetFloat("velocity", moveDir.y); // 애니메이션 매개변수 설정
        anim.SetBool("isGround", isGround);
        anim.SetBool("isGun", isGun);
    }

    void SfxSound(int kind)
    {
        audioPoint.SendMessage("PlaySound", kind);
    }

    void FireGun()
    {
        if (!Input.GetKeyDown(KeyCode.LeftControl)) return;

        StartCoroutine(GunAnimation());

        // 방향 설정
        Quaternion rotation = transform.rotation;
        if (transform.localScale.x < 0)
        {
            rotation.eulerAngles = new Vector3(0, 180, 0); // 플레이어의 방향은 Scale.x로 처리하므로 y축 회전으로 변환
        }

        Instantiate(bullet, gunPoint.position, rotation); // 플레이어의 방향으로 발사
    }

    IEnumerator GunAnimation()
    {
        isGun = true;
        yield return new WaitForSeconds(1f);
        isGun = false;
    }

    void LongJump()
    {
        isGround = false; // 애니메이션 점프 모션 실행
        moveDir.y = speedJump * 1.5f; // 점프 높이 설정

        if (Settings.canSound)
        {
            SfxSound(2);
        }
    }

    void AddHP()
    {
        hp += Settings.HP_ADD; // 미리 설정해 둔 값 만큼 체력 증가
        hp = Mathf.Clamp(hp, 0, Settings.PlayerHP); // 증가한 체력이 체력의 최댓값을 넘지 않도록 설정
    }

    void SetHP()
    {
        /*hp -= Settings.HP_DEC;*/
        ScoreManager.hp = hp; // ScoreManager에 값 저장

        if (hp < 1)
        {
            Instantiate(Resources.Load("Gravestone"), transform.position, Quaternion.identity);
            isDead = true;
            Camera.main.SendMessage("SetTrack", false); // 카메라 트래킹 금지
            anim.SetBool("isDead", true);
            audioPoint.SendMessage("SetDamage", -1);
            SetPlayerDead();
        }
    }

    IEnumerator Hurt()
    {
        render.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.3f);
        render.color = new Color(1, 1, 1, 1);
    }

    // HP 감소 (적과 상호작용)
    void SetDamage(int damage)
    {
        if (Settings.canSound)
        {
            /*AudioClip clip = Resources.Load("Audio/PlayerHurt") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, transform.position);*/
            SfxSound(2);
            StartCoroutine(Hurt());
        }

        if (!isDead)
        {
            hp += damage;
            Debug.Log("데미지 입음, " + damage);
            /*hp = (damage < 0) ? damage : hp - damage;
            AudioPoint.SendMessage("SetDamage", damage);*/
            Debug.Log("hp : " + hp);
        }
    }

    void SetPlayerDead()
    {
        // GameManager에 통지 - 보류
        GameObject.Find("GameManager").SendMessage("PlayerDead");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin" && Settings.canSound)
        {
            Debug.Log("코인");
            SfxSound(0);
        }
        else if(other.tag == "Gem" && Settings.canSound)
        {
            SfxSound(0);
        }
        else if (other.tag == "Energy" && Settings.canSound)
        {
            SfxSound(0);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Coin" && Settings.canSound)
        {
            Debug.Log("코인");
            SfxSound(0);
        }
    }

    // Init Player
    void InitPlayer()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        audioPoint = transform.Find("AudioPoint");
        gunPoint = transform.Find("GunPoint");

        ground = 1 << LayerMask.NameToLayer("Ground"); // 탐지할 레이어를 Ground로 설정
        extents = GetComponent<Collider2D>().bounds.extents; // Collider2D 사이즈의 절반 크기를 구함

        render = GetComponentInChildren<SpriteRenderer>();

        // 변수 초기값 설정
        speedRun = Settings.SPEED_RUN;
        speedJump = Settings.SPEED_JUMP;
        gravity = Settings.GRAVITY;
    }
}