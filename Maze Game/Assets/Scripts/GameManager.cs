using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject login;
    public GameObject[] soal2;
    public GameObject canvas;

    public Player controlledPlayer;

    public bool joystickAllowed = true;
    public bool available = false;
    public Text InputName;

    public bool AllowEntityMove { get; private set; }
    public bool AllowEnemyMove { get; private set; }
    public bool AllowPlayerMove { get; private set; }

    public bool gateCheck;

    public Sprite treasureSprite;

    Player playerInfo;
    InventoryManager inventoryInfo;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
    }

    void SpawnSoal(){
        Instantiate(soal2[0], canvas.transform, false);
        // Allowed = false;
        available = false;
    }
    void SpawnMainSoal(){
        Instantiate(soal2[1], canvas.transform, false);
        // Allowed = false;
        gateCheck = false;
    }

    public void EnterAnswer(){
        GameObject Questions = GameObject.FindGameObjectWithTag("soal");
        Text InputAnswer = GameObject.Find("InputAnswer").GetComponent<Text>();
        QuestionScript correctAnswer = GameObject.Find("QuestionText").GetComponent<QuestionScript>();
        if(InputAnswer.text == correctAnswer.answer){
            GameObject.Find("MainGate(Clone)").GetComponent<Gate>().open = true;
            // Allowed = true;
            Destroy(GameObject.Find("GateOpen"));
            Destroy(Questions);
        }
    }

    public void InteractButton(){
        /*
        if(Allowed && available){
            SpawnSoal();
        }else if(gateCheck && Allowed){
            SpawnMainSoal();
        }
        */
    }
}
