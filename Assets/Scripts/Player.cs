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

    Transform footPoint;
    Transform gunPoint;

    float speedJump;
    float gravity;
    bool isGround = true;

    LayerMask ground; // ���̾� ����ũ
    Vector2 extents; // Colider2D ũ���� ���� ���� ������ ����

    public Transform bullet;

    private GameObject contactPlatform;
    private Vector3 platformPosition;
    private Vector3 distance;

    float hp = Settings.HP;
    bool isDead = false;

    void Awake()
    {
        InitPlayer(); // �ʱ�ȭ�� ������ �Լ��� �и�
    }

    void Update()
    {
        JumpPlayer();
        FireGun();
    }

    void FixedUpdate() // ��� ���� ������ �Լ��� ���� ��, FixedUpdate()���� ȣ���ϴ� ����
    {
        InputKeys();
        MovePlayer();
        SetAnimation();
        if (!isDead) return;

        SetHP();
    }

    // Ű �Է�
    void InputKeys()
    {
        float key = Input.GetAxis("Horizontal");
        moveDir.x = speedRun * key;

        FlipPlayer(key);
    }

    // �÷��̾� ������
    void FlipPlayer(float key)
    {
        if (key == 0) return;

        dir = (key > 0) ? 1 : -1; // �Է� Ű�� -1�� 1�� ��ȯ
        Vector3 scale = transform.localScale;

        // �÷��̾� ���� ����
        scale.x = Mathf.Abs(scale.x) * dir; // scale.x�� ��ȣ ����
        transform.localScale = scale;
    }

    // �̵�
    void MovePlayer()
    {
        moveDir.y -= gravity * Time.deltaTime; // ������ �ϰ��� �߷� ����

        rbody.MovePosition(rbody.position + moveDir * Time.deltaTime); // ��� 1
        // rbody.velocity = moveDir; // ��� 2

        if (moveDir.y < -20)
        {
            moveDir.y = -1;
        }
    }

    // ����
    void JumpPlayer()
    {
        CheckGround(); // ���� ������ ����
        if (isGround && Input.GetButtonDown("Jump")) // �ٴ� ���� ����
        {
            transform.parent = null; // �����̴� ���ǿ� ���� �� �θ� ������Ʈ ����
            moveDir.y -= gravity * Time.deltaTime; // ������ �ϰ��� �߷� ����
            moveDir.y = speedJump;
            isGround = false;
            FootSound(2);
        }
    }

    void CheckGround()
    {
        if (Physics2D.OverlapCircle(footPoint.position, 0.1f, ground)) // footpoint ��ġ�� ������ ���� �����ϰ� ���� ���ο� ground ���̾ �ִ��� ����
        {
            isGround = true;
            // moveDir.y = -1;
        }

        // �Ӹ� ���� ��ֹ� ����
        Vector3 pos = transform.position; // bounds / 2
        pos.y += extents.y;

        // ������ ��ֹ� ����
        // Ray Position & ���� �Ÿ�
        pos = transform.position;
        float rayLen = gunPoint.localPosition.x + 0.2f; // Ray�� ���̴� �ѱ����� �ణ ��� ����

        // �������� Raycast
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.right * dir, rayLen, ground);
        if (hit.transform == null || hit.transform.CompareTag("Crate")) return; // ���� ó��

        // hit point�� �Ÿ� ����
        float hitLen = Vector2.Distance(pos, hit.point); // ���ΰ� �߽ɺ��� ��ü������ �Ÿ�
        float dist = rayLen - hitLen; // Ray�� ���̿� ���ΰ� �߽ɺ��� ��ü������ �Ÿ�, �� ���� Ŭ ���� ��ü�� �� ����� ���� ����

        // GunPoint�� ���� ����� �����̸� ����
        if (dist > 0.1f)
        {
            pos.x -= dist * dir;
            transform.position = pos;
        }

        moveDir.x = 0; // ���� �ӵ� Ŭ����, �̵� ����
    }

    // �ִϸ��̼� ����
    void SetAnimation()
    {
        anim.SetFloat("speed", Mathf.Abs(moveDir.x)); // �ִϸ��̼��� Parameter speed�� 0���� ũ�� Run�̹Ƿ� �̵� �ӵ��� ���� ���
        anim.SetFloat("velocity", moveDir.y); // �ִϸ��̼� �Ű����� ����
        anim.SetBool("isGround", isGround);
    }

    // �ִϸ��̼� �̺�Ʈ
    void FootSound(int kind)
    {
        footPoint.SendMessage("PlaySound", kind);
    }

    void FireGun()
    {
        if (!Input.GetKeyDown(KeyCode.LeftControl)) return;

        // ���� ����
        Quaternion rotation = transform.rotation;
        if (transform.localScale.x < 0)
        {
            rotation.eulerAngles = new Vector3(0, 180, 0); // �÷��̾��� ������ Scale.x�� ó���ϹǷ� y�� ȸ������ ��ȯ
        }

        Instantiate(bullet, gunPoint.position, rotation); // �÷��̾��� �������� �߻�
    }
    
    void LongJump()
    {
        isGround = false; // �ִϸ��̼� ���� ��� ����
        moveDir.y = speedJump * 1.5f; // ���� ���� ����

        FootSound(2);
    }

    void AddHP()
    {
        hp += Settings.HP_ADD; // �̸� ������ �� �� ��ŭ ü�� ����
        hp = Mathf.Clamp(hp, 0, Settings.PlayerHP); // ������ ü���� ü���� �ִ��� ���� �ʵ��� ����
    }

    void SetHP()
    {
        hp -= Settings.HP_DEC;
        ScoreManager.hp = hp; // ScoreManager�� �� ����

        if (hp < 0)
        {
            isDead = true;
            Camera.main.SendMessage("SetTrack", false); // ī�޶� Ʈ��ŷ ����
            anim.SetBool("isDead", true);
            footPoint.SendMessage("SetDamage", -1);
        }
    }

    // HP ���� (���� ��ȣ�ۿ�)
    void SetDamage(int damage)
    {
        if (!isDead)
        {
            Debug.Log("������ ����, " + damage);
            hp = (damage < 0) ? damage : hp - damage;
            footPoint.SendMessage("SetDamage", damage);
            Debug.Log("hp : " + hp);
            // SetPlayerDead();
        }
    }

    void SetPlayerDead()
    {
        // GameManager�� ���� - ����
        Destroy(gameObject);
    }

    // Init Player
    void InitPlayer()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        footPoint = transform.Find("FootPoint");
        gunPoint = transform.Find("GunPoint");

        ground = 1 << LayerMask.NameToLayer("Ground"); // Ž���� ���̾ Ground�� ����
        extents = GetComponent<Collider2D>().bounds.extents; // Collider2D �������� ���� ũ�⸦ ����

        // ���� �ʱⰪ ����
        speedRun = Settings.SPEED_RUN;
        speedJump = Settings.SPEED_JUMP;
        gravity = Settings.GRAVITY;

        // �׽�Ʈ
        /*Monster mob = Enemy.Find(name);
        if (mob != null)
        {
            Debug.LogError(name + " : ���� ����");
        }
        else
        {
            Debug.LogError("ü�� : " + mob.hp);
        }*/
    }
}