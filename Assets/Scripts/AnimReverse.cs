using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimReverse : MonoBehaviour
{
    void SetDirection (float dir)
    {
        GetComponent<Animator>().SetFloat("dir", dir);
    }
}
