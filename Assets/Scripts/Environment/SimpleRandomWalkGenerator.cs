using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SimpleRandomWalkGenerator : AbstractLevelGenerator
{
   
    [SerializeField]
    private Tilemap floorTilemap;
    
    [SerializeField]
    private SimpleRandomWalkData randomWalkParameters;

    public GameObject enemyPrefab;           
    public int maxEnemyCount = 10;     
    private int currentEnemyCount = 0;   
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    private List<Vector3> walkablePositions = new List<Vector3>();      

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        //GenerateEnemies();
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

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkData parameters)
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < randomWalkParameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);
            floorPositions.UnionWith(path);
            if(randomWalkParameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0,floorPositions.Count));
            }
        }    
        return floorPositions;
    }

}
