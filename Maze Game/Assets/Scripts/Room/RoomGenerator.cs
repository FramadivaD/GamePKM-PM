using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{

    [Header("Settings")]
    bool[,] roomMap;
    Room[,] roomMapRoom;

    private List<Room> allRoom;
    private List<Room> cornerRoom;
    private List<Room> notCornerRoom;

    public Vector2 roomWidthHeight = new Vector2(16, 10);

    [Header("Room size harus ganjil")]
    public int roomSize;
    public int roomWalkIteration = 20;

    [Header("Configuration")]
    public Room roomPrefab;

    [Header("Treasure Chest")]
    public int treasureChestPerTeamCount = 5;

    public void RandomizeMap()
    {
        // kalo genap, jadikan ganjil
        if (roomSize % 2 == 0) roomSize += 1;

        // generate map
        roomMap = new bool[roomSize, roomSize];
        roomMapRoom = new Room[roomSize, roomSize];

        int midPoint = roomSize / 2;

        roomMap[midPoint, midPoint] = true;

        // Generate Array boolean to make path
        DrawMap(midPoint - 1, midPoint, roomWalkIteration);
        DrawMap(midPoint + 1, midPoint, roomWalkIteration);
        DrawMap(midPoint, midPoint - 1, roomWalkIteration);
        DrawMap(midPoint, midPoint + 1, roomWalkIteration);

        // Instantiate Room
        BuildMap();

        // Find outer room with 3 closed door
        FindAllCornerRoom();

        // Spawn Random Treasure
        SpawnTreasures();
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
                    Room room = Instantiate(roomPrefab.gameObject, GetRoomPos(x, y), Quaternion.identity, transform).GetComponent<Room>();

                    // save room in collections
                    roomMapRoom[y, x] = room;
                    allRoom.Add(room);

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
