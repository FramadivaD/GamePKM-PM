using UnityEngine;
using System.Collections.Generic;

public class PlayerCompass : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float clampCompassMagnitude;

    private List<GameObject> compassUI;
    private List<ChestContainer> chestTreasures;

    private GameObject compassPrefab;

    private Transform compassParent;

    private bool initialized = false;

    public void Initialize(Player player)
    {
        this.player = player;

        initialized = true;
    }

    private void Update()
    {
        if (initialized) {
            ControlCompassUI();
        }
    }

    public void FindAllChest()
    {
        if (compassUI == null) compassUI = new List<GameObject>();
        compassUI.Clear();

        if (chestTreasures == null) chestTreasures = new List<ChestContainer>();
        chestTreasures.Clear();

        foreach (ChestContainer chest in FindObjectsOfType<ChestContainer>())
        {
            if (chest.teamType == player.teamType)
            {
                chestTreasures.Add(chest);

                GameObject compass = Instantiate(compassPrefab, compassParent.transform.position, Quaternion.identity, compassParent);
                compassUI.Add(compass);
            }
        }
    }

    private void ControlCompassUI()
    {
        for (int i = 0;i < chestTreasures.Count;i++)
        {
            Vector3 direction = (chestTreasures[i].transform.position - player.transform.position);

            Vector3 clamped = Vector3.ClampMagnitude(direction, clampCompassMagnitude);

            compassUI[i].transform.position = clamped;
        }
    }
}
