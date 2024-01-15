using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 15;

    public GameObject gunFire; // 프리팹 파티클
    public AudioClip soundClip; // Audio/FX/Laser

    void Awake() // 초기화
    {
        // 플레이어 레이어와는 충돌금지
        LayerMask player = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(player, player); // 주인공과 총알과의 충돌은 무시

        Destroy(gameObject, 1f); // 생존 시간이 짧으므로 총알이 멀리까지는 가지 않음
    }

    void Start()
    {
        // 발사구름 & 사운드
        Instantiate(gunFire, transform.position, transform.rotation); // 발사구름은 총알이 향하는 방향으로 만듦
        AudioSource.PlayClipAtPoint(soundClip, transform.position); // 오디오 클립을 지정한 위치에서 재생
        if (Settings.canSound) AudioSource.PlayClipAtPoint(soundClip, transform.position);
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject exp = Instantiate(Resources.Load("Explosion")) as GameObject; // 폭파 불꽃을 만들고 크기를 줄임
        exp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Destroy(gameObject);
    }
}