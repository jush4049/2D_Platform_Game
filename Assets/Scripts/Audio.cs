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
            // 다른 인스턴스 존재 시, 현재 오브젝트 파괴
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

        // 배경 음악
        music = GetComponent<AudioSource>();
        if (Settings.canMusic) music.Play();
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("현재 씬 : " + scene.name);

        if (SceneManager.GetActiveScene().name == "GameTitle") // 씬 교체 시
        {
            music.clip = audioGroups[0]; //오디오 클립 교체
            if (Settings.canMusic) music.Play();
        }

        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            music.clip = audioGroups[1];
            if (Settings.canMusic) music.Play();
        }

    }
}
