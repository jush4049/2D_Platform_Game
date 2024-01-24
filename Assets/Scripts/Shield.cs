using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    float hp;

    void Start()
    {
        hp = Enemy.Find(name).hp;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Bullet")
        {
            if (--hp < 0)
            {
                SendMessage("SetDestroy", transform.position);
                Destroy(gameObject);
            }
        }
    }
}
