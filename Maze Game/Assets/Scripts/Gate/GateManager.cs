using UnityEngine;
using System.Collections;

public class GateManager : MonoBehaviour
{
    private Player player;

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public void OpenGate(Gate gate)
    {
        if (gate.teamType == player.teamType)
        {
            if (gate != null)
            {
                gate.OpenGate();
            } else
            {
                Debug.Log("Inequal Team Type");
            }
        }
    }
}
