using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomGen : MonoBehaviour
{
    [SerializeField]
    Tilemap m_tilemap;
    [SerializeField]
    Vector2Int m_wallDimensions;
    [SerializeField]
    int m_wallMaxX;
    [SerializeField]
    int m_wallMinX;
    [SerializeField]
    int m_wallMaxY;
    [SerializeField]
    int m_wallMinY;
    [SerializeField]
    List<TileBase> m_tiles = new List<TileBase>();
    [SerializeField]
    List<Vector3Int> m_tilePostions = new List<Vector3Int>();
    Vector3Int m_tempPosition;
    [SerializeField]
    Vector2Int m_dungeonDimensions;
    int m_doorAmount;
    [SerializeField]
    List<Vector2Int> m_buildPoints = new List<Vector2Int>();
    [SerializeField]
    Vector2Int m_buildPointChoice;

    void Start()
    {
        for (int x = 0; x < m_dungeonDimensions.x; ++x)
        {
            for (int y = 0; y < m_dungeonDimensions.y; ++y)
            {
                Vector2Int temp = new Vector2Int(x, y);
                m_buildPoints.Add(temp);
            }
        }
        m_buildPointChoice = m_buildPoints[Random.Range(0, m_buildPoints.Count)];

        RebuildRoom(1);

    }
    void BuildWalls()
    {
        m_tilePostions.Clear();
        for (int i = 0; i < m_wallDimensions.x + 1; ++i)
        {
            BuildPiece(m_buildPointChoice.x + i, m_buildPointChoice.y, 0);
            if (m_tempPosition != new Vector3Int(m_buildPointChoice.x, m_buildPointChoice.y, 0) && m_tempPosition != new Vector3Int(m_buildPointChoice.x, m_buildPointChoice.y + m_wallDimensions.y, 0) && m_tempPosition != new Vector3Int(m_buildPointChoice.x +m_wallDimensions.x, m_buildPointChoice.y, 0))
                m_tilePostions.Add(m_tempPosition);

            BuildPiece(m_buildPointChoice.x + i, m_buildPointChoice.y + m_wallDimensions.y, 0);
            m_buildPoints.Remove((Vector2Int)m_tempPosition);
        }
        for (int a = 0; a < m_wallDimensions.y + 1; ++a)
        {
            BuildPiece(m_buildPointChoice.x, m_buildPointChoice.y + a, 0);
            if (m_tempPosition != new Vector3Int(m_buildPointChoice.x, m_buildPointChoice.y, 0) && m_tempPosition != new Vector3Int(m_buildPointChoice.x, m_buildPointChoice.y + m_wallDimensions.y, 0) && m_tempPosition != new Vector3Int(m_buildPointChoice.x + m_wallDimensions.x, m_buildPointChoice.y, 0))
                m_tilePostions.Add(m_tempPosition);

            BuildPiece(m_buildPointChoice.x + m_wallDimensions.x, m_buildPointChoice.y + a, 0);
            m_buildPoints.Remove((Vector2Int)m_tempPosition);
        }
        for (int i = 0; i < m_doorAmount; ++i)
        {
            PlaceDoor();
        }
        FillFloor();
    }
    void BuildPiece(int _value1, int _value2, int _tileIndex)
    {
        Vector3Int posY = new Vector3Int(_value1, _value2, 0);
        if (m_tilemap.GetTile(posY) == null)
            m_tilemap.SetTile(posY, m_tiles[_tileIndex]);
        m_tempPosition = posY;

    }
    void FillFloor()
    {
        for (int i = 0; i < m_wallDimensions.x; ++i)
        {
            for (int a = 0; a < m_wallDimensions.y; ++a)
            {
                BuildPiece(m_buildPointChoice.x + i, m_buildPointChoice.y + a, 1);
            }
        }
    }
    public void RebuildRoom(int _times)
    {
        for(int i = 0; i < _times; ++i)
        {
            m_doorAmount = Random.Range(1, 4);
            m_buildPointChoice = m_buildPoints[Random.Range(0, m_buildPoints.Count)];
            RandomiseWallDimensions();
            BuildWalls();
        }
    }
    public void ClearTileMap()
    {
        m_tilemap.ClearAllTiles();
    }
    void PlaceDoor()
    {
        int randomPosChoice = Random.Range(0, m_tilePostions.Count);
        Vector3Int randomWallPos = m_tilePostions[randomPosChoice];
        m_tilemap.SetTile(randomWallPos, null);
    }
    void RandomiseWallDimensions()
    {
        m_wallDimensions = new Vector2Int(Random.Range(m_wallMinX, m_wallMaxX), Random.Range(m_wallMinY, m_wallMaxY));
    }
}
