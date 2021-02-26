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
    int m_wallMaxX;
    [SerializeField]
    int m_wallMinX;
    [SerializeField]
    int m_wallMaxY;
    [SerializeField]
    int m_wallMinY;
    [SerializeField]
    int m_circleStartSize;
    [SerializeField]
    int m_circleMiddleAmount;
    [SerializeField]
    int m_diamondMinRowAmount;
    [SerializeField]
    int m_diamondMaxRowAmount;
    [SerializeField]
    int m_maxDoorAmount;
    [SerializeField]
    int m_squareRoomAmount;
    [SerializeField]
    int m_circleRoomAmount;
    [SerializeField]
    int m_diamondRoomAmount;
    [SerializeField]
    int m_pathAmount;
    void Start()
    {
        Stopwatch SW = new Stopwatch();
        SW.Start();
        TileManager.LoadTileManager();
        DungeonUtility.DungeonSetup(m_dungeonDimensions, m_tilemap);

        WallGen.SetWallSizes(m_wallDimensions);
        WallGen.SetWallsTileMap(m_walls);
        for (int i = 0; i < m_squareRoomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            WallGen.RandomiseWallSizes(m_wallMaxX, m_wallMaxY, m_wallMinX, m_wallMaxY);
            FloorGen.FillFloor();
        }
        for (int i = 0; i < m_circleRoomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            FloorGen.FillFloorCircle(m_circleStartSize, m_circleMiddleAmount);
        }
        for (int i = 0; i < m_diamondRoomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            FloorGen.FillFloorDiamond(m_diamondMinRowAmount, m_diamondMaxRowAmount);
        }

        for (int i = 0; i < m_pathAmount; ++i)
        {
            ConnectRoom.PlacePositions(i);
            ConnectRoom.FindOtherRoom();
        }
        WallGen.PlaceWalls();
        SW.Stop();
        TimeSpan ts = SW.Elapsed;
        UnityEngine.Debug.Log("Building Dungeon Took: " + ts.Milliseconds + " ms");
      //  GameObject.Find("Player").GetComponent<PlaceTile>().GetComponent<PlaceTile>().FillTilesList();
    }
    public void PlaceWalls()
    {
        WallGen.PlaceWalls();
    }
}
