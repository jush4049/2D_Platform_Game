
public class Settings
{

    #region "게임 설정 메뉴 -----------"
    static public bool canMusic = true; // 배경음악 ON/OFF
    static public bool canSound = true; // Player 수
    static public int level = 0;        // 난이도 설정, 몬스터의 능력을 높이는 용도로 처리
    static public bool isMoblie = true; // 모바일 기기 체크

    // 사용 언어
    static public int language = 0;
    static private string[] langNames = { "Korean", "English", "Japanese" };

    static public string LanguageName
    {
        get { return langNames[language]; } 
    }
    #endregion

    #region "GameManager -----------"
    // const로 선언한 자료는 상수이므로 스크립트에서 값 변경 불가 및 대문자 사용 필요
    // const로 선언한 자료는 그 자체가 정적 자료이므로 static을 붙이지 않음
    public const int STAGE = 3;        // 게임 전체 스테이지 수
    public const int MAX_PLAYER = 5;   // Player 수

    public const int stageNum = 1;     // 현재의 Stage 번호
    public const int lastStage = 1;    // 진행한 최종 Stage
    #endregion

    #region "Player Status -----------"
    public const float HP = 3;         // 체력
    public const float HP_INC = 5;     // Stage별 체력 증가분
    public const float HP_DEC = 1;     // 1초에 소모하는 체력

    public const float SPEED_RUN = 5;  // 속도
    public const float SPEED_JUMP = 14;// 점프력
    public const float GRAVITY = 26;   // 중력

    static public float PlayerHP
    {
        get { return HP + (stageNum - 1) * HP_INC; } // 스테이지에 비례하여 플레이어의 체력을 설정
    }
    #endregion

    #region "Item -----------"
    public const int COIN_SCORE = 10;   // 코인 득점
    public const int GEM_SCORE = 50;    // 보석 득점
    public const int COMBO_SCORE = 100; // 콤보 득점

    public const int HP_ADD = 50;       // 주인공 체력 획득
    #endregion

    #region "Enmey Status -----------"
    // 별도의 Class로 작성
    #endregion
}
