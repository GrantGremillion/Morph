using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class RoomFirstLevelGenerator : SimpleRandomWalkGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms;

    private float gridSizeMultiplier = 0.15f;

    [SerializeField]
    private Transform player;

    private Dictionary<Vector2Int, BoundsInt> roomBoundsDictionary = new Dictionary<Vector2Int, BoundsInt>();

    private Dictionary<Vector2Int, RoomType> roomTypes = new Dictionary<Vector2Int, RoomType>();

    private enum RoomType
    {
        Start,
        Normal,
        Treasure,
        Enemy,
        Shop,
        Boss
    }

    public GameObject box;
    public Shop shop;
    public EnemySpawner enemySpawner;

    [SerializeField]
    private Tilemap wallTilemap;

    private bool levelGenerated;

    public void Start() 
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        levelGenerated = false;
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Lvl1" && levelGenerated == false )
        {
            tilemapVisualizer.Clear();
            RunProceduralGeneration();
            levelGenerated = true;
        }
    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int
            (dungeonWidth,dungeonHeight,0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = randomWalkRooms ? CreateRoomsRandomly(roomsList) : CreateSimpleRooms(roomsList);


        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            Vector2Int roomCenter = (Vector2Int)Vector3Int.RoundToInt(room.center);
            roomCenters.Add(roomCenter);

            // Store each room's bounds in the dictionary.
            roomBoundsDictionary[roomCenter] = room;
        }


        // for (int i = 0; i < roomCenters.Count; i++)
        // {
        //     print(roomCenters[i]);
        // }

        player.position = new Vector3(roomCenters[0][0]*gridSizeMultiplier,roomCenters[0][1]*gridSizeMultiplier, player.position.z);
        //print(player.position);

        AssignRoomTypes(roomCenters);

        foreach (var room in roomTypes)
        {
            //print(room);
        }
        foreach(var bounds in roomBoundsDictionary)
        {
            //print(bounds);
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor,false);
        WallGenerator.CreateWalls(floor,tilemapVisualizer);

        SpawnEntitiesBasedOnRoomTypes();

    }

    private void SpawnEntitiesBasedOnRoomTypes()
    {
        foreach (var roomCenter in roomTypes.Keys)
        {
            if (roomTypes[roomCenter] == RoomType.Treasure)
            {
                BoundsInt bounds = roomBoundsDictionary[roomCenter];
                //print(bounds);
                Instantiate(box,new Vector3 (roomCenter.x*gridSizeMultiplier,roomCenter.y*gridSizeMultiplier,0),Quaternion.identity);
            }
            if (roomTypes[roomCenter] == RoomType.Shop)
            {
                BoundsInt bounds = roomBoundsDictionary[roomCenter];
                shop.transform.position = new Vector3 (roomCenter.x*gridSizeMultiplier+.3f,roomCenter.y*gridSizeMultiplier,0);
            }
            if (roomTypes[roomCenter] == RoomType.Enemy)
            {
                BoundsInt bounds = roomBoundsDictionary[roomCenter];
                GenerateEnemies(roomCenter);
                
            }
        }
    }

    private void GenerateEnemies(Vector2Int roomCenter)
    {
        Instantiate(enemySpawner, new Vector3 (roomCenter.x*gridSizeMultiplier+.3f,roomCenter.y*gridSizeMultiplier,0),Quaternion.identity);
    }

    private void AssignRoomTypes(List<Vector2Int> roomCenters)
    {
        // Counters for special rooms
        int treasureRoomCount = 0;
        int maxTreasureRooms = Random.Range(2, 4); // Randomly 2 or 3 treasure rooms
        bool shopRoomAssigned = false;

        // Track number of Normal and Enemy rooms assigned
        int normalRoomCount = 0;
        int enemyRoomCount = 0;

        for (int i = 0; i < roomCenters.Count; i++)
        {
            RoomType roomType;

            if (i == 0)
            {
                roomType = RoomType.Start; // Starting room
            }
            else if (i == roomCenters.Count - 1)
            {
                roomType = RoomType.Boss; // Boss room at the end
            }
            else if (!shopRoomAssigned)
            {
                roomType = RoomType.Shop; // Assign one shop room
                shopRoomAssigned = true;
            }
            else if (treasureRoomCount < maxTreasureRooms)
            {
                roomType = RoomType.Treasure; // Assign up to 2-3 treasure rooms
                treasureRoomCount++;
            }
            else
            {
                // Ensure 1:2 ratio of Normal to Enemy rooms
                if (normalRoomCount < enemyRoomCount)
                {
                    roomType = RoomType.Normal;
                    normalRoomCount++;
                }
                else
                {
                    roomType = RoomType.Enemy;
                    enemyRoomCount++;
                }
            }

            // Add the room type to the dictionary
            roomTypes[roomCenters[i]] = roomType;
        }

        // Move the player to the Start Room
        MovePlayerToStartRoom();
    }



    private void MovePlayerToStartRoom()
    {
        // Find the position of the Start Room
        foreach (var roomCenter in roomTypes.Keys)
        {
            if (roomTypes[roomCenter] == RoomType.Start)
            {
                //player.position = new Vector3(roomCenter.x * .016f, roomCenter.y * .016f, player.position.z);
                break;
            }
        }
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++ )
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - 
                offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0,roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);

            // Add additional tile to the side for two-tile width
            corridor.Add(position + Vector2Int.right); // or Vector2Int.left depending on preferred offset direction
        }
        
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);

            // Add additional tile to the side for two-tile width
            corridor.Add(position + Vector2Int.up); // or Vector2Int.down depending on preferred offset direction
        }

        return corridor;
    }


    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;

    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

}
