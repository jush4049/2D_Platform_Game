using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    [SerializeField][Range(-2, 2)] float speed = 1;

    Transform body;

    void Start()
    {
        // �ڽ� ������Ʈ���� Animation ������Ʈ ã��
        Animation anim = transform.GetComponentInChildren<Animation>();

        string clip = anim.clip.name;
        anim[clip].speed = speed; // �ִϸ��̼� �ӵ� ����
        if (speed < 0)
        {
            anim[clip].time = anim[clip].length; // �ִϸ��̼��� �����̸� ������ ���������� �̵�
        }
        anim.Play();

        // Is Trigger Off�� �ڽ� ã��
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
            other.transform.parent = body; // �÷��̾ ������Ʈ�� �ڽ����� ����
        }
    }

    void OnCollisionExit2D (Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = null; // �÷��̾ �ڽĿ��� ����
        }
    }
}
