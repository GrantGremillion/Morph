using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
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
    [SerializeField]
    private TileBase wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull;
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

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType,2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if (tile != null)
            PaintSingleTile(wallTilemap,tile, position);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string neighboursBinaryType)
    {
        
    }
}
