using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    TileBase m_wall;
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
            for (int x = 0; x < Bounds.size.x; x++)
            {
                for (int y = 0; y < Bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * Bounds.size.x];
                    if (tile == null)
                    {
                        BuildTilePiece.BuildPiece(x, y, 0, false, TileType.Wall, m_walls);
                    }
                }
            }
        }
    }
