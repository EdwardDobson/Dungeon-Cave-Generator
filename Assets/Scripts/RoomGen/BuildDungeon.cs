using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildDungeon : MonoBehaviour
{
    [SerializeField]
    Vector2 m_dungeonDimensions = new Vector2();
    [SerializeField]
    Vector2Int m_wallDimensions = new Vector2Int();
    [SerializeField]
    Tilemap m_tilemap;
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
        DungeonUtility.DungeonSetup(m_dungeonDimensions, m_wallDimensions, m_tilemap, m_tiles);
        for (int i = 0; i < m_roomAmount; ++i)
        {
            DungeonUtility.PickBuildPoint();
            DungeonUtility.RandomiseDoorAmount(m_maxDoorAmount);
            DungeonUtility.RandomiseWallSizes(m_wallMaxX, m_wallMaxY, m_wallMinX, m_wallMaxY);
            WallGen.BuildWall();
            WallGen.RemoveWalls();

            BuildFloor.FillFloor();
            ConnectRoom.FindClosestDoor();
    
        }
 
        
        for (int a = 0; a < m_maxDoorAmount; ++a)
        {
            BuildDoor.PlaceDoor();
        }
        BuildDoor.RemoveDoors();
    }
}
