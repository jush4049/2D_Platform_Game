using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    [SerializeField][Range(-2, 2)] float speed = 1;

    Transform body;

    void Start()
    {
        // 자식 오브젝트에서 Animation 컴포넌트 찾기
        Animation anim = transform.GetComponentInChildren<Animation>();

        string clip = anim.clip.name;
        anim[clip].speed = speed; // 애니메이션 속도 설정
        if (speed < 0)
        {
            anim[clip].time = anim[clip].length; // 애니메이션이 역순이면 마지막 프레임으로 이동
        }
        anim.Play();

        // Is Trigger Off인 자식 찾기
        Collider2D[] colls = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D coll in colls) 
        {
            if (!coll.isTrigger)
            {
                body = coll.transform;
                break;
            }
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = body; // 플레이어를 오브젝트의 자식으로 설정
        }
    }

    void OnCollisionExit2D (Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = null; // 플레이어를 자식에서 해제
        }
    }
}
