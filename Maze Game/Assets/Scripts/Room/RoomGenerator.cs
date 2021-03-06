using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    [Header("Settings")]
    bool[,] roomMap;
    RoomGeneratorGrid[,] roomMapRoom;

    private List<RoomGeneratorGrid> allRoom;
    private List<RoomGeneratorGrid> cornerRoom;
    private List<RoomGeneratorGrid> notCornerRoom;
    private List<RoomGeneratorGrid> bossRoom;

    // atas, kanan, bawah, kiri
    private Vector2Int[] mostBorderRoom = { Vector2Int.zero, Vector2Int.zero, Vector2Int.zero, Vector2Int.zero };

    public Vector2 roomWidthHeight = new Vector2(16, 10);

    [Header("Room size harus ganjil")]
    public int roomSize;
    public int roomWalkIteration = 20;

    [Header("Configuration")]
    public RoomGeneratorGrid roomPrefab;

    [Header("Item Configuration")]
    public int weaponItemCount = 3;

    [Header("Enemies Configuration")]
    public int enemiesCount = 3;

    [Header("Network")]
    public PhotonView pv;

    public void RandomizeMap()
    {
        // kalo genap, jadikan ganjil
        if (roomSize % 2 == 0) roomSize += 1;

        // generate map
        roomMap = new bool[roomSize, roomSize];
        roomMapRoom = new RoomGeneratorGrid[roomSize, roomSize];

        // make collection
        if (allRoom == null) allRoom = new List<RoomGeneratorGrid>();
        allRoom.Clear();

        if (notCornerRoom == null) notCornerRoom = new List<RoomGeneratorGrid>();
        notCornerRoom.Clear();

        if (cornerRoom == null) cornerRoom = new List<RoomGeneratorGrid>();
        cornerRoom.Clear();

        if (bossRoom == null) bossRoom = new List<RoomGeneratorGrid>();
        bossRoom.Clear();

        int midPoint = roomSize / 2;

        // reset border room
        mostBorderRoom[0] = new Vector2Int(midPoint, midPoint);
        mostBorderRoom[1] = new Vector2Int(midPoint, midPoint);
        mostBorderRoom[2] = new Vector2Int(midPoint, midPoint);
        mostBorderRoom[3] = new Vector2Int(midPoint, midPoint);

        // Generate Room on midPoint
        roomMap[midPoint, midPoint] = true;

        // Generate Array boolean to make path
        DrawMap(midPoint, midPoint, 1);
        DrawMap(midPoint - 1, midPoint, roomWalkIteration);
        DrawMap(midPoint + 1, midPoint, roomWalkIteration);
        DrawMap(midPoint, midPoint - 1, roomWalkIteration);
        DrawMap(midPoint, midPoint + 1, roomWalkIteration);

        // Instantiate Room
        BuildMap();

        // Find outer room with 3 closed door
        FindAllCornerRoom();

        FillNecessaryCornerRoom(true);

        // Spawn Random Treasure
        SpawnTreasures();

        // Spawn Random Items
        SpawnItems();

        // Spawn Enemies
        SpawnEnemies();

        // Set Corner rooms neighboroom
        SetCornerRoomNeighborRoom();

        // Spawn Random Boss on Corner Room
        SpawnBossRooms();
    }

    private void DrawMap(int x, int y, int iter)
    {
        if (iter <= 0)
        {
            return;
        }

        x = Mathf.Clamp(x, 0, roomSize);
        y = Mathf.Clamp(y, 0, roomSize);

        if (x >= 0 && y >= 0 && x < roomSize && y < roomSize) {
            roomMap[x, y] = true;

            int direction = Random.Range(0, 4);

            if (direction == 0) DrawMap(x + 1, y, iter - 1);
            else if (direction == 1) DrawMap(x - 1, y, iter - 1);
            else if (direction == 2) DrawMap(x, y + 1, iter - 1);
            else if (direction == 3) DrawMap(x, y - 1, iter - 1);
        }
    }

    private void BuildMap()
    {
        // make collection
        if (allRoom == null) allRoom = new List<RoomGeneratorGrid>();
        allRoom.Clear();

        // y pos
        for (int y = 0;y < roomSize; y++)
        {
            // x pos
            for (int x = 0; x < roomSize; x++)
            {
                if (roomMap[y, x])
                {
                    BuildRoomGrid(x, y);
                }
            }
        }
    }

    private void FindAllCornerRoom()
    {
        if (cornerRoom == null) cornerRoom = new List<RoomGeneratorGrid>();
        cornerRoom.Clear();

        if (notCornerRoom == null) notCornerRoom = new List<RoomGeneratorGrid>();
        notCornerRoom.Clear();

        // y pos
        for (int y = 0; y < roomSize; y++)
        {
            // x pos
            for (int x = 0; x < roomSize; x++)
            {
                if (roomMap[y, x])
                {
                    if (roomMapRoom[y, x].GetUnlockedDoorCount() == 3)
                    {
                        cornerRoom.Add(roomMapRoom[y, x]);
                    } else
                    {
                        notCornerRoom.Add(roomMapRoom[y, x]);
                    }
                }
            }
        }
    }

    private void FillNecessaryCornerRoom(bool forceFillAll = false)
    {
        if (forceFillAll || cornerRoom.Count < TeamHelper.GetTeamCount())
        {
            int x = mostBorderRoom[0].x;
            int y = mostBorderRoom[0].y;

            RoomGeneratorGrid room = SpawnRoom(GetRoomPos(x, y) + new Vector2(0, roomWidthHeight.y));
            room.gameObject.name = "AddonNecessaryRoomTop";
            room.Initialize();

            room.OpenBottomDoor();
            roomMapRoom[y, x].OpenTopDoor();
            cornerRoom.Add(room);
            allRoom.Add(room);

            cornerRoom.Remove(roomMapRoom[y, x]);

            room.SetNeighborRoom(null, null, roomMapRoom[y, x], null);
        }
        if (forceFillAll || cornerRoom.Count < TeamHelper.GetTeamCount())
        {
            int x = mostBorderRoom[1].x;
            int y = mostBorderRoom[1].y;

            RoomGeneratorGrid room = SpawnRoom(GetRoomPos(x, y) + new Vector2(roomWidthHeight.x, 0));
            room.gameObject.name = "AddonNecessaryRoomRight";
            room.Initialize();

            room.OpenLeftDoor();
            roomMapRoom[y, x].OpenRightDoor();
            cornerRoom.Add(room);
            allRoom.Add(room);

            cornerRoom.Remove(roomMapRoom[y, x]);

            room.SetNeighborRoom(null, null, null, roomMapRoom[y, x]);
        }

        if (forceFillAll || cornerRoom.Count < TeamHelper.GetTeamCount())
        {
            int x = mostBorderRoom[2].x;
            int y = mostBorderRoom[2].y;

            RoomGeneratorGrid room = SpawnRoom(GetRoomPos(x, y) + new Vector2(0, -roomWidthHeight.y));
            room.gameObject.name = "AddonNecessaryRoomBottom";
            room.Initialize();

            room.OpenTopDoor();
            roomMapRoom[y, x].OpenBottomDoor();
            cornerRoom.Add(room);
            allRoom.Add(room);

            cornerRoom.Remove(roomMapRoom[y, x]);

            room.SetNeighborRoom(roomMapRoom[y, x], null, null, null);
        }

        if (forceFillAll || cornerRoom.Count < TeamHelper.GetTeamCount())
        {
            int x = mostBorderRoom[3].x;
            int y = mostBorderRoom[3].y;

            RoomGeneratorGrid room = SpawnRoom(GetRoomPos(x, y) + new Vector2(-roomWidthHeight.x, 0));
            room.gameObject.name = "AddonNecessaryRoomLeft";
            room.Initialize();

            room.OpenRightDoor();
            roomMapRoom[y, x].OpenLeftDoor();
            cornerRoom.Add(room);
            allRoom.Add(room);

            cornerRoom.Remove(roomMapRoom[y, x]);

            room.SetNeighborRoom(null, roomMapRoom[y, x], null, null);
        }
    }

    private void BuildRoomGrid(int x, int y)
    {
        RoomGeneratorGrid room = SpawnRoom(x, y);

        // save room in collections
        roomMapRoom[y, x] = room;
        allRoom.Add(room);

        // save most top, right, bottom, left room place
        if (y > mostBorderRoom[0].y) mostBorderRoom[0] = new Vector2Int(x, y);
        if (x > mostBorderRoom[1].x) mostBorderRoom[1] = new Vector2Int(x, y);
        if (y < mostBorderRoom[2].y) mostBorderRoom[2] = new Vector2Int(x, y);
        if (x < mostBorderRoom[3].x) mostBorderRoom[3] = new Vector2Int(x, y);

        // awalnya pintu locked semua
        room.Initialize();

        // tapi nanti dinyalain satu persatu
        if (y - 1 >= 0 && roomMap[y - 1, x])
        {
            roomMapRoom[y - 1, x].OpenTopDoor();
            room.OpenBottomDoor();
        }
        if (x - 1 >= 0 && roomMap[y, x - 1])
        {
            roomMapRoom[y, x - 1].OpenRightDoor();
            room.OpenLeftDoor();
        }
    }

    private RoomGeneratorGrid SpawnRoom(int x, int y)
    {
        GameObject roomObject;

        if (PhotonNetwork.connected)
        {
            roomObject = PhotonNetwork.Instantiate(roomPrefab.name, GetRoomPos(x, y), Quaternion.identity, 0);
        }
        else
        {
            roomObject = Instantiate(roomPrefab.gameObject, GetRoomPos(x, y), Quaternion.identity, transform);
        }

        RoomGeneratorGrid room = roomObject.GetComponent<RoomGeneratorGrid>();

        return room;
    }

    private RoomGeneratorGrid SpawnRoom(Vector3 pos)
    {
        GameObject roomObject;

        if (PhotonNetwork.connected)
        {
            roomObject = PhotonNetwork.Instantiate(roomPrefab.name, pos, Quaternion.identity, 0);
        }
        else
        {
            roomObject = Instantiate(roomPrefab.gameObject, pos, Quaternion.identity, transform);
        }

        RoomGeneratorGrid room = roomObject.GetComponent<RoomGeneratorGrid>();

        return room;
    }

    private Vector2 GetRoomPos(int x, int y)
    {
        int midPoint = roomSize / 2;
        Vector2 origin = Vector2.zero;

        return origin + new Vector2(roomWidthHeight.x * (x - midPoint), roomWidthHeight.y * (y - midPoint));
    }

    private void SpawnTreasures()
    {
        int teamNumber = TeamHelper.GetTeamCount();

        for (int i = 0;i < teamNumber; i++)
        {
            // Team is Exist
            if (GameManager.PlayersTeam.TryGetValue(TeamHelper.TeamTypes[i], out TeamData teamData)) {

                Debug.Log("Spawning team chest : " + TeamHelper.TeamTypes[i]);

                MainGateKey key = teamData.FragmentsKey;
                int chestTreasureCount = key.Fragments.Count;

                for (int j = 0; j < chestTreasureCount; j++)
                {
                    // limit biar gak infinite
                    ChestContainer chest = null;

                    while (true)
                    {
                        int ind = Random.Range(0, notCornerRoom.Count);
                        if (ind < notCornerRoom.Count)
                        {
                            RoomGeneratorGrid selectedRoom = notCornerRoom[ind];
                            if (selectedRoom)
                            {
                                chest = selectedRoom.SpawnTreasureChest(TeamHelper.TeamTypes[i], j);

                                if (chest)
                                {
                                    // break, spot kosong ditemukan
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void SpawnItems()
    {
        int itemsCount = weaponItemCount;

        for (int j = 0; j < itemsCount; j++)
        {
            // limit biar gak infinite
            WeaponOrb weaponOrb = null;

            while (true)
            {
                int ind = Random.Range(0, notCornerRoom.Count);
                if (ind < notCornerRoom.Count)
                {
                    RoomGeneratorGrid selectedRoom = notCornerRoom[ind];
                    if (selectedRoom)
                    {
                        weaponOrb = selectedRoom.SpawnWeaponItemOrb();
                        if (weaponOrb)
                        {
                            // break, spot kosong ditemukan
                            break;
                        }
                    }
                }
            }
        }
    }

    private void SpawnEnemies()
    {
        int itemsCount = weaponItemCount;

        for (int j = 0; j < enemiesCount; j++)
        {
            // limit biar gak infinite
            GameObject enemy = null;

            while (true)
            {
                int ind = Random.Range(0, notCornerRoom.Count);
                if (ind < notCornerRoom.Count)
                {
                    RoomGeneratorGrid selectedRoom = notCornerRoom[ind];
                    if (selectedRoom)
                    {
                        enemy = selectedRoom.SpawnEnemy();
                        if (enemy)
                        {
                            // break, spot kosong ditemukan
                            break;
                        }
                    }
                }
            }
        }
    }

    private void SpawnBossRooms()
    {
        int teamNumber = TeamHelper.GetTeamCount();
        for (int i = 0; i < teamNumber && i < cornerRoom.Count; i++)
        {
            if (!bossRoom.Contains(cornerRoom[i])) {
                Debug.Log("Spawning boss.");
                RoomGeneratorGrid selectedRoom = cornerRoom[i];

                selectedRoom.SetToBossRoom(TeamHelper.TeamTypes[i]);
                bossRoom.Add(selectedRoom);
            }
        }
    }

    // FIXME : Fix error on some corner room cannot get neighbor room from boss room (fill necesaary)
    private void SetCornerRoomNeighborRoom()
    {
        for (int y = 0; y < roomSize; y++)
        {
            for (int x = 0; x < roomSize; x++)
            {
                if (roomMapRoom[y, x])
                {
                    RoomGeneratorGrid top = null;
                    RoomGeneratorGrid right = null;
                    RoomGeneratorGrid bottom = null;
                    RoomGeneratorGrid left = null;

                    if (x + 1 < roomSize) right = roomMapRoom[y, x + 1];
                    if (x - 1 >= 0) left = roomMapRoom[y, x - 1];

                    if (y + 1 < roomSize) top = roomMapRoom[y + 1, x];
                    if (y - 1 >= 0) bottom = roomMapRoom[y - 1, x];

                    roomMapRoom[y, x].SetNeighborRoom(top, right, bottom, left);
                }
            }
        }
    }
}
