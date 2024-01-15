using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 15;

    public GameObject gunFire; // ������ ��ƼŬ
    public AudioClip soundClip; // Audio/FX/Laser

    void Awake() // �ʱ�ȭ
    {
        // �÷��̾� ���̾�ʹ� �浹����
        LayerMask player = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(player, player); // ���ΰ��� �Ѿ˰��� �浹�� ����

        Destroy(gameObject, 1f); // ���� �ð��� ª���Ƿ� �Ѿ��� �ָ������� ���� ����
    }

    void Start()
    {
        // �߻籸�� & ����
        Instantiate(gunFire, transform.position, transform.rotation); // �߻籸���� �Ѿ��� ���ϴ� �������� ����
        AudioSource.PlayClipAtPoint(soundClip, transform.position); // ����� Ŭ���� ������ ��ġ���� ���
        if (Settings.canSound) AudioSource.PlayClipAtPoint(soundClip, transform.position);
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject exp = Instantiate(Resources.Load("Explosion")) as GameObject; // ���� �Ҳ��� ����� ũ�⸦ ����
        exp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Destroy(gameObject);
    }
}