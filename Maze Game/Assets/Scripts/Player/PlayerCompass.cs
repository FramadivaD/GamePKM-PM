using UnityEngine;
using System.Collections.Generic;

public class PlayerCompass : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float clampCompassMagnitude;

    private List<GameObject> compassUI;
    private List<ChestContainer> chestTreasures;

    private GameObject mainGateCompassUI;
    private Gate mainGate;

    [SerializeField] private GameObject compassPrefab;
    [SerializeField] private GameObject mainGateCompassPrefab;

    [SerializeField] private Transform compassParent;
    [SerializeField] private Transform mainGateCompassParent;

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
            ControlMainGateCompassUI();
        }
    }

    public void FindMainGate()
    {
        foreach (Transform t in mainGateCompassParent)
        {
            Destroy(t.gameObject);
        }

        if (mainGateCompassUI)
        {
            Destroy(mainGateCompassUI.gameObject);
        }

        foreach (Gate gate in FindObjectsOfType<Gate>())
        {
            if (gate.teamType == player.teamType && !gate.IsOpened)
            {
                mainGateCompassUI = Instantiate(mainGateCompassPrefab, compassParent.transform.position, Quaternion.identity, mainGateCompassParent);
                mainGate = gate;
            }
        }
    }

    public void FindAllChest()
    {
        if (compassUI == null) compassUI = new List<GameObject>();
        compassUI.Clear();

        if (chestTreasures == null) chestTreasures = new List<ChestContainer>();
        chestTreasures.Clear();

        foreach (Transform t in compassParent)
        {
            Destroy(t.gameObject);
        }

        foreach (ChestContainer chest in FindObjectsOfType<ChestContainer>())
        {
            if (chest.teamType == player.teamType && !chest.IsFragmentTaken)
            {
                chestTreasures.Add(chest);

                GameObject compass = Instantiate(compassPrefab, compassParent.transform.position, Quaternion.identity, compassParent);
                compassUI.Add(compass);
            }
        }
    }

    private void ControlCompassUI()
    {
        if (chestTreasures != null)
        {
            for (int i = 0; i < chestTreasures.Count; i++)
            {
                Vector3 direction = (chestTreasures[i].transform.position - player.transform.position);

                Vector3 clamped = Vector3.ClampMagnitude(direction, clampCompassMagnitude);

                compassUI[i].transform.position = player.transform.position + clamped;
            }
        }
    }

    private void ControlMainGateCompassUI()
    {
        if (mainGateCompassUI != null && mainGate != null)
        {
            Vector3 direction = (mainGate.transform.position - player.transform.position);

            Vector3 clamped = Vector3.ClampMagnitude(direction, clampCompassMagnitude);

            mainGateCompassUI.transform.position = player.transform.position + clamped;
        }
    }
}
