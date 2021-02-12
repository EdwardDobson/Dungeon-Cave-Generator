using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneration
{

    public class DungeonUtility
    {
        static Vector2 m_dungeonDimensions = new Vector2();
        static Vector2Int m_wallDimensions = new Vector2Int();
        static Tilemap m_tilemap;
        static List<TileBase> m_tiles = new List<TileBase>();
        static int m_doorAmount;
        static Vector3Int m_tilePosition = new Vector3Int();
        static List<Vector2Int> m_buildPoints = new List<Vector2Int>();
        static Vector2Int m_buildPoint = new Vector2Int();
        static Vector2Int m_buildPointChoice = new Vector2Int();
        static List<Vector3Int> m_tilePositions = new List<Vector3Int>();
        static List<Vector3Int> m_doorPositions = new List<Vector3Int>();
        static List<Vector3Int> m_wallPositions = new List<Vector3Int>();
        static List<Vector3Int> m_wallPositionsForDoors = new List<Vector3Int>();
        static List<Vector3Int> m_floorPositions = new List<Vector3Int>();
        static List<Vector3Int> m_otherTilePositions = new List<Vector3Int>();
        public static void DungeonSetup(Vector2 _dungeonDimensions, Vector2Int _wallDimensions, Tilemap _map, List<TileBase> _tiles)
        {
            m_dungeonDimensions = _dungeonDimensions;
            m_wallDimensions = _wallDimensions;
            m_tilemap = _map;
            m_tiles = _tiles;
            PlaceBuildPoints();
        }
        #region TilePositionFunctions
        public static Vector3Int GetTilePosition()
        {
            return m_tilePosition;
        }
        public static Vector3Int SetTilePosition(Vector3Int _pos)
        {
            return m_tilePosition = _pos;
        }
        public static List<Vector3Int> GetTilePositions()
        {
            return m_tilePositions;
        }
        public static List<Vector3Int> GetWallPositions()
        {
            return m_wallPositions;
        }
        public static void AddWallPositions(Vector3Int _pos)
        {
            m_wallPositions.Add(_pos);
        }
        public static List<Vector3Int> GetWallForDoorsPositions()
        {
            return m_wallPositionsForDoors;
        }
        public static void AddWallForDoorsPositions(Vector3Int _pos)
        {
            m_wallPositionsForDoors.Add(_pos);
        }
        public static List<Vector3Int> GetFloorPositions()
        {
            return m_floorPositions;
        }
        public static void AddFloorPositions(Vector3Int _pos)
        {
            m_floorPositions.Add(_pos);
        }
        #endregion
        #region WallDimensionFunctions
        public static void RandomiseWallSizes(int _wallMaxX, int _wallMaxY, int _wallMinX, int _wallMinY)
        {
            m_wallDimensions = new Vector2Int(Random.Range(_wallMinX, _wallMaxX), Random.Range(_wallMinY, _wallMaxY));
        }
        public static Vector2Int GetWallDimensions()
        {
            return m_wallDimensions;
        }
        #endregion
        public static Tilemap GetTilemap()
        {
            return m_tilemap;
        }
        public static List<TileBase> GetTiles()
        {
            return m_tiles;
        }

        public static void SetDungeonDimensions(int _x, int _y)
        {
            m_dungeonDimensions = new Vector2(_x, _y);
        }
        public static Vector2 GetDungeonDimensions()
        {
            return m_dungeonDimensions;
        }
        #region BuildPointFunctions
        public static void PickBuildPoint()
        {
            m_buildPointChoice = m_buildPoints[Random.Range(0, m_buildPoints.Count)];
        }
        public static Vector2Int GetBuildPoint()
        {
            return m_buildPointChoice;
        }
        public static List<Vector2Int> GetAllBuildPoints()
        {
            return m_buildPoints;
        }
        public static void RemoveBuildPoint()
        {
            m_buildPoints.Remove((Vector2Int)m_tilePosition);
        }
        public static void PlaceBuildPoints()
        {
            for (int x = 0; x < m_dungeonDimensions.x; ++x)
            {
                for (int y = 0; y < m_dungeonDimensions.y; ++y)
                {
                    m_buildPoint = new Vector2Int(x, y);
                    m_buildPoints.Add(m_buildPoint);
                }
            }
        }
        public static void AddBuildPoint(Vector2Int _point)
        {
            m_buildPoints.Add(_point);
        }
        #endregion

        public static void CheckIfWall()
        {
            if (m_tilePosition != new Vector3Int(m_buildPointChoice.x, m_buildPointChoice.y, 0) && m_tilePosition != new Vector3Int(m_buildPointChoice.x, m_buildPointChoice.y + m_wallDimensions.y, 0) && m_tilePosition != new Vector3Int(m_buildPointChoice.x + m_wallDimensions.x, m_buildPointChoice.y, 0))
            {
                m_tilePositions.Add(m_tilePosition);
            }
        }

        #region DoorFunctions
        public static void RandomiseDoorAmount(int _maxDoorAmount)
        {
            m_doorAmount = Random.Range(1, _maxDoorAmount);
        }
        public static int GetDoorAmount()
        {
            return m_doorAmount;
        }
        public static void AddDoorPosition(Vector3Int _pos)
        {
            m_doorPositions.Add(_pos);
        }
        public static List<Vector3Int> GetDoorPositions()
        {
            return m_doorPositions;
        }
        public static void OrderDoorPositions()
        {
            m_doorPositions = m_doorPositions.OrderByDescending(v => v.x).ToList();
        }
        #endregion
        public static void GetTile()
        {
            m_tilemap.GetTile(m_tilePosition);
        }
        #region SurroundingTileFunctions
        public static void GetSurroundingPositions(int _index, List<Vector3Int> _positions)
        {
            m_otherTilePositions.Add(new Vector3Int(_positions[_index].x + 1, _positions[_index].y, 0));
            m_otherTilePositions.Add(new Vector3Int(_positions[_index].x - 1, _positions[_index].y, 0));
            m_otherTilePositions.Add(new Vector3Int(_positions[_index].x, _positions[_index].y + 1, 0));
            m_otherTilePositions.Add(new Vector3Int(_positions[_index].x, _positions[_index].y - 1, 0));
        }
        public static Vector3Int GetSurroundingPosition(int _index)
        {
            return m_otherTilePositions[_index];
        }
        public static void ClearSurroundPositions()
        {
            m_otherTilePositions.Clear();
        }
        public static TileBase GetTileSurrounding(int _index)
        {
            Vector3Int _pos = m_otherTilePositions[_index];
            return m_tilemap.GetTile(_pos);
        }
        #endregion
    }
}

