using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManage : MonoBehaviour
{
    public GameObject login;
    public GameObject[] soal2;
    public GameObject canvas;
    public GameObject JamurMerah;
    public bool joystickAllowed = true;
    public bool available = false;
    public Text InputName;
    public bool Allowed;
    public bool gateCheck;
    public Sprite treasureSprite;

    playerScript playerInfo;
    InventoryManager inventoryInfo;
    Text playerName;

    void Start()
    {
        playerName = GameObject.Find("NameTag").GetComponent<Text>();
        InputName = GameObject.Find("InputText").GetComponent<Text>();
        JamurMerah = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }

    void SpawnSoal(){
        Instantiate(soal2[0], canvas.transform, false);
        Allowed = false;
        available = false;
    }
    void SpawnMainSoal(){
        Instantiate(soal2[1], canvas.transform, false);
        Allowed = false;
        gateCheck = false;
    }

    public void EnterName(){
        if(InputName.text != "" && InputName.text.Length <= 5){
            playerName.text = InputName.text;
            Destroy(login);
            Allowed = true;
        }
    }

    public void EnterAnswer(){
        GameObject Questions = GameObject.FindGameObjectWithTag("soal");
        Text InputAnswer = GameObject.Find("InputAnswer").GetComponent<Text>();
        QuestionScript correctAnswer = GameObject.Find("QuestionText").GetComponent<QuestionScript>();
        if(InputAnswer.text == correctAnswer.answer){
            GameObject.Find("MainGate(Clone)").GetComponent<Gate>().open = true;
            Allowed = true;
            Destroy(GameObject.Find("GateOpen"));
            Destroy(Questions);
        }
    }

    public void PressButton(){
        GameObject.Find("JamurMerah").GetComponent<playerScript>().isJoystick = false;
        Debug.Log("Pressed");
    }

    public void AfterPressed(){
        GameObject.Find("JamurMerah").GetComponent<playerScript>().isJoystick = true;
        //joystickAllowed = true;
        Debug.Log("Droped");
    }

    public void InteractButton(){
        if(Allowed && available){
            SpawnSoal();
        }else if(gateCheck && Allowed){
            SpawnMainSoal();
        }
    }
}
