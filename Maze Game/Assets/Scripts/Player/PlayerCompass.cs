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

    private GameObject enemyBossCompassUI;
    private EnemyBoss enemyBoss;

    [SerializeField] private GameObject compassPrefab;
    [SerializeField] private GameObject mainGateCompassPrefab;
    [SerializeField] private GameObject enemyBossCompassPrefab;

    [SerializeField] private Transform compassParent;
    [SerializeField] private Transform mainGateCompassParent;
    [SerializeField] private Transform enemyBossCompassParent;

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
            ControlEnemyBossCompassUI();
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

    public void FindEnemyBoss()
    {
        foreach (Transform t in enemyBossCompassParent)
        {
            Destroy(t.gameObject);
        }

        if (enemyBossCompassUI)
        {
            Destroy(enemyBossCompassUI.gameObject);
        }

        foreach (EnemyBoss boss in FindObjectsOfType<EnemyBoss>())
        {
            if (boss.TeamType == player.teamType)
            {
                enemyBossCompassUI = Instantiate(enemyBossCompassPrefab, compassParent.transform.position, Quaternion.identity, enemyBossCompassParent);
                enemyBoss = boss;
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

    private void ControlEnemyBossCompassUI()
    {
        if (enemyBossCompassUI != null && enemyBoss != null)
        {
            Vector3 direction = (enemyBoss.transform.position - player.transform.position);

            Vector3 clamped = Vector3.ClampMagnitude(direction, clampCompassMagnitude);

            enemyBossCompassUI.transform.position = player.transform.position + clamped;
        }
    }
}
