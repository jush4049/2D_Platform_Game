
public class Settings
{

    #region "���� ���� �޴� -----------"
    static public bool canMusic = true; // ������� ON/OFF
    static public bool canSound = true; // Player ��
    static public int level = 0;        // ���̵� ����, ������ �ɷ��� ���̴� �뵵�� ó��
    static public bool isMoblie = true; // ����� ��� üũ

    // ��� ���
    static public int language = 0;
    static private string[] langNames = { "Korean", "English", "Japanese" };

    static public string LanguageName
    {
        get { return langNames[language]; } 
    }
    #endregion

    #region "GameManager -----------"
    // const�� ������ �ڷ�� ����̹Ƿ� ��ũ��Ʈ���� �� ���� �Ұ� �� �빮�� ��� �ʿ�
    // const�� ������ �ڷ�� �� ��ü�� ���� �ڷ��̹Ƿ� static�� ������ ����
    public const int STAGE = 3;        // ���� ��ü �������� ��
    public const int MAX_PLAYER = 5;   // Player ��

    public const int stageNum = 1;     // ������ Stage ��ȣ
    public const int lastStage = 1;    // ������ ���� Stage
    #endregion

    #region "Player Status -----------"
    public const float HP = 3;         // ü��
    public const float HP_INC = 5;     // Stage�� ü�� ������
    public const float HP_DEC = 1;     // 1�ʿ� �Ҹ��ϴ� ü��

    public const float SPEED_RUN = 5;  // �ӵ�
    public const float SPEED_JUMP = 14;// ������
    public const float GRAVITY = 26;   // �߷�

    static public float PlayerHP
    {
        get { return HP + (stageNum - 1) * HP_INC; } // ���������� ����Ͽ� �÷��̾��� ü���� ����
    }
    #endregion

    #region "Item -----------"
    public const int COIN_SCORE = 10;   // ���� ����
    public const int GEM_SCORE = 50;    // ���� ����
    public const int COMBO_SCORE = 100; // �޺� ����

    public const int HP_ADD = 50;       // ���ΰ� ü�� ȹ��
    #endregion

    #region "Enmey Status -----------"
    // ������ Class�� �ۼ�
    #endregion
}
