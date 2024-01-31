#define RELEASE // ���Ǻ� �������� ���� �ɺ� ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game State
    enum STATE {start, play, wait, respawn, clear, complete, gameOver};
    STATE state;
    // Top Panel UI
    Text textStage;
    Text textCoin;
    Text textGem;
    Text textCombo;
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


    void Awake()
    {
        InitGame();
        state = STATE.start;
    }

    void Update()
    {
        // SetScore();

        switch (state)
        {
            case STATE.start:
                // ���ӽ���
                StartGame();
                break;
            case STATE.play:
                SetScore();
                break;
            case STATE.respawn:
                // �÷��̾� ������
                StartCoroutine(PlayerRespawn());
                break;
            case STATE.clear:
                //
                break;
            case STATE.complete:
                //
                break;
            case STATE.gameOver:
                //
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
        textCombo.text = ScoreManager.comboCount.ToString();
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
        // UI ����
        textStage = GameObject.Find("TextStage").GetComponent<Text>();
        textCoin = GameObject.Find("TextCoin").GetComponent<Text>();
        textGem = GameObject.Find("TextGem").GetComponent<Text>();

        textCombo = GameObject.Find("TextCombo").GetComponent<Text>();
        textScore = GameObject.Find("TextScore").GetComponent<Text>();
        textHeart = GameObject.Find("TextHeart").GetComponent<Text>();

        // ü�¹� �ʱ�ȭ
        hpSize = GameObject.Find("PanelTop/HP").GetComponent<RectTransform>().sizeDelta;
        hpGauge = GameObject.Find("HP/Gauge").GetComponent<RectTransform>();
        hpGauge.sizeDelta = hpSize;

        // Score Manager
        ScoreManager.Clear();
        hpMax = Settings.PlayerHP;

        // ��� ����
        AudioSource music = GetComponent<AudioSource>();
        if (Settings.canMusic) music.Play();
    }
}
