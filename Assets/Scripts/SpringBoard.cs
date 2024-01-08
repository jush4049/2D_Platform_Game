using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoard : MonoBehaviour
{
    GameObject spring;
    Animation anim;
    
    void Start()
    {
        gameObject.layer = 0; // 자신의 Layer를 디폴트로 설정

        spring = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            spring.layer = 0; // 스프링의 Layer를 디폴트로 설정
            other.transform.SendMessage("LongJump"); // 플레이어의 LongJump()함수 호출
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            spring.layer = LayerMask.NameToLayer("Ground"); // 스프링의 Layer를 Ground로 설정
        }
    }
}
