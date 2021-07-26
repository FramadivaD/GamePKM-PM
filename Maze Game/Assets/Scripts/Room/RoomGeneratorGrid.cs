﻿using UnityEngine;
using System.Collections;

public class RoomGeneratorGrid : MonoBehaviour
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

    private EnemyBoss activeBossEnemy;
    private Gate activeMainGate;

    public bool IsBossRoom { get; private set; } = false;
    public TeamType BossTeamType { get; private set; } = TeamType.Red;

    [Header("Main Gate Config")]
    [SerializeField] private Transform mainGateParent;
    [SerializeField] private GameObject mainGatePrefab;

    [SerializeField] private RoomGeneratorGrid[] neighborRoom;

    [Header("Network")]
    [SerializeField] private PhotonView pv;

    public void Initialize()
    {
        topDoor.SetActive(true);
        rightDoor.SetActive(true);
        bottomDoor.SetActive(true);
        leftDoor.SetActive(true);
    }

    public void SetNeighborRoom(RoomGeneratorGrid topRoom, RoomGeneratorGrid rightRoom, RoomGeneratorGrid bottomRoom, RoomGeneratorGrid leftRoom)
    {
        neighborRoom = new RoomGeneratorGrid[4]{ topRoom, rightRoom, bottomRoom, leftRoom };
    }

    public RoomGeneratorGrid GetRandomNeighborRoom()
    {
        int siz = 4;
        int randomIndex = Random.Range(0, siz - 1);

        for (int i = 0; i < siz;i++)
        {
            int ind = (i + randomIndex) % siz;
            if (neighborRoom[ind] != null)
            {
                return neighborRoom[ind];
            }
        }

        return null;
    }

    [PunRPC]
    public void OpenTopDoor()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            topDoor.SetActive(false);
            pv.RPC("OpenTopDoor", PhotonTargets.OthersBuffered);
        }
    }

    [PunRPC]
    public void OpenRightDoor()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            rightDoor.SetActive(false);
            pv.RPC("OpenRightDoor", PhotonTargets.OthersBuffered);
        }
    }

    [PunRPC]
    public void OpenBottomDoor()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            bottomDoor.SetActive(false);
            pv.RPC("OpenBottomDoor", PhotonTargets.OthersBuffered);
        }
    }

    [PunRPC]
    public void OpenLeftDoor()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            leftDoor.SetActive(false);
            pv.RPC("OpenLeftDoor", PhotonTargets.OthersBuffered);
        }
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
                    GameObject weapon;

                    if (PhotonNetwork.connected)
                    {
                        weapon = PhotonNetwork.Instantiate(weaponOrbPrefab.name, spawnedItemPossiblePos[latestSpawnedItemPossiblePosIndex].position, Quaternion.identity, 0);
                    } else
                    {
                        weapon = Instantiate(weaponOrbPrefab, spawnedItemPossiblePos[latestSpawnedItemPossiblePosIndex].position, Quaternion.identity, spawnedItemParent);
                    }

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
                    GameObject chest;
                    if (PhotonNetwork.connected)
                    {
                        chest = PhotonNetwork.Instantiate(treasureChestPrefab.name, spawnedItemPossiblePos[latestSpawnedItemPossiblePosIndex].position, Quaternion.identity, 0);
                    } else
                    {
                        chest = Instantiate(treasureChestPrefab, spawnedItemPossiblePos[latestSpawnedItemPossiblePosIndex].position, Quaternion.identity, spawnedItemParent);
                    }

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

        activeBossEnemy.Initialize(teamType, activeMainGate);
        activeBossEnemy.ChangeMoveTarget(transform);
    }

    private void SpawnBoss(TeamType teamType)
    {
        GameObject boss;
        if (PhotonNetwork.connected)
        {
            boss = PhotonNetwork.Instantiate(bossTypePrefab[(int)teamType].name, bossParent.position, Quaternion.identity, 0);
        }
        else
        {
            boss = Instantiate(bossTypePrefab[(int)teamType], bossParent);
        }

        activeBossEnemy = boss.GetComponent<EnemyBoss>();
    }

    private void ChangeDoorToMainGate(TeamType teamType)
    {
        GameManager.PlayersTeam.TryGetValue(teamType, out TeamData teamData);
        
        MainGateKey mainKey = teamData != null ? teamData.FragmentsKey : new MainGateKey(teamType);

        if (!topDoor.activeSelf)
        {
            GameObject topGate;

            if (PhotonNetwork.connected)
            {
                topGate = PhotonNetwork.Instantiate(mainGatePrefab.name, topDoor.transform.position, Quaternion.Euler(0, 0, -90), 0);
            }
            else
            {
                topGate = Instantiate(mainGatePrefab, topDoor.transform.position, Quaternion.Euler(0, 0, -90), mainGateParent);
            }

            Destroy(topDoor);
            topDoor = topGate;

            Gate gate = topGate.GetComponent<Gate>();
            gate.Initialize(teamType, mainKey);

            activeMainGate = gate;
        }

        if (!rightDoor.activeSelf)
        {
            GameObject rightGate;

            if (PhotonNetwork.connected)
            {
                rightGate = PhotonNetwork.Instantiate(mainGatePrefab.name, rightDoor.transform.position, Quaternion.Euler(0, 0, 180), 0);
            }
            else
            {
                rightGate = Instantiate(mainGatePrefab, rightDoor.transform.position, Quaternion.Euler(0, 0, 180), mainGateParent);
            }

            Destroy(rightDoor);
            rightDoor = rightGate;

            Gate gate = rightGate.GetComponent<Gate>();
            gate.Initialize(teamType, mainKey);

            activeMainGate = gate;
        }

        if (!bottomDoor.activeSelf)
        {
            GameObject bottomGate;

            if (PhotonNetwork.connected)
            {
                bottomGate = PhotonNetwork.Instantiate(mainGatePrefab.name, bottomDoor.transform.position, Quaternion.Euler(0, 0, 90), 0);
            }
            else
            {
                bottomGate = Instantiate(mainGatePrefab, bottomDoor.transform.position, Quaternion.Euler(0, 0, 90), mainGateParent);
            }

            Destroy(bottomDoor);
            bottomDoor = bottomGate;

            Gate gate = bottomGate.GetComponent<Gate>();
            gate.Initialize(teamType, mainKey);

            activeMainGate = gate;
        }

        if (!leftDoor.activeSelf)
        {
            GameObject leftGate;

            if (PhotonNetwork.connected)
            {
                leftGate = PhotonNetwork.Instantiate(mainGatePrefab.name, leftDoor.transform.position, Quaternion.Euler(0, 0, 0), 0);
            }
            else
            {
                leftGate = Instantiate(mainGatePrefab, leftDoor.transform.position, Quaternion.Euler(0, 0, 0), mainGateParent);
            }

            Destroy(leftDoor);
            leftDoor = leftGate;

            Gate gate = leftGate.GetComponent<Gate>();
            gate.Initialize(teamType, mainKey);

            activeMainGate = gate;
        }
    }
}
