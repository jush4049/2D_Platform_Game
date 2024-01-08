
public class ScoreManager
{
    // ���� ����, GameManager ������
    static public float hp = 0;  // �÷��̾� ü��
    static public int heart = 0; // ���� �÷��̾� ��
    static public int score = 0; // ����

    static public int coin = 0;  // ȹ���� ���� ��
    static public int gem = 0;   // ȹ���� ���� ��
    static public int combo = 0; // �޼��� �޺� ��
    
    /*score += Settings.COIN_SCORE;
    score += Settings.GEM_SCORE;
    score += combo * Settings.COIN_SCORE;*/

    static public void AddCoin()
    {
        coin++;
        score += 10;
    }

    static public void AddGem()
    {
        gem++;
        score += 50;
    }

    static public void AddCombo(bool isCombo)
    {
        if (isCombo)
        {
            combo++;
            score += combo * 100;
        }
        else
        {
            combo = 0;
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

        score = coin = gem = combo = 0;
    }
}
