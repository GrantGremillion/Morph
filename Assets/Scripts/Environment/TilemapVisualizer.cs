using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap, grassTilemap;
    [SerializeField]
    private TileBase floorTile0;
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
    private TileBase wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight, wallInnerCornerUpLeft, wallInnerCornerUpRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft,
        wallFullyExposedTop, wallFullyExposedBottom, wallFullyExposedRight, wallFullyExposedLeft,
        wallFullyExposedTopCorners, wallFullyExposedBottomCorners, wallFullyExposed, wallExposedSides1, wallExposedSides2,
        wallCurveTop1, wallCurveTop2, wallCurveBottom1, wallCurveBottom2, wallCurveLeft1, wallCurveLeft2, wallCurveRight1, wallCurveRight2,
        cornersBottomRight, cornersBottomLeft, cornersTopRight, cornersTopLeft;
    private List<TileBase> tiles;

    public GameObject enemyPrefab;           // Enemy prefab to spawn
    public int enemyCount = 10;              // Number of enemies to spawn
    

    public void Awake()
    {
        //Clear();
    }

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions, bool paintOnlyCorridors)
    {
        TileBase selectedTile = null;
        if (paintOnlyCorridors) selectedTile = floorTile0;
        else
        {
            tiles = new List<TileBase> { floorTile1, floorTile2, floorTile3, floorTile4, floorTile5 };
            selectedTile = tiles[Random.Range(0, tiles.Count)];
        }

        PaintTiles(floorPositions, floorTilemap, selectedTile);
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
            PaintSingleTile(floorTilemap,floorTile4,position);

        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
            //Paint floor tile underneath this specific type of grass tile
            PaintSingleTile(floorTilemap,floorTile4,position);
            
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if (tile != null)
            PaintSingleTile(wallTilemap,tile,position);

    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        grassTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType,2);
        TileBase tile = null;
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallInnerCornerUpRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerUpRight;
        }
        else if (WallTypesHelper.wallInnerCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerUpLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFullyExposedTop.Contains(typeAsInt))
        {
            tile = wallFullyExposedTop;
        }
        else if (WallTypesHelper.wallFullyExposedRight.Contains(typeAsInt))
        {
            tile = wallFullyExposedRight;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallFullyExposedLeft.Contains(typeAsInt))
        {
            tile = wallFullyExposedLeft;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallFullyExposedBottom.Contains(typeAsInt))
        {
            tile = wallFullyExposedBottom;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallFullyExposedTopCorners.Contains(typeAsInt))
        {
            tile = wallFullyExposedTopCorners;
        }
        else if (WallTypesHelper.wallFullyExposedBottomCorners.Contains(typeAsInt))
        {
            tile = wallFullyExposedBottomCorners;
        }
        else if (WallTypesHelper.wallFullyExposed.Contains(typeAsInt))
        {
            tile = wallFullyExposed;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallExposedSides1.Contains(typeAsInt))
        {
            tile = wallExposedSides1;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallExposedSides2.Contains(typeAsInt))
        {
            tile = wallExposedSides2;
            PaintSingleTile(floorTilemap,floorTile4,position);
        }
        else if (WallTypesHelper.wallCurveTop1.Contains(typeAsInt))
        {
            tile = wallCurveTop1;
        }
        else if (WallTypesHelper.wallCurveTop2.Contains(typeAsInt))
        {
            tile = wallCurveTop2;
        }
        else if (WallTypesHelper.wallCurveBottom1.Contains(typeAsInt))
        {
            tile = wallCurveBottom1;
        }
        else if (WallTypesHelper.wallCurveBottom2.Contains(typeAsInt))
        {
            tile = wallCurveBottom2;
        }
        else if (WallTypesHelper.wallCurveLeft1.Contains(typeAsInt))
        {
            tile = wallCurveLeft1;
        }
        else if (WallTypesHelper.wallCurveLeft2.Contains(typeAsInt))
        {
            tile = wallCurveLeft2;
        }
        else if (WallTypesHelper.wallCurveRight1.Contains(typeAsInt))
        {
            tile = wallCurveRight1;
        }
        else if (WallTypesHelper.wallCurveRight2.Contains(typeAsInt))
        {
            tile = wallCurveRight2;
        }
        else if (WallTypesHelper.cornerBottomRight.Contains(typeAsInt))
        {
            tile = cornersBottomRight;
        }
        else if (WallTypesHelper.cornerBottomLeft.Contains(typeAsInt))
        {
            tile = cornersBottomLeft;
        }
        else if (WallTypesHelper.cornerTopRight.Contains(typeAsInt))
        {
            tile = cornersTopRight;
        }
        else if (WallTypesHelper.cornerTopLeft.Contains(typeAsInt))
        {
            tile = cornersTopLeft;
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }

    }
}
