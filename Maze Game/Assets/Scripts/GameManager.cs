using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player player;

    public bool AllowEntityMove { get; private set; } = true;
    public bool AllowEnemyMove { get; private set; } = true;
    public bool AllowPlayerMove { get; private set; } = true;

    private void Awake()
    {
        Instance = this;

        AllowEntityMove = false;
        player.login.OnSubmitNameSuccess += EnableAllEntitiesMovement;
    }

    private void OnDestroy()
    {
        player.login.OnSubmitNameSuccess -= EnableAllEntitiesMovement;
    }

    public void EnableAllEntitiesMovement(string str)
    {
        AllowEntityMove = true;
    }
}
