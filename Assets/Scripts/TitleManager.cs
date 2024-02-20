using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ���� �����
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
        Button[] buttons = panelStage.GetComponentsInChildren<Button>(); // ������Ʈ ������ ��� ��ư ������Ʈ �б�
        foreach (Button button in buttons)
        {
            if (button.name == "ButtonClose") continue;                  // �ݱ� ��ư�� ����

            // �� ��ư�� �������� ��ȣ �б�
            Text textWidget = button.transform.Find("Text").GetComponent<Text>();   // �ؽ�Ʈ ���� ã��
            int n = int.Parse(textWidget.text);
            bool enabled = (n <= Settings.lastStage);                               // ������ ��ȣ �����̸� Ȱ��ȭ

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

    [Serializable]  // Ŭ���� ������ ����ȭ
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
        SaveData data = new SaveData();                                  // ����� ������ ����
        string path = Application.persistentDataPath + "/Settings.dat";  // ������ ���ϸ�

        BinaryFormatter formatter = new BinaryFormatter();               // ������ ����� ���� ����
        FileStream file = File.Open(path, FileMode.Create);              // ���ο� ���� ����, ���� ����� �����

        formatter.Serialize(file, data);                                 // ����� ���ۿ� ������ �Ҵ�, ����ȭ�Ͽ� ����
        file.Close();                                                    // ���� �ݱ�
    }

    void LoadSettings()
    {
        string path = Application.persistentDataPath + "/Settings.dat";
        if (!File.Exists(path)) return;                                  // ����� ������ ������ ����

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);                // ���� ����
        SaveData data = (SaveData)formatter.Deserialize(file);           // ������ �ڷḦ �����Ϳ� ����
        file.Close();

        Settings.canMusic = data.canMusic;
        Settings.canSound = data.canSound;
        Settings.level = data.level;

        Settings.lastStage = data.lastStage;
    }
}