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
    int m_circleStartSizeMax;
    int m_circleMiddleAmount;
    [Range(10,1000)]
    [SerializeField]
    int m_diamondMaxRowAmount;
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
    public List<GameObject> Scores;
    public bool ScoresPlaced;
    public GameObject SquareRoom;
    public GameObject CircleRoom;
    public GameObject DiamondRoom;
    public bool UseMiniMapIcons;
    public int Seed;
    FileManager m_fileManager;
    LevelLoad m_levelLoader;
    public void Build()
    {
        m_fileManager = transform.parent.parent.GetComponent<FileManager>();
        if (!m_fileManager.Save.SeedSet)
        {
            m_fileManager.Save.Seed = Seed;
            m_fileManager.Save.SeedSet = true;
        }
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
            if (UseMiniMapIcons)
            {
                Vector2 pos = new Vector2(DungeonUtility.GetBuildPoint().x + WallGen.GetWallDimensions().x / 2, DungeonUtility.GetBuildPoint().y + WallGen.GetWallDimensions().y / 2);
                Instantiate(SquareRoom, pos, Quaternion.identity);
            }

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
            int randomCircleSize = UnityEngine.Random.Range(1, m_circleStartSizeMax);
            DungeonUtility.PickBuildPoint();
            m_circleMiddleAmount = randomCircleSize;
            FloorGen.Circle(randomCircleSize, m_circleMiddleAmount);
            if (UseMiniMapIcons)
            {
                Vector2 pos = new Vector2(DungeonUtility.GetBuildPoint().x + m_circleMiddleAmount / 2, DungeonUtility.GetBuildPoint().y + m_circleMiddleAmount * 2);
                Instantiate(CircleRoom, pos, Quaternion.identity);
            }

        }
        for (int i = 0; i < m_diamondRoomAmount; ++i)
        {
            int RandomMaxRowAmount = UnityEngine.Random.Range(10, m_diamondMaxRowAmount);
            DungeonUtility.PickBuildPoint();
            FloorGen.Diamond(1, RandomMaxRowAmount);
            if (UseMiniMapIcons)
            {
                Vector2 pos = new Vector2(DungeonUtility.GetBuildPoint().x, DungeonUtility.GetBuildPoint().y + RandomMaxRowAmount / 2);
                Instantiate(DiamondRoom, pos, DiamondRoom.transform.rotation);
            }
        }
        for (int i = 0; i < FloorGen.GetFloorTilePositions().Count; ++i)
        {
            FloorGen.PlaceFloorTile(FloorGen.GetFloorTilePositions()[i]);
        }
        for (int i = 0; i < m_pathAmount; ++i)
        {
            ConnectRoom.PlacePositions(i);
            ConnectRoom.FindOtherRoom();
        }
        WallGen.PlaceWalls();
        if (m_levelLoader == null)
            m_levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoad>();
            //  m_fileManager.TileSetter();
        if (m_levelLoader != null)
        {
            if (m_levelLoader.GetComponent<LevelLoad>().ScoreMode)
            {
                for (int i = 0; i < m_scoreAmount; ++i)
                {
                    if (FloorGen.GetFloorPositions().Count > 1)
                    {
                        Vector3Int position = FloorGen.GetFloorPositions()[UnityEngine.Random.Range(0, FloorGen.GetFloorPositions().Count)];
                        Vector3 positionReadjusted = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
                        FloorGen.GetFloorPositions().Remove(position);
                        GameObject scoreClone = Instantiate(ScorePrefab, positionReadjusted, Quaternion.identity, transform);
                        scoreClone.GetComponent<ScorePoint>().ScoreWorth = UnityEngine.Random.Range(1, 25);
                        Scores.Add(scoreClone);
                    }
                }
                ScoresPlaced = true;
            }
        }
        GameObject.Find("Player").GetComponent<PlaceTile>().FillTilesList();
        SW.Stop();
        TimeSpan ts = SW.Elapsed;
        UnityEngine.Debug.Log("Building Dungeon Took: " + ts.Milliseconds + " ms");
    }
    //Used for buttons
    public void PlaceWalls()
    {
        WallGen.PlaceWalls();
    }
    public int GetScoreAmount()
    {
        return m_scoreAmount;
    }
}
