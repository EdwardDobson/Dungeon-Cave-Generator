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
    int[] m_wallSizesSquare;
    [SerializeField]
    int[] m_wallSizesTShape;
    [SerializeField]
    int m_wallLengthLShape;
    [SerializeField]
    int m_wallHeightLShape;
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
    int m_lShapeRoomAmount;
    [SerializeField]
    int m_TShapeRoomAmount;
    [SerializeField]
    int m_circleRoomAmount;
    [SerializeField]
    int m_diamondRoomAmount;

    [SerializeField]
    int m_pathAmount;
    [SerializeField]
    int m_scoreAmount;
    public GameObject ScorePrefab;
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
            WallGen.RandomiseWallSizes(m_wallSizesSquare[0], m_wallSizesSquare[1], m_wallSizesSquare[2], m_wallSizesSquare[3]);
            FloorGen.Square();
        }
        for (int i = 0; i < m_lShapeRoomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            WallGen.RandomiseWallSizes(m_wallSizesSquare[0], m_wallSizesSquare[1], m_wallSizesSquare[2], m_wallSizesSquare[3]);
            int directionRandom = UnityEngine.Random.Range(0, 4);
            FloorGen.LShape(m_wallLengthLShape, m_wallHeightLShape, directionRandom);
        }
        for (int i = 0; i < m_TShapeRoomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            int directionRandom = UnityEngine.Random.Range(0, 4);
            int stemWidth = UnityEngine.Random.Range(2, m_wallSizesTShape[0]);
            int stemHeight = UnityEngine.Random.Range(2, m_wallSizesTShape[1]);
            int roomLength = UnityEngine.Random.Range(2, m_wallSizesTShape[2]);
            int roomHeight = UnityEngine.Random.Range(2, m_wallSizesTShape[3]);
            FloorGen.TShape(stemWidth, stemHeight, roomLength, roomHeight, directionRandom);
        }
        for (int i = 0; i < m_circleRoomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            FloorGen.Circle(m_circleStartSize, m_circleMiddleAmount);
        }
        for (int i = 0; i < m_diamondRoomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            FloorGen.Diamond(m_diamondMinRowAmount, m_diamondMaxRowAmount);
        }
        for (int i =0; i< FloorGen.GetFloorTilePositions().Count; ++i)
        {
            FloorGen.PlaceFloorTile(FloorGen.GetFloorTilePositions()[i]);
        }
        for (int i = 0; i < m_pathAmount; ++i)
        {
            ConnectRoom.PlacePositions(i);
            ConnectRoom.FindOtherRoom();
        }
        WallGen.PlaceWalls();
        for(int i = 0; i < m_scoreAmount; ++i)
        {
            if(FloorGen.GetFloorPositions().Count > 1)
            {
                Vector3Int position = FloorGen.GetFloorPositions()[UnityEngine.Random.Range(0, FloorGen.GetFloorPositions().Count)];
                Vector3 positionReadjusted = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
                FloorGen.GetFloorPositions().Remove(position);
                GameObject scoreClone = Instantiate(ScorePrefab, positionReadjusted, Quaternion.identity, transform);
                scoreClone.GetComponent<ScorePoint>().ScoreWorth = UnityEngine.Random.Range(1, 25);
            }
        
        }
        SW.Stop();
        TimeSpan ts = SW.Elapsed;
        UnityEngine.Debug.Log("Building Dungeon Took: " + ts.Milliseconds + " ms");
    }
    public void PlaceWalls()
    {
        WallGen.PlaceWalls();
    }
}
