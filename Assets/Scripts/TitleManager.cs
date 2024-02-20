using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 파일 입출력
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TitleManager : MonoBehaviour
{
    GameObject panelMain;
    GameObject panelStage;
    GameObject panelOption;
    GameObject panelControl;

    GameObject panelCurrent;
    GameObject imageCursor;

    public Slider musicSlider;
    public Slider sfxSlider;

    void Awake()
    {
        LoadSettings();
        InitGame();
        Cursor.visible = true;
    }

    void Update()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            musicSlider.value = 0.5f;
            sfxSlider.value = 0.5f;
        }
    }

    public void OnButtonClick(GameObject button)
    {
        switch (button.name)
        {
            case "ButtonStartGame":
                if (Settings.stageNum > Settings.STAGE_COUNT)
                {
                    Settings.stageNum = Settings.STAGE_COUNT;
                }
                SceneManager.LoadScene("Stage" + Settings.stageNum);
                break;
            case "ButtonStage":
                OpenPopup(panelStage);
                break;
            case "ButtonOption":
                OpenPopup(panelOption);
                break;
            case "ButtonControl":
                OpenPopup(panelControl);
                break;
            case "ButtonQuit":
                SaveSettings();
                Application.Quit();
                break;
            case "ButtonClose":
                ClosePopup();
                break;
        }

        if (button.name.Contains("ButtonMap"))
        {
            SetStage(button.transform);
        }
    }

    void SetStage(Transform button)
    {
        int n = int.Parse(button.Find("Text").GetComponent<Text>().text);
        Settings.stageNum = n;

        imageCursor.transform.position = button.position;
    }

    void SetMapButtons()
    {
        Button[] buttons = panelStage.GetComponentsInChildren<Button>(); // 오브젝트 하위의 모든 버튼 오브젝트 읽기
        foreach (Button button in buttons)
        {
            if (button.name == "ButtonClose") continue;                  // 닫기 버튼은 제외

            // 각 버튼의 스테이지 번호 읽기
            Text textWidget = button.transform.Find("Text").GetComponent<Text>();   // 텍스트 위젯 찾기
            int n = int.Parse(textWidget.text);
            bool enabled = (n <= Settings.lastStage);                               // 마지막 번호 이하이면 활성화

            button.enabled = enabled;
            if (!enabled)
            {
                button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.8f);
            }
        }
    }

    public void OnValueChange(Dropdown drop)
    {
        int n = drop.value;

        switch (drop.name)
        {
            case "DropLevel":
                Settings.level = n;
                break;
        }

        Debug.Log(n);
    }

    void OpenPopup(GameObject panel)
    {
        panelMain.SetActive(false);
        panel.SetActive(true);
        panelCurrent = panel;
    }

    void ClosePopup()
    {
        panelMain.SetActive(true);
        panelCurrent.SetActive(false);
    }

    void InitGame()
    {
        panelMain = GameObject.Find("PanelMain");
        panelStage = GameObject.Find("PanelStage");
        panelOption = GameObject.Find("PanelOption");
        panelControl = GameObject.Find("PanelControl");
        imageCursor = GameObject.Find("ImgCursor");

        panelStage.SetActive(false);
        panelOption.SetActive(false);
        panelControl.SetActive(false);

        SetMapButtons();
    }

    [Serializable]  // 클래스 데이터 직렬화
    class SaveData
    {
        public bool canMusic;
        public bool canSound;
        public int level;

        public int lastStage;

        public SaveData()
        {
            canMusic = Settings.canMusic;
            canSound = Settings.canSound;
            level = Settings.level;

            lastStage = Settings.lastStage;
        }
    }

    void SaveSettings()
    {
        SaveData data = new SaveData();                                  // 저장용 데이터 생성
        string path = Application.persistentDataPath + "/Settings.dat";  // 저장할 파일명

        BinaryFormatter formatter = new BinaryFormatter();               // 데이터 입출력 버퍼 생성
        FileStream file = File.Open(path, FileMode.Create);              // 새로운 파일 생성, 파일 존재시 덮어쓰기

        formatter.Serialize(file, data);                                 // 입출력 버퍼에 데이터 할당, 직렬화하여 저장
        file.Close();                                                    // 파일 닫기
    }

    void LoadSettings()
    {
        string path = Application.persistentDataPath + "/Settings.dat";
        if (!File.Exists(path)) return;                                  // 저장된 파일이 없으면 리턴

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);                // 파일 오픈
        SaveData data = (SaveData)formatter.Deserialize(file);           // 파일의 자료를 데이터에 저장
        file.Close();

        Settings.canMusic = data.canMusic;
        Settings.canSound = data.canSound;
        Settings.level = data.level;

        Settings.lastStage = data.lastStage;
    }
}