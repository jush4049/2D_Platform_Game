using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    RectTransform hpBar; // RectTransform : UI 위젯의 Anchor, Pivot 및 크기에 대한 정보를 관리하는 컴포넌트
    Rect rect;           // Rect : 사각형 영역의 좌표 및 크기를 관리하는 클래스

    void Start()
    {
        var back = transform.Find("Background").GetComponent<RectTransform>(); // 배경 이미지
        rect = back.rect;                                                      // 배경 이미지의 크기

        hpBar = transform.Find("HP").GetComponent<RectTransform>();
        hpBar.sizeDelta = new Vector2(rect.width, rect.height);                // HP를 배경 이미지 크기로 설정, RectTransform의 크기는 sizeDelta로 설정
    }

    void Update()
    {
        if (transform.parent == null) return;

        // 체력바 뒤집기
        int dir = (transform.parent.localScale.x > 0) ? 1 : -1;
        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;                                          // 부모와 같은 방향으로 뒤집으면 실제로 뒤집히지 않음
    }

    // 체력 표시 <- monster
    void SetHP (float hp)
    {
        hpBar.sizeDelta = new Vector2(rect.width * hp, rect.height);           // HP의 길이를 매개변수 hp의 비율로 설정
    }
}
