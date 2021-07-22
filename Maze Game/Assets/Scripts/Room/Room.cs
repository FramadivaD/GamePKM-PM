using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
    [Header("Door Objects")]
    [SerializeField] private GameObject topDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject bottomDoor;
    [SerializeField] private GameObject leftDoor;

    [Header("Spawn Item Config")]
    [SerializeField] private Transform spawnedItemParent;
    [SerializeField] private Transform[] spawnedItemPossiblePos;
    private GameObject[] spawnedItemSpawned;

    private int latestSpawnedItemPossiblePosIndex = -1;
    private int spawnedItemCount = 0;

    [Header("Chest Treasure Config")]
    [SerializeField] private GameObject treasureChestPrefab;

    [Header("Weapon Orb Config")]
    [SerializeField] private GameObject weaponOrbPrefab;

    [Header("Boss Room Config")]
    [SerializeField] private Transform bossParent;
    [SerializeField] private GameObject[] bossTypePrefab;

    private GameObject activeBossEnemy;
    public bool IsBossRoom { get; private set; } = false;
    public TeamType BossTeamType { get; private set; } = TeamType.Red;

    [Header("Main Gate Config")]
    [SerializeField] private Transform mainGateParent;
    [SerializeField] private GameObject mainGatePrefab;

    public void Initialize()
    {
        topDoor.SetActive(true);
        rightDoor.SetActive(true);
        bottomDoor.SetActive(true);
        leftDoor.SetActive(true);
    }

    public void OpenTopDoor()
    {
        topDoor.SetActive(false);
    }

    public void OpenRightDoor()
    {
        rightDoor.SetActive(false);
    }

    public void OpenBottomDoor()
    {
        bottomDoor.SetActive(false);
    }

    public void OpenLeftDoor()
    {
        leftDoor.SetActive(false);
    }

    public int GetUnlockedDoorCount()
    {
        int val = 0;
        val += topDoor.activeSelf ? 1 : 0;
        val += rightDoor.activeSelf ? 1 : 0;
        val += bottomDoor.activeSelf ? 1 : 0;
        val += leftDoor.activeSelf ? 1 : 0;

        return val;

    }

    public WeaponOrb SpawnWeaponItemOrb()
    {
        if (spawnedItemCount < spawnedItemPossiblePos.Length)
        {
            if (spawnedItemSpawned == null) spawnedItemSpawned = new GameObject[spawnedItemPossiblePos.Length];
            if (latestSpawnedItemPossiblePosIndex == -1)
            {
                latestSpawnedItemPossiblePosIndex = Random.Range(0, spawnedItemPossiblePos.Length);
            }

            while (true)
            {
                if (spawnedItemSpawned[latestSpawnedItemPossiblePosIndex] == null)
                {
                    GameObject weapon = Instantiate(weaponOrbPrefab, spawnedItemPossiblePos[latestSpawnedItemPossiblePosIndex].position, Quaternion.identity, spawnedItemParent);
                    spawnedItemSpawned[latestSpawnedItemPossiblePosIndex] = weapon;

                    WeaponOrb weaponOrb = weapon.GetComponent<WeaponOrb>();

                    // Saat ini hanya random Basoka
                    weaponOrb.Initialize(new WeaponInventory() { weaponType = WeaponType.Basoka });

                    spawnedItemCount++;

                    return weaponOrb;
                }

                latestSpawnedItemPossiblePosIndex++;
                latestSpawnedItemPossiblePosIndex %= spawnedItemPossiblePos.Length;
            }
        }
        return null;
    }

    public ChestContainer SpawnTreasureChest(TeamType teamType, MainGateFragment fragment)
    {
        if (spawnedItemCount < spawnedItemPossiblePos.Length)
        {
            if (spawnedItemSpawned == null) spawnedItemSpawned = new GameObject[spawnedItemPossiblePos.Length];
            if (latestSpawnedItemPossiblePosIndex == -1)
            {
                latestSpawnedItemPossiblePosIndex = Random.Range(0, spawnedItemPossiblePos.Length);
            }

            while (true)
            {
                if (spawnedItemSpawned[latestSpawnedItemPossiblePosIndex] == null)
                {
                    GameObject chest = Instantiate(treasureChestPrefab, spawnedItemPossiblePos[latestSpawnedItemPossiblePosIndex].position, Quaternion.identity, spawnedItemParent);
                    spawnedItemSpawned[latestSpawnedItemPossiblePosIndex] = chest;

                    ChestContainer chestContainer = chest.GetComponent<ChestContainer>();
                    chestContainer.Initialize(teamType, fragment);

                    spawnedItemCount++;

                    return chestContainer;
                }

                latestSpawnedItemPossiblePosIndex++;
                latestSpawnedItemPossiblePosIndex %= spawnedItemPossiblePos.Length;
            }
        }
        return null;
    }

    public void SetToBossRoom(TeamType teamType)
    {
        IsBossRoom = true;
        BossTeamType = teamType;

        SpawnBoss(teamType);
        ChangeDoorToMainGate(teamType);
    }

    private void SpawnBoss(TeamType teamType)
    {
        activeBossEnemy = Instantiate(bossTypePrefab[(int)teamType], bossParent);
    }

    private void ChangeDoorToMainGate(TeamType teamType)
    {
        GameManager.PlayersTeam.TryGetValue(teamType, out TeamData teamData);
        
        MainGateKey mainKey = teamData != null ? teamData.FragmentsKey : new MainGateKey(teamType);

        if (!topDoor.activeSelf)
        {
            GameObject topGate = Instantiate(mainGatePrefab, topDoor.transform.position, Quaternion.Euler(0, 0, -90), mainGateParent);
            Destroy(topDoor);
            topDoor = topGate;

            Gate gate = topGate.GetComponent<Gate>();
            gate.Initialize(teamType, mainKey);
        }

        if (!rightDoor.activeSelf)
        {
            GameObject rightGate = Instantiate(mainGatePrefab, rightDoor.transform.position, Quaternion.Euler(0, 0, 180), mainGateParent);
            Destroy(rightDoor);
            rightDoor = rightGate;

            Gate gate = rightGate.GetComponent<Gate>();
            gate.Initialize(teamType, mainKey);
        }

        if (!bottomDoor.activeSelf)
        {
            GameObject bottomGate = Instantiate(mainGatePrefab, bottomDoor.transform.position, Quaternion.Euler(0, 0, 90), mainGateParent);
            Destroy(bottomDoor);
            bottomDoor = bottomGate;

            Gate gate = bottomGate.GetComponent<Gate>();
            gate.Initialize(teamType, mainKey);
        }

        if (!leftDoor.activeSelf)
        {
            GameObject leftGate = Instantiate(mainGatePrefab, leftDoor.transform.position, Quaternion.Euler(0, 0, 0), mainGateParent);
            Destroy(leftDoor);
            leftDoor = leftGate;

            Gate gate = leftGate.GetComponent<Gate>();
            gate.Initialize(teamType, mainKey);
        }
    }

    [System.Obsolete()]
    private void ChangeDoorToMainGateLegacy(TeamType teamType)
    {
        GameObject topGate = Instantiate(mainGatePrefab, topDoor.transform.position, Quaternion.Euler(0, 0, 90), mainGateParent);
        GameObject rightGate = Instantiate(mainGatePrefab, rightDoor.transform.position, Quaternion.identity, mainGateParent);
        GameObject bottomGate = Instantiate(mainGatePrefab, bottomDoor.transform.position, Quaternion.Euler(0, 0, 90), mainGateParent);
        GameObject leftGate = Instantiate(mainGatePrefab, leftDoor.transform.position, Quaternion.identity, mainGateParent);

        Destroy(topDoor);
        Destroy(rightDoor);
        Destroy(bottomDoor);
        Destroy(leftDoor);

        topDoor = topGate;
        rightDoor = rightGate;
        bottomDoor = bottomGate;
        leftDoor = leftGate;
    }
}
