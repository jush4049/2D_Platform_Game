using System.Collections.Generic;
using UnityEngine;

class Monster
{
    public string name;   // 이름
    public float hp;      // 체력
    public float damage;  // 플레이어에게 가하는 데미지
    public float delay;   // 공격 지연 시간
    public int score;     // 제거 시 획득 점수
}

class Enemy
{
    // 적군 List
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

    // 이름으로 적군 찾기
    static public Monster Find(string name)
    {
        string[] tmp = name.Trim().Split(' '); // 좌우 공백을 제거하고 내부의 공백을 준으로 문자열을 분리하여 배열에 저장
        int idx = mob.FindIndex(x => x.name.ToUpper() == tmp[0].ToUpper()); // 자료의 index 찾기, 발견되지 않으면 -1

        // not found
        if (idx < 0) return null;

        // 난이도 설정
        Monster newMob = new Monster();
        float rate = 1 + Settings.level * 0.5f;

        newMob.hp = mob[idx].hp * rate;
        newMob.damage = /*(int)Mathf.Clamp(mob[idx].damage * rate, -1, 10000);*/ mob[idx].damage * rate;
        newMob.delay = mob[idx].delay / rate;
        newMob.score = Mathf.FloorToInt(mob[idx].score * rate);

        return newMob;
    }
}