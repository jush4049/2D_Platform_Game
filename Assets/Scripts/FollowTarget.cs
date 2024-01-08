using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    Transform target;

    [SerializeField] Vector2 speed = new Vector2 (3, 8);
    bool canTrack = true;

    void Start()
    {
        GetTarget();
    }

    void LateUpdate()
    {
        // 플레이어 사망
        if (target == null || !canTrack) { return; }
        
        Vector3 pos = transform.position;

        // x축 추적
        float x = target.position.x;
        if (Mathf.Abs(pos.x - x) > 0)
        {
            pos.x = Mathf.Lerp(pos.x, x, speed.x * Time.deltaTime);
            transform.position = pos;
        }

        // y축 추적
        float y = target.position.y;
        if (Mathf.Abs(pos.y - y) > 0)
        {
            pos.y = Mathf.Lerp(pos.y, y, speed.y * Time.deltaTime);
            transform.position = pos;
        }
    }

    void GetTarget()
    {
        GameObject obj = GameObject.Find("Player/CameraPoint"); // 주인공 사망 시, 오류 방지
        if (obj) target = obj.transform;
    }

    // Tracking On/Off <- Player
    void SetTrack(bool _canTrack) // 특정 상황에서 트래킹 중지
    {
        canTrack = _canTrack;
        if (canTrack) GetTarget();
    }
}