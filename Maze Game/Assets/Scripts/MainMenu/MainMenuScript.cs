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
        musicVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.ChangeMusicVolume);
        soundVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.ChangeSFXVolume);

        musicVolumeButton.onClick.AddListener(() => { musicVolumeSlider.value = musicVolumeSlider.value == 0 ? 1 : 0; });
        soundVolumeButton.onClick.AddListener(() => { soundVolumeSlider.value = soundVolumeSlider.value == 0 ? 1 : 0; });

        musicVolumeSlider.value = 1.0f;
        soundVolumeSlider.value = 1.0f;
    }

    void Update()
    {
        if (musicVolumeSlider.value <= 0) musicVolumeButton.image.sprite = soundSprites[1];
        else if (musicVolumeSlider.value > 0) musicVolumeButton.image.sprite = soundSprites[0];

        if (soundVolumeSlider.value <= 0) soundVolumeButton.image.sprite = soundSprites[1];
        else if (soundVolumeSlider.value > 0) soundVolumeButton.image.sprite = soundSprites[0];
    }

    public void SettingsMenu(){
        menus[0].transform.localPosition = Vector2.right * -1601f;
        menus[1].transform.localPosition = Vector2.zero;
        menus[2].transform.localPosition = Vector2.right * 1601f;
    }

    public void MainMenus(){
        menus[0].transform.localPosition = Vector2.zero;
        menus[1].transform.localPosition = Vector2.up * 901f;
        menus[2].transform.localPosition = Vector2.right * 1601f;
    }

    public void JoinMenus(){
        menus[0].transform.localPosition = Vector2.right * -1601f;
        menus[1].transform.localPosition = Vector2.up * 901f;
        menus[2].transform.localPosition = Vector2.zero;
    }
}
