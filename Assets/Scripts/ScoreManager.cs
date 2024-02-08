using UnityEngine.SceneManagement;

public class ScoreManager
{
    // ���� ����, GameManager ������
    static public float hp = 0;  // �÷��̾� ü��
    static public int heart = 0; // ���� �÷��̾� ��
    static public int score = 0; // ����

    static public int coinCount = 0;  // ȹ���� ���� ��
    static public int gemCount = 0;   // ȹ���� ���� ��
    static public int comboCount = 0; // �޼��� �޺� ��

    static public bool isStart = true; // ������ ���۵Ǵ°�?

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
