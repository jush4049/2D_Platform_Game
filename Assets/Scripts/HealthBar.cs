using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    RectTransform hpBar; // RectTransform : UI ������ Anchor, Pivot �� ũ�⿡ ���� ������ �����ϴ� ������Ʈ
    Rect rect;           // Rect : �簢�� ������ ��ǥ �� ũ�⸦ �����ϴ� Ŭ����

    void Start()
    {
        var back = transform.Find("Background").GetComponent<RectTransform>(); // ��� �̹���
        rect = back.rect;                                                      // ��� �̹����� ũ��

        hpBar = transform.Find("HP").GetComponent<RectTransform>();
        hpBar.sizeDelta = new Vector2(rect.width, rect.height);                // HP�� ��� �̹��� ũ��� ����, RectTransform�� ũ��� sizeDelta�� ����
    }

    void Update()
    {
        if (transform.parent == null) return;

        // ü�¹� ������
        int dir = (transform.parent.localScale.x > 0) ? 1 : -1;
        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;                                          // �θ�� ���� �������� �������� ������ �������� ����
    }

    // ü�� ǥ�� <- monster
    void SetHP (float hp)
    {
        hpBar.sizeDelta = new Vector2(rect.width * hp, rect.height);           // HP�� ���̸� �Ű����� hp�� ������ ����
    }
}
