
public class ScoreManager
{
    // 전역 변수, GameManager 참조용
    static public float hp = 0;  // 플레이어 체력
    static public int heart = 0; // 남은 플레이어 수
    static public int score = 0; // 득점

    static public int coin = 0;  // 획득한 코인 수
    static public int gem = 0;   // 획득한 보석 수
    static public int combo = 0; // 달성한 콤보 수
    
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
