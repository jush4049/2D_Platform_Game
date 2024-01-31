#define RELEASE // 조건부 컴파일을 위한 심볼 생성
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
                // 게임시작
                StartGame();
                break;
            case STATE.play:
                SetScore();
                break;
            case STATE.respawn:
                // 플레이어 리스폰
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
// RELEASE라는 심볼 존재 시, 컴파일 수행
#if (RELEASE)
        // 주인공을 시작 지점으로 이동
        spawnPoint = GameObject.Find("StartPoint").transform.position;
        player = GameObject.Find("Player");
        player.transform.position = spawnPoint;

        Camera.main.transform.position = spawnPoint + camOffset;
        state = STATE.play;
#else
        Debug.Log("디버그 모드 실행");
#endif
    }

    void PlayerDead()
    {
        state = STATE.respawn;
    }

    IEnumerator PlayerRespawn()
    {
        state = STATE.wait;                              // 게임의 상태를 wait로 변경, Update() 함수 실행 중지
        yield return new WaitForSeconds(2f);

        if (--ScoreManager.heart > 0)
        {
            MakePlayer();
            Camera.main.SendMessage("SetTrack", true);   // 카메라 트래킹 재시작
            state = STATE.play;                          // 게임의 상태를 play로 변경
        }
        else
        {
            state = STATE.gameOver;                      // 생명 소진시, 게임종료
        }
    }

    void MakePlayer()
    {
        player = Instantiate(Resources.Load("Player")) as GameObject;
        player.transform.position = spawnPoint;
        player.name = "Player";                         // 플레이어 생성 시, 오브젝트 이름 변경

        // 메인 카메라
        Vector3 camPos = spawnPoint + camOffset;
        Camera.main.transform.position = camPos;        // 카메라를 리스폰 위치로 이동
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
        float rate = ScoreManager.hp / hpMax; // 플레이어의 현재 체력과 기본 체력의 비율 구하기
        hpGauge.sizeDelta = new Vector2(hpSize.x * rate, hpSize.y);
    }

    void NewRespawnPoint(Vector3 pos)
    {
        spawnPoint = pos;
    }

    void InitGame()
    {
        // UI 구성
        textStage = GameObject.Find("TextStage").GetComponent<Text>();
        textCoin = GameObject.Find("TextCoin").GetComponent<Text>();
        textGem = GameObject.Find("TextGem").GetComponent<Text>();

        textCombo = GameObject.Find("TextCombo").GetComponent<Text>();
        textScore = GameObject.Find("TextScore").GetComponent<Text>();
        textHeart = GameObject.Find("TextHeart").GetComponent<Text>();

        // 체력바 초기화
        hpSize = GameObject.Find("PanelTop/HP").GetComponent<RectTransform>().sizeDelta;
        hpGauge = GameObject.Find("HP/Gauge").GetComponent<RectTransform>();
        hpGauge.sizeDelta = hpSize;

        // Score Manager
        ScoreManager.Clear();
        hpMax = Settings.PlayerHP;

        // 배경 음악
        AudioSource music = GetComponent<AudioSource>();
        if (Settings.canMusic) music.Play();
    }
}
