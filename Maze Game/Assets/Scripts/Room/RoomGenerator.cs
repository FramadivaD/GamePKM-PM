﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{

    [Header("Settings")]
    bool[,] roomMap;
    Room[,] roomMapRoom;

    private List<Room> allRoom;
    private List<Room> cornerRoom;
    private List<Room> notCornerRoom;

    // atas, kanan, bawah, kiri
    private Vector2Int[] mostBorderRoom = { Vector2Int.zero, Vector2Int.zero, Vector2Int.zero, Vector2Int.zero };

    public Vector2 roomWidthHeight = new Vector2(16, 10);

    [Header("Room size harus ganjil")]
    public int roomSize;
    public int roomWalkIteration = 20;

    [Header("Configuration")]
    public Room roomPrefab;

    [Header("Treasure Chest")]
    public int treasureChestPerTeamCount = 5;

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void RandomizeMap()
    {
        // kalo genap, jadikan ganjil
        if (roomSize % 2 == 0) roomSize += 1;

        // generate map
        roomMap = new bool[roomSize, roomSize];
        roomMapRoom = new Room[roomSize, roomSize];

        // make collection
        if (allRoom == null) allRoom = new List<Room>();
        allRoom.Clear();

        if (notCornerRoom == null) notCornerRoom = new List<Room>();
        notCornerRoom.Clear();

        if (cornerRoom == null) cornerRoom = new List<Room>();
        cornerRoom.Clear();

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

        FillNecessaryCornerRoom();
        FillNecessaryCornerRoom();
        FillNecessaryCornerRoom();
        FillNecessaryCornerRoom();

        // Spawn Random Treasure
        SpawnTreasures();

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
        if (allRoom == null) allRoom = new List<Room>();
        allRoom.Clear();

        // y pos
        for (int y = 0;y < roomSize; y++)
        {
            // x pos
            for (int x = 0; x < roomSize; x++)
            {
                if (roomMap[y, x])
                {
                    Room room = SpawnRoom(x, y);

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
            }
        }
    }

    private void FindAllCornerRoom()
    {
        if (cornerRoom == null) cornerRoom = new List<Room>();
        cornerRoom.Clear();

        if (notCornerRoom == null) notCornerRoom = new List<Room>();
        notCornerRoom.Clear();

        // y pos
        for (int y = 0; y < roomSize; y++)
        {
            // x pos
            for (int x = 0; x < roomSize; x++)
            {
                if (roomMap[y, x])
                {
                    if (roomMapRoom[y, x].GetUnlockedDoorCount() >= 3)
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

    private void FillNecessaryCornerRoom()
    {
        if (cornerRoom.Count < TeamHelper.GetTeamCount())
        {
            int randoming = Random.Range(0, 4);

            if (randoming == 0)
            {
                int x = mostBorderRoom[0].x;
                int y = mostBorderRoom[0].y;

                Room room = SpawnRoom(x, y);
                room.transform.position += new Vector3(0, roomWidthHeight.y, 0);
                room.gameObject.name = "AddonNecessaryRoomTop";
                room.Initialize();

                room.OpenBottomDoor();
                roomMapRoom[y, x].OpenTopDoor();
                cornerRoom.Add(room);
                allRoom.Add(room);
            }
            else if (randoming == 1)
            {
                int x = mostBorderRoom[1].x;
                int y = mostBorderRoom[1].y;

                Room room = SpawnRoom(x, y);
                room.transform.position += new Vector3(roomWidthHeight.x, 0, 0);
                room.gameObject.name = "AddonNecessaryRoomRight";
                room.Initialize();

                room.OpenLeftDoor();
                roomMapRoom[y, x].OpenRightDoor();
                cornerRoom.Add(room);
                allRoom.Add(room);
            }
            else if (randoming == 2)
            {
                int x = mostBorderRoom[2].x;
                int y = mostBorderRoom[2].y;

                Room room = SpawnRoom(x, y);
                room.transform.position += new Vector3(0, -roomWidthHeight.y, 0);
                room.gameObject.name = "AddonNecessaryRoomBottom";
                room.Initialize();

                room.OpenTopDoor();
                roomMapRoom[y, x].OpenBottomDoor();
                cornerRoom.Add(room);
                allRoom.Add(room);
            }
            else if (randoming == 3)
            {
                int x = mostBorderRoom[3].x;
                int y = mostBorderRoom[3].y;

                Room room = SpawnRoom(x, y);
                room.transform.position += new Vector3(-roomWidthHeight.x, 0, 0);
                room.gameObject.name = "AddonNecessaryRoomLeft";
                room.Initialize();

                room.OpenRightDoor();
                roomMapRoom[y, x].OpenLeftDoor();
                cornerRoom.Add(room);
                allRoom.Add(room);
            }
        }
    }

    private Room SpawnRoom(int x, int y)
    {
        Room room = Instantiate(roomPrefab.gameObject, GetRoomPos(x, y), Quaternion.identity, transform).GetComponent<Room>();
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
            for (int j = 0; j < treasureChestPerTeamCount; j++)
            {
                int ind = Random.Range(0, notCornerRoom.Count);
                if (ind < notCornerRoom.Count) {
                    Room selectedRoom = notCornerRoom[ind];
                    if (selectedRoom)
                    {
                        // limit biar gak infinite
                        for (int k = 0; k < 10; k++)
                        {
                            ChestContainer chest = selectedRoom.SpawnTreasureChest(TeamHelper.TeamTypes[i]);
                            if (chest)
                            {
                                break;
                            }
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
            Debug.Log("Spawning boss.");
            Room selectedRoom = cornerRoom[i];
            selectedRoom.SetToBossRoom(TeamHelper.TeamTypes[i]);
        }
    }
}
