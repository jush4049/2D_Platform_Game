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
        // �÷��̾� ���
        if (target == null || !canTrack) { return; }
        
        Vector3 pos = transform.position;

        // x�� ����
        float x = target.position.x;
        if (Mathf.Abs(pos.x - x) > 0)
        {
            pos.x = Mathf.Lerp(pos.x, x, speed.x * Time.deltaTime);
            transform.position = pos;
        }

        // y�� ����
        float y = target.position.y;
        if (Mathf.Abs(pos.y - y) > 0)
        {
            pos.y = Mathf.Lerp(pos.y, y, speed.y * Time.deltaTime);
            transform.position = pos;
        }
    }

    void GetTarget()
    {
        GameObject obj = GameObject.Find("Player/CameraPoint"); // ���ΰ� ��� ��, ���� ����
        if (obj) target = obj.transform;
    }

    // Tracking On/Off <- Player
    void SetTrack(bool _canTrack) // Ư�� ��Ȳ���� Ʈ��ŷ ����
    {
        canTrack = _canTrack;
        if (canTrack) GetTarget();
    }
}