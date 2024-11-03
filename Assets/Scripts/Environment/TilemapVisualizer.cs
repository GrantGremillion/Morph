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
    private List<TileBase> tiles = new List<TileBase>();

    public void Start()
    {
        tiles = new List<TileBase> { floorTile1, floorTile2, floorTile3, floorTile4, floorTile5 };
    }

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        TileBase randomTile = tiles[Random.Range(0, tiles.Count)];
        PaintTiles(floorPositions, floorTilemap, randomTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            TileBase randomTile = tiles[Random.Range(0, tiles.Count)];
            PaintSingleTile(tilemap, randomTile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        //var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        var tilePosition = new Vector3Int(position.x,position.y,0);
        tilemap.SetTile(tilePosition, tile);
    }

}
