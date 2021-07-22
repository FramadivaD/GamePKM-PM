using UnityEngine;
using System.Collections;

public class GateManager : MonoBehaviour
{
    private Player player;

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public void OpenGate(Gate gate, InventoryManager inventory)
    {
        if (gate.teamType == player.teamType)
        {
            if (gate != null)
            {
                if (gate.CheckGateShouldBeOpen())
                {
                    Debug.Log("its match. Open Gate Now!");
                    gate.OpenGate();
                }
                else if (gate.CheckGateIsReadyReordering())
                {
                    Debug.Log("open game Reordering now!");
                    StartReorderingMiniGame();
                }
                else
                {
                    Debug.Log("Store item key into the gate!");
                    gate.TryOpenGate(inventory);
                }
            } else
            {
                Debug.Log("Inequal Team Type");
            }
        }
    }

    public void StartReorderingMiniGame()
    {
        
    }
}
