using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
    [Header("Door Objects")]
    [SerializeField] private GameObject topDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject bottomDoor;
    [SerializeField] private GameObject leftDoor;

    [Header("Chest Treasure Config")]
    [SerializeField] private Transform treasureChestParent;
    [SerializeField] private GameObject treasureChestPrefab;

    [SerializeField] private Transform[] treasureChestPossiblePos;
    private GameObject[] treasureChestSpawned;

    private int latestTreasureChestPossiblePosIndex = -1;
    private int treasureChestCount = 0;

    [Header("Boss Room Config")]
    [SerializeField] private Transform bossParent;
    [SerializeField] private GameObject[] bossTypePrefab;

    private GameObject activeBossEnemy;
    public bool IsBossRoom { get; private set; } = false;
    public TeamType BossTeamType { get; private set; } = TeamType.Red;

    [Header("Main Gate Config")]
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

    public ChestContainer SpawnTreasureChest(TeamType teamType)
    {
        if (treasureChestCount < treasureChestPossiblePos.Length)
        {
            if (treasureChestSpawned == null) treasureChestSpawned = new GameObject[treasureChestPossiblePos.Length];
            if (latestTreasureChestPossiblePosIndex == -1)
            {
                latestTreasureChestPossiblePosIndex = Random.Range(0, treasureChestPossiblePos.Length);
            }

            while (true)
            {
                if (treasureChestSpawned[latestTreasureChestPossiblePosIndex] == null)
                {
                    GameObject chest = Instantiate(treasureChestPrefab, treasureChestPossiblePos[latestTreasureChestPossiblePosIndex].position, Quaternion.identity, treasureChestParent);
                    treasureChestSpawned[latestTreasureChestPossiblePosIndex] = chest;

                    ChestContainer chestContainer = chest.GetComponent<ChestContainer>();
                    chestContainer.Initialize(teamType);

                    treasureChestCount++;

                    return chestContainer;
                }

                latestTreasureChestPossiblePosIndex++;
                latestTreasureChestPossiblePosIndex %= treasureChestPossiblePos.Length;
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
        /*
        GameObject topGate = Instantiate(mainGatePrefab, );
        GameObject rightGate = Instantiate(mainGatePrefab, );
        GameObject bottomGate = Instantiate(mainGatePrefab, );
        GameObject leftGate = Instantiate(mainGatePrefab, );
        */
    }
}
