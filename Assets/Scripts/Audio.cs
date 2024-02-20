using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour
{
    AudioSource music;
    public AudioClip[] audioGroups;

    public static Audio Instance;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            // �ٸ� �ν��Ͻ� ���� ��, ���� ������Ʈ �ı�
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += LoadedsceneEvent;

        // ��� ����
        music = GetComponent<AudioSource>();
        if (Settings.canMusic) music.Play();
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("���� �� : " + scene.name);

        if (SceneManager.GetActiveScene().name == "GameTitle") // �� ��ü ��
        {
            music.clip = audioGroups[0]; //����� Ŭ�� ��ü
            if (Settings.canMusic) music.Play();
        }

        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            music.clip = audioGroups[1];
            if (Settings.canMusic) music.Play();
        }

    }
}
