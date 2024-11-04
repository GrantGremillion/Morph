using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SimpleRandomWalkGenerator : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;
    
    [SerializeField]
    private Tilemap floorTilemap;
    
    [SerializeField]
    private int iterations = 10;
    [SerializeField]
    public int walkLength = 10;
    [SerializeField]
    public bool startRandomlyEachIteration = true;

    [SerializeField]
    private TilemapVisualizer tilemapVisualizer;
    public GameObject enemyPrefab;           
    public int maxEnemyCount = 10;     
    private int currentEnemyCount = 0;   
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    private List<Vector3> walkablePositions = new List<Vector3>();      

    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        GenerateEnemies();
    }

    private void GenerateEnemies()
    {
        CollectWalkableTilePositions();

        StartCoroutine(SpawnEnemiesAtRandomIntervals());
    }

    void CollectWalkableTilePositions()
    {
        foreach (Vector3Int position in floorTilemap.cellBounds.allPositionsWithin)
        {
            // Check if the tile at this position is a walkable tile
            TileBase tile = floorTilemap.GetTile(position);
            if (tile != null) // Replace with specific tile checks if needed
            {
                // Convert tilemap cell position to world position and add to list
                Vector3 worldPosition = floorTilemap.CellToWorld(position) + floorTilemap.tileAnchor;
                walkablePositions.Add(worldPosition);
            }
        }
    }

    IEnumerator SpawnEnemiesAtRandomIntervals()
    {
        while (currentEnemyCount < maxEnemyCount)
        {
            // Wait for a random time between minSpawnDelay and maxSpawnDelay
            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);

            // Spawn an enemy at a random walkable position
            Vector3 spawnPosition = walkablePositions[Random.Range(0, walkablePositions.Count)];
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            currentEnemyCount++;
        }
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path);
            if(startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0,floorPositions.Count));
            }
        }    
        return floorPositions;
    }

}
