using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(GetComponent<Collider2D>());
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.8f, 1);

            GameObject.Find("GameManager").SendMessage("NewSpawnPoint", transform.position);
        }
    }
}
