using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Extensione.Audio;

public class MainMenuScript : MonoBehaviour
{
    Animator transistion;
    public GameObject[] menus;
    public Sprite[] soundSprites;
    public bool isMute = false;
    public float lastSoundValue;

    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    public Button musicVolumeButton;
    public Button soundVolumeButton;

    private void Start()
    {
        AudioVolumeInitialization();

        MainMenus();
    }

    void Update()
    {
        if (musicVolumeSlider.value <= 0) musicVolumeButton.image.sprite = soundSprites[1];
        else if (musicVolumeSlider.value > 0) musicVolumeButton.image.sprite = soundSprites[0];

        if (soundVolumeSlider.value <= 0) soundVolumeButton.image.sprite = soundSprites[1];
        else if (soundVolumeSlider.value > 0) soundVolumeButton.image.sprite = soundSprites[0];
    }

    public void SettingsMenu(){
        menus[0].SetActive(false);
        menus[1].SetActive(true);
        menus[2].SetActive(false);
        menus[3].SetActive(false);
        menus[4].SetActive(false);
    }

    public void MainMenus(){
        menus[0].SetActive(true);
        menus[1].SetActive(false);
        menus[2].SetActive(false);
        menus[3].SetActive(false);
        menus[4].SetActive(false);
    }

    public void JoinMenus()
    {
        menus[0].SetActive(false);
        menus[1].SetActive(false);
        menus[2].SetActive(true);
        menus[3].SetActive(false);
        menus[4].SetActive(false);
    }

    public void AboutMenus()
    {
        menus[0].SetActive(false);
        menus[1].SetActive(false);
        menus[2].SetActive(false);
        menus[3].SetActive(true);
        menus[4].SetActive(false);
    }

    public void CreditsMenus()
    {
        menus[0].SetActive(false);
        menus[1].SetActive(false);
        menus[2].SetActive(false);
        menus[3].SetActive(false);
        menus[4].SetActive(true);
    }

    private void AudioVolumeInitialization()
    {
        musicVolumeSlider.onValueChanged.AddListener((float x) => { AudioManager.Instance.ChangeMusicVolume(x); PlayerPrefs.SetFloat("AudioMusicVolume", x); });
        soundVolumeSlider.onValueChanged.AddListener((float x) => { AudioManager.Instance.ChangeSFXVolume(x); PlayerPrefs.SetFloat("AudioSFXVolume", x); });

        musicVolumeButton.onClick.AddListener(() => { musicVolumeSlider.value = musicVolumeSlider.value == 0 ? 1 : 0; });
        soundVolumeButton.onClick.AddListener(() => { soundVolumeSlider.value = soundVolumeSlider.value == 0 ? 1 : 0; });

        // Load Volume if exists, if not then set volume to 1.0
        if (PlayerPrefs.HasKey("AudioMusicVolume")) {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("AudioMusicVolume");
        } else
        {
            musicVolumeSlider.value = 1.0f;
        }

        if (PlayerPrefs.HasKey("AudioSFXVolume"))
        {
            soundVolumeSlider.value = PlayerPrefs.GetFloat("AudioSFXVolume");
        }
        else
        {
            soundVolumeSlider.value = 1.0f;
        }
    }
}
