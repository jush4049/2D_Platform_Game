using System.Collections.Generic;
using UnityEngine;

class Monster
{
    public string name;   // �̸�
    public float hp;      // ü��
    public float damage;  // �÷��̾�� ���ϴ� ������
    public float delay;   // ���� ���� �ð�
    public int score;     // ���� �� ȹ�� ����
}

class Enemy
{
    // ���� List
    static List<Monster> mob = new List<Monster>
    {
        new Monster() {name = "Trap", hp = 0, damage = -1},
        new Monster() {name = "Spike", hp = 5, damage = -1, score = 200},
        new Monster() {name = "Slime", hp = 5, damage = -1, score = 500}
    };

    // �̸����� ���� ã��
    static public Monster Find(string name)
    {
        string[] tmp = name.Trim().Split(' '); // �¿� ������ �����ϰ� ������ ������ ������ ���ڿ��� �и��Ͽ� �迭�� ����
        int idx = mob.FindIndex(x => x.name.ToUpper() == tmp[0].ToUpper()); // �ڷ��� index ã��, �߰ߵ��� ������ -1

        // not found
        if (idx < 0) return null;

        // ���̵� ����
        Monster newMob = new Monster();
        float rate = 1 + Settings.level * 0.5f;

        newMob.hp = mob[idx].hp * rate;
        newMob.damage = (int)Mathf.Clamp(mob[idx].damage * rate, -1, 10000);
        newMob.delay = mob[idx].delay / rate;
        newMob.score = Mathf.FloorToInt(mob[idx].score * rate);

        return newMob;
    }
}