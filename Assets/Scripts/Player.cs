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

    LayerMask ground; // ���̾� ����ũ
    Vector2 extents; // Colider2D ũ���� ���� ���� ������ ����

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
        /*if (!isDead) return;*/

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
        if (isGround && Input.GetButtonDown("Jump"))
        {
            isGround = false;
            transform.parent = null; // �����̴� ���ǿ� ���� �� �θ� ������Ʈ ����
            moveDir.y -= gravity * Time.deltaTime; // ������ �ϰ��� �߷� ����
            moveDir.y = speedJump;
            SfxSound(1);
        }
    }

    void CheckGround()
    {
        if (Physics2D.OverlapCircle(audioPoint.position, 0.1f, ground)) // footpoint ��ġ�� ������ ���� �����ϰ� ���� ���ο� ground ���̾ �ִ��� ����
        {
            isGround = true;
            // moveDir.y = -1;
        }
        else
        {
            //isGround = false;
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

        // ���� ����
        Quaternion rotation = transform.rotation;
        if (transform.localScale.x < 0)
        {
            rotation.eulerAngles = new Vector3(0, 180, 0); // �÷��̾��� ������ Scale.x�� ó���ϹǷ� y�� ȸ������ ��ȯ
        }

        Instantiate(bullet, gunPoint.position, rotation); // �÷��̾��� �������� �߻�
    }

    IEnumerator GunAnimation()
    {
        isGun = true;
        yield return new WaitForSeconds(1f);
        isGun = false;
    }

    void LongJump()
    {
        isGround = false; // �ִϸ��̼� ���� ��� ����
        moveDir.y = speedJump * 1.5f; // ���� ���� ����

        if (Settings.canSound)
        {
            SfxSound(2);
        }
    }

    void AddHP()
    {
        hp += Settings.HP_ADD; // �̸� ������ �� �� ��ŭ ü�� ����
        hp = Mathf.Clamp(hp, 0, Settings.PlayerHP); // ������ ü���� ü���� �ִ��� ���� �ʵ��� ����
    }

    void SetHP()
    {
        /*hp -= Settings.HP_DEC;*/
        ScoreManager.hp = hp; // ScoreManager�� �� ����

        if (hp < 1)
        {
            Instantiate(Resources.Load("Gravestone"), transform.position, Quaternion.identity);
            isDead = true;
            Camera.main.SendMessage("SetTrack", false); // ī�޶� Ʈ��ŷ ����
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

    // HP ���� (���� ��ȣ�ۿ�)
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
            Debug.Log("������ ����, " + damage);
            /*hp = (damage < 0) ? damage : hp - damage;
            AudioPoint.SendMessage("SetDamage", damage);*/
            Debug.Log("hp : " + hp);
        }
    }

    void SetPlayerDead()
    {
        // GameManager�� ���� - ����
        GameObject.Find("GameManager").SendMessage("PlayerDead");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin" && Settings.canSound)
        {
            Debug.Log("����");
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
            Debug.Log("����");
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

        ground = 1 << LayerMask.NameToLayer("Ground"); // Ž���� ���̾ Ground�� ����
        extents = GetComponent<Collider2D>().bounds.extents; // Collider2D �������� ���� ũ�⸦ ����

        render = GetComponentInChildren<SpriteRenderer>();

        // ���� �ʱⰪ ����
        speedRun = Settings.SPEED_RUN;
        speedJump = Settings.SPEED_JUMP;
        gravity = Settings.GRAVITY;
    }
}