using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    float damage;

    void Start()
    {
        damage = Enemy.Find(name).damage;
    }

    void Update()
    {
        Debug.Log(damage);
    }
    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.transform.tag != "Player") return;

        // Vector3 pos = transform.position; +new Vector3(0, 1.2f, 0);

        other.transform.SendMessage("SetDamage", damage);
    }
}
