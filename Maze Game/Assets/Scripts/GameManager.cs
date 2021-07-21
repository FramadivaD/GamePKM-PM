using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player controlledPlayer;

    public bool available = false;
    public Text InputName;

    public bool AllowEntityMove { get; private set; } = true;
    public bool AllowEnemyMove { get; private set; } = true;
    public bool AllowPlayerMove { get; private set; } = true;

    public Sprite treasureSprite;

    Player playerInfo;
    InventoryManager inventoryInfo;

    public Gate mainGate;

    private void Awake()
    {
        Instance = this;
    }
}
