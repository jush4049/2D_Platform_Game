using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoard : MonoBehaviour
{
    GameObject spring;
    Animation anim;
    
    void Start()
    {
        gameObject.layer = 0; // �ڽ��� Layer�� ����Ʈ�� ����

        spring = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            spring.layer = 0; // �������� Layer�� ����Ʈ�� ����
            other.transform.SendMessage("LongJump"); // �÷��̾��� LongJump()�Լ� ȣ��
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            spring.layer = LayerMask.NameToLayer("Ground"); // �������� Layer�� Ground�� ����
        }
    }
}
