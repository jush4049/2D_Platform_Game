#define RELEASE // 조건부 컴파일을 위한 심볼 생성
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
                // 게임시작
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
                // 플레이어 리스폰
                StartCoroutine(PlayerRespawn());
                break;
            case STATE.clear:
                // 스테이지 클리어
                StartCoroutine(NextStage());
                break;
            case STATE.complete:
                // 미션 성공
                OpenPopup(panelComplete);
                break;
            case STATE.gameOver:
                // 게임 종료
                OpenPopup(panelGameOver);
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
        // Canvas UI 구성
        textStage = GameObject.Find("TextStage").GetComponent<Text>();
        textCoin = GameObject.Find("TextCoin").GetComponent<Text>();
        textGem = GameObject.Find("TextGem").GetComponent<Text>();

        textScore = GameObject.Find("TextScore").GetComponent<Text>();
        textHeart = GameObject.Find("TextHeart").GetComponent<Text>();

        // PopUp UI 구성
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

        // 체력바 초기화
        hpSize = GameObject.Find("PanelTop/HP").GetComponent<RectTransform>().sizeDelta;
        hpGauge = GameObject.Find("HP/Gauge").GetComponent<RectTransform>();
        hpGauge.sizeDelta = hpSize;

        // Score Manager
        ScoreManager.Clear();
        hpMax = Settings.PlayerHP;

        Cursor.visible = false;
    }

    // 팝업창 열기
    void OpenPopup (GameObject panel)
    {
        state = STATE.wait;
        Time.timeScale = 0;
        
        panelMask.SetActive(true);
        panel.SetActive(true);
        panelCurrent = panel;
        Cursor.visible = true;
    }

    // 팝업창 닫기
    void ClosePopup ()
    {
        panelMask.SetActive(false);
        panelCurrent.SetActive(false);

        Time.timeScale = 1;
        Cursor.visible = false;
    }

    // 버튼 이벤트
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
        // MaskPanel 활성화
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

        // 마지막 스테이지인지 확인
        if (Settings.stageNum >= Settings.STAGE_COUNT)
        {
            state = STATE.complete;
            yield break;
        }

        // 스테이지 전환
        Settings.stageNum++;
        if (Settings.stageNum > Settings.lastStage)
        {
            Settings.lastStage = Settings.stageNum;
        }

        // 플레이어 체력 회복
        ScoreManager.hp += Settings.HP_ADD;
        hpMax = Settings.PlayerHP;

        // 다음 스테이지 입장
        SceneManager.LoadScene("Stage" + Settings.stageNum);
    }

    void OnEnable()
    {
        string str = SceneManager.GetActiveScene().name;     // 활성화된 씬의 이름 구하기, 씬의 번호를 구할 경우에는 name 대신 buildIndex 사용
        int n = int.Parse(str.Substring(5));                 // 문자열의 다섯 번째부터 끝까지 자르고 정수로 변환
        Settings.stageNum = n;
    }
}
