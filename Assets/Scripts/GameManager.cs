#define RELEASE // ���Ǻ� �������� ���� �ɺ� ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game State
    enum STATE {start, play, wait, respawn, clear, complete, gameOver};
    STATE state;

    // Top Panel UI
    Text textStage;
    Text textCoin;
    Text textGem;
    Text textScore;
    Text textHeart;

    // HP Bar
    RectTransform hpGauge;
    Vector2 hpSize;

    // Player
    GameObject player;
    Vector3 spawnPoint;
    float hpMax;

    // Main Camera Offset
    Vector3 camOffset = new Vector3(3, -2, -10);

    // PopUp UI
    GameObject panelMask;
    GameObject panelMenu;
    GameObject panelGameOver;
    GameObject panelComplete;
    GameObject panelCurrent;

    GameObject panelOption;

    void Awake()
    {
        InitGame();
        state = STATE.start;
    }

    void Update()
    {
        SetScore();

        switch (state)
        {
            case STATE.start:
                // ���ӽ���
                StartGame();
                break;
            case STATE.play:
                SetScore();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OpenPopup(panelMenu);
                }
                break;
            case STATE.respawn:
                // �÷��̾� ������
                StartCoroutine(PlayerRespawn());
                break;
            case STATE.clear:
                // �������� Ŭ����
                StartCoroutine(NextStage());
                break;
            case STATE.complete:
                // �̼� ����
                OpenPopup(panelComplete);
                break;
            case STATE.gameOver:
                // ���� ����
                OpenPopup(panelGameOver);
                break;
        }
    }

    void StartGame()
    {
// RELEASE��� �ɺ� ���� ��, ������ ����
#if (RELEASE)
        // ���ΰ��� ���� �������� �̵�
        spawnPoint = GameObject.Find("StartPoint").transform.position;
        player = GameObject.Find("Player");
        player.transform.position = spawnPoint;

        Camera.main.transform.position = spawnPoint + camOffset;
        state = STATE.play;
#else
        Debug.Log("����� ��� ����");
#endif
    }

    void PlayerDead()
    {
        state = STATE.respawn;
    }

    IEnumerator PlayerRespawn()
    {
        state = STATE.wait;                              // ������ ���¸� wait�� ����, Update() �Լ� ���� ����
        yield return new WaitForSeconds(2f);

        if (--ScoreManager.heart > 0)
        {
            MakePlayer();
            Camera.main.SendMessage("SetTrack", true);   // ī�޶� Ʈ��ŷ �����
            state = STATE.play;                          // ������ ���¸� play�� ����
        }
        else
        {
            state = STATE.gameOver;                      // ���� ������, ��������
        }
    }

    void MakePlayer()
    {
        player = Instantiate(Resources.Load("Player")) as GameObject;
        player.transform.position = spawnPoint;
        player.name = "Player";                         // �÷��̾� ���� ��, ������Ʈ �̸� ����

        // ���� ī�޶�
        Vector3 camPos = spawnPoint + camOffset;
        Camera.main.transform.position = camPos;        // ī�޶� ������ ��ġ�� �̵�
    }
    void SetScore()
    {
        textStage.text = Settings.stageNum.ToString("00");
        textCoin.text = ScoreManager.coinCount.ToString();
        textGem.text = ScoreManager.gemCount.ToString();
        textScore.text = ScoreManager.score.ToString("#,0");
        textHeart.text = ScoreManager.heart.ToString("x 0");

        // HP
        float rate = ScoreManager.hp / hpMax; // �÷��̾��� ���� ü�°� �⺻ ü���� ���� ���ϱ�
        hpGauge.sizeDelta = new Vector2(hpSize.x * rate, hpSize.y);
    }

    void NewRespawnPoint(Vector3 pos)
    {
        spawnPoint = pos;
    }

    void InitGame()
    {
        // Canvas UI ����
        textStage = GameObject.Find("TextStage").GetComponent<Text>();
        textCoin = GameObject.Find("TextCoin").GetComponent<Text>();
        textGem = GameObject.Find("TextGem").GetComponent<Text>();

        textScore = GameObject.Find("TextScore").GetComponent<Text>();
        textHeart = GameObject.Find("TextHeart").GetComponent<Text>();

        // PopUp UI ����
        panelMask = GameObject.Find("PanelMask");
        panelMenu = GameObject.Find("PanelMenu");
        panelGameOver = GameObject.Find("PanelGameOver");
        panelComplete = GameObject.Find("PanelComplete");

        panelOption = GameObject.Find("PanelOption");

        panelOption.SetActive(false);

        panelMask.SetActive(false);
        panelMenu.SetActive(false);
        panelGameOver.SetActive(false);
        panelComplete.SetActive(false);

        // ü�¹� �ʱ�ȭ
        hpSize = GameObject.Find("PanelTop/HP").GetComponent<RectTransform>().sizeDelta;
        hpGauge = GameObject.Find("HP/Gauge").GetComponent<RectTransform>();
        hpGauge.sizeDelta = hpSize;

        // Score Manager
        ScoreManager.Clear();
        hpMax = Settings.PlayerHP;

        Cursor.visible = false;
    }

    // �˾�â ����
    void OpenPopup (GameObject panel)
    {
        state = STATE.wait;
        Time.timeScale = 0;
        
        panelMask.SetActive(true);
        panel.SetActive(true);
        panelCurrent = panel;
        Cursor.visible = true;
    }

    // �˾�â �ݱ�
    void ClosePopup ()
    {
        panelMask.SetActive(false);
        panelCurrent.SetActive(false);

        Time.timeScale = 1;
        Cursor.visible = false;
    }

    // ��ư �̺�Ʈ
    public void OnButtonClick(GameObject button)
    {
        switch (button.name)
        {
            case "CloseButton":
            case "NoButton":
                ClosePopup();
                state = STATE.play;
                break;
            case "QuitButton":
            case "YesButton":
                ClosePopup();
                SceneManager.LoadScene("GameTitle");
                break;
            case "AgainButton":
                ClosePopup();
                ScoreManager.isStart = true;
                ScoreManager.Clear();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "OptionButton":
                panelOption.SetActive(true);
                break;
            case "OptionMenuCloseButton":
                panelOption.SetActive(false);
                break;
        }
    }

    void SetClear()
    {
        state = STATE.clear;
        Cursor.visible = true;
    }

    IEnumerator Fadeout()
    {
        // MaskPanel Ȱ��ȭ
        Image image = panelMask.GetComponent<Image>();
        panelMask.SetActive(true);

        for (float a = 0; a < 1; a += 0.02f)
        {
            image.color = new Color(0, 0, 0, a);
            yield return null;
        }
    }
    IEnumerator NextStage()
    {
        state = STATE.wait;
        yield return StartCoroutine(Fadeout());

        // ������ ������������ Ȯ��
        if (Settings.stageNum >= Settings.STAGE_COUNT)
        {
            state = STATE.complete;
            yield break;
        }

        // �������� ��ȯ
        Settings.stageNum++;
        if (Settings.stageNum > Settings.lastStage)
        {
            Settings.lastStage = Settings.stageNum;
        }

        // �÷��̾� ü�� ȸ��
        ScoreManager.hp += Settings.HP_ADD;
        hpMax = Settings.PlayerHP;

        // ���� �������� ����
        SceneManager.LoadScene("Stage" + Settings.stageNum);
    }

    void OnEnable()
    {
        string str = SceneManager.GetActiveScene().name;     // Ȱ��ȭ�� ���� �̸� ���ϱ�, ���� ��ȣ�� ���� ��쿡�� name ��� buildIndex ���
        int n = int.Parse(str.Substring(5));                 // ���ڿ��� �ټ� ��°���� ������ �ڸ��� ������ ��ȯ
        Settings.stageNum = n;
    }
}
