using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    Animator transistion;
    public GameObject[] menus;
    public Sprite[] soundSprites;
    public bool isMute = false;
    public float lastSoundValue;
    void Start()
    {
        
    }

    void Update()
    {
        //Sound Monitoring
        if(GameObject.Find("SoundSlider").GetComponent<Slider>().value == 0){
            GameObject.Find("Music Button").GetComponent<Image>().sprite = soundSprites[1];
            isMute = true;
        }else if(GameObject.Find("SoundSlider").GetComponent<Slider>().value > 0){
            GameObject.Find("Music Button").GetComponent<Image>().sprite = soundSprites[0];
            isMute = false;
        }
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

    public void ExitGame(){
        //Exit Game
    }

    public void StartGame(){
        SceneManager.LoadScene("SampleScene");
    }

    public void SoundSettings(){
        if(isMute){
            //GameObject.Find("Music Button").GetComponent<Image>().sprite = soundSprites[0];
            if(lastSoundValue == 0){
                GameObject.Find("SoundSlider").GetComponent<Slider>().value += 0.1f;
            }else if(lastSoundValue != 0){
                GameObject.Find("SoundSlider").GetComponent<Slider>().value = lastSoundValue;
            }
            isMute = false;
        }
        else if(!isMute){
            //GameObject.Find("Music Button").GetComponent<Image>().sprite = soundSprites[1];
            lastSoundValue = GameObject.Find("SoundSlider").GetComponent<Slider>().value;
            GameObject.Find("SoundSlider").GetComponent<Slider>().value = 0;
            isMute = true;
        }
    }
    
}
