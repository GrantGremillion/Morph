using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;
    [SerializeField]
    private TileBase floorTile1;
    [SerializeField]
    private TileBase floorTile2;
    [SerializeField]
    private TileBase floorTile3;
    [SerializeField]
    private TileBase floorTile4;
    [SerializeField]
    private TileBase floorTile5;
    private List<TileBase> tiles;

    public GameObject enemyPrefab;           // Enemy prefab to spawn
    public int enemyCount = 10;              // Number of enemies to spawn
    

    public void Start()
    {
        
    }

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        
        tiles = new List<TileBase> { floorTile1, floorTile2, floorTile3, floorTile4, floorTile5 };
        TileBase randomTile = tiles[Random.Range(0, tiles.Count)];
        PaintTiles(floorPositions, floorTilemap, randomTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        
        tiles = new List<TileBase> { floorTile1, floorTile2, floorTile3, floorTile4, floorTile5 };
        foreach (var position in positions)
        {   
            TileBase randomTile = tiles[Random.Range(0, tiles.Count)];
            PaintSingleTile(tilemap, randomTile, position);
        }


    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {

        var tilePosition = new Vector3Int(position.x,position.y,0);
        tilemap.SetTile(tilePosition, tile);
    }

}
