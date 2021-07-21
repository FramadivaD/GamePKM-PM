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
    }

    public void PlayerInteract()
    {
        if (player.PlayerInteraction == PlayerInteractionType.OpenChest)
        {
            if (player.LatestInteractionObject.TryGetComponent(out ChestContainer chest)) {
                player.questionManager.OpenQuestion(chest);
            }
            player.PlayerInteraction = PlayerInteractionType.None;
        } else if (player.PlayerInteraction == PlayerInteractionType.OpenGate)
        {
            if (player.LatestInteractionObject.TryGetComponent(out Gate gate))
            {
                player.gateManager.OpenGate(gate);
            }
            player.PlayerInteraction = PlayerInteractionType.None;
        }
    }
}
