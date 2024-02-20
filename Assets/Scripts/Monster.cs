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
        new Monster() {name = "Trap", hp = 0, damage = -10},
        new Monster() {name = "Spike", hp = 5, damage = -1, score = 200},
        new Monster() {name = "Slime", hp = 5, damage = -1, score = 100},
        new Monster() {name = "Cannon", hp = 15, damage = -2, delay = 3, score = 300},
        new Monster() {name = "Person", hp = 10, damage = -1, delay = 2, score = 200},
        new Monster() {name = "Shield", hp = 30},
        new Monster() {name = "Boss", hp = 100, damage = -3, delay = 5, score = 1000}
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
        newMob.damage = /*(int)Mathf.Clamp(mob[idx].damage * rate, -1, 10000);*/ mob[idx].damage * rate;
        newMob.delay = mob[idx].delay / rate;
        newMob.score = Mathf.FloorToInt(mob[idx].score * rate);

        return newMob;
    }
}