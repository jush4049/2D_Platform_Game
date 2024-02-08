using UnityEngine.SceneManagement;

public class ScoreManager
{
    // 전역 변수, GameManager 참조용
    static public float hp = 0;  // 플레이어 체력
    static public int heart = 0; // 남은 플레이어 수
    static public int score = 0; // 득점

    static public int coinCount = 0;  // 획득한 코인 수
    static public int gemCount = 0;   // 획득한 보석 수
    static public int comboCount = 0; // 달성한 콤보 수

    static public bool isStart = true; // 게임이 시작되는가?

    /*score += Settings.COIN_SCORE;
    score += Settings.GEM_SCORE;
    score += combo * Settings.COIN_SCORE;*/

    void OnEnable()
    {
        string str = SceneManager.GetActiveScene().name;
    }
    static public void AddCoin()
    {
        coinCount++;
        score += 10;
    }

    static public void AddGem()
    {
        gemCount++;
        score += 50;
    }

    static public void AddCombo(bool isCombo)
    {
        if (isCombo)
        {
            comboCount++;
            score += comboCount * 100;
        }
        else
        {
            comboCount = 0;
        }
    }

    static public void AddScore(int _score)
    {
        score += _score;
    }

    static public void Clear()
    {
        hp = Settings.HP;
        heart = Settings.MAX_PLAYER;

        score = coinCount = gemCount = comboCount = 0;

        if (!isStart) return;
        isStart = false;
    }
}
