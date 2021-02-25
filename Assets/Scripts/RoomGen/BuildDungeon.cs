using DungeonGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        Stopwatch SW = new Stopwatch();
        SW.Start();
        TileManager.LoadTileManager();
        DungeonUtility.DungeonSetup(m_dungeonDimensions, m_tilemap);
        WallGen.SetWallSizes(m_wallDimensions);
        for (int i = 0; i < m_roomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            WallGen.RandomiseWallSizes(m_wallMaxX, m_wallMaxY, m_wallMinX, m_wallMaxY);
            FloorGen.FillFloor();
        }
        for (int i = 0; i < m_roomAmount; ++i)
        {
            ConnectRoom.PlacePositions(i);
            ConnectRoom.FindOtherRoom();
        }
        WallGen.SetWallsTileMap(m_walls);
        WallGen.PlaceWalls();
        SW.Stop();
        TimeSpan ts = SW.Elapsed;
        UnityEngine.Debug.Log("Building Dungeon Took: " + ts.Milliseconds + " ms");
      //  GameObject.Find("Player").GetComponent<PlaceTile>().GetComponent<PlaceTile>().FillTilesList();
    }
}
