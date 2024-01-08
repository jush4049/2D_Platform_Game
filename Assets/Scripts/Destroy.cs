using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject obj;

    void Start()
    {
        Destroy(obj, 1f);
    }
}
