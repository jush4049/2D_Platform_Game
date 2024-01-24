using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject obj;
    public int delay;

    void Start()
    {
        Destroy(obj, delay);
    }
}
