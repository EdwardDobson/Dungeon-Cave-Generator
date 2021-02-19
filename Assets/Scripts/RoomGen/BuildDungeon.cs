using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildDungeon : MonoBehaviour
{
    [SerializeField]
    Vector2Int m_dungeonDimensions = new Vector2Int();
    [SerializeField]
    Vector2Int m_wallDimensions = new Vector2Int();
    [SerializeField]
    Tilemap m_tilemap;
    [SerializeField]
    Tilemap m_walls;
    [SerializeField]
    List<TileBase> m_tiles = new List<TileBase>();
    [SerializeField]
    int m_wallMaxX;
    [SerializeField]
    int m_wallMinX;
    [SerializeField]
    int m_wallMaxY;
    [SerializeField]
    int m_wallMinY;
    [SerializeField]
    int m_maxDoorAmount;
    [SerializeField]
    int m_roomAmount;
    void Start()
    {
        TileManager.FillTilesList();
        DungeonUtility.DungeonSetup(m_dungeonDimensions, m_wallDimensions, m_tilemap, m_tiles);

        for (int i = 0; i < m_roomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            DungeonUtility.RandomiseWallSizes(m_wallMaxX, m_wallMaxY, m_wallMinX, m_wallMaxY);
            BuildFloor.FillFloor();
            RoomManager.InitialiseRoomSingle(DungeonUtility.GetWallForDoorsPositions(), DungeonUtility.GetTilePositions(), DungeonUtility.GetBuildPoint());
        }

        for (int r = 0; r < RoomManager.GetAllRooms().Count; ++r)
        {
            ConnectRoom.PlacePositions(r);
            ConnectRoom.FindOtherRoom();
        }
        PlaceWalls();

    }
    void PlaceWalls()
    {
        BoundsInt Bounds = DungeonUtility.GetTilemap().cellBounds;
        TileBase[] allTiles = DungeonUtility.GetTilemap().GetTilesBlock(Bounds);
        List<CustomTile> tilesWithinRange = new List<CustomTile>();
        for (int x = 0; x < Bounds.size.x; x++)
        {
            for (int y = 0; y < Bounds.size.y; y++)
            {
                float randomFreq = Random.Range(1, TileManager.GetTileHolder(TileType.Wall).Tiles.OrderByDescending(t => t.PickChance).First().PickChance);
                tilesWithinRange = TileManager.GetTileHolder(TileType.Wall).Tiles.Where(t => t.PickChance >= randomFreq).ToList();
                TileBase tile = allTiles[x + y * Bounds.size.x];
                int tempTileIndex;
                tempTileIndex = Random.Range(0, tilesWithinRange.Count);
                if (tile == null)
                {
                    TileManager.BuildPiece(x, y, Random.Range(0, tilesWithinRange.Count), false, TileType.Wall, m_walls);
                    TileManager.ChangeTileColour(m_walls, new Vector3Int(x, y, 0), tilesWithinRange[tempTileIndex]);
                    DungeonUtility.AddWallPositions(new Vector3Int(x, y, 0));
                    Vector3Int pos = new Vector3Int(x, y, 0);
                   DungeonGeneration.TileData td = new DungeonGeneration.TileData();
                    td.CustomTile = tilesWithinRange[tempTileIndex];
                    td.TileBase = DungeonUtility.GetTilemap().GetTile(pos);
                    if (!TileManager.GetTileDictionary().ContainsKey(pos))
                        TileManager.FillDictionary(pos, td);
                }
            }
        }
    }
}
