using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneration
{

    public class DungeonUtility
    {
        static Vector2Int m_dungeonDimensions = new Vector2Int();

        static Tilemap m_tilemap;
        static int m_doorAmount;
        static List<Vector2Int> m_buildPoints = new List<Vector2Int>();
        static Vector2Int m_buildPoint = new Vector2Int();
        static Vector2Int m_buildPointChoice = new Vector2Int();
        static List<Vector3Int> m_doorPositions = new List<Vector3Int>();

        static List<Vector3Int> m_otherTilePositions = new List<Vector3Int>();
        static List<Vector2Int> m_pathPoints = new List<Vector2Int>();
        public static void DungeonSetup(Vector2Int _dungeonDimensions, Tilemap _map)
        {
            m_dungeonDimensions = _dungeonDimensions;
            m_tilemap = _map;
            PlaceBuildPoints();
        }
        public static Tilemap GetTilemap()
        {
            return m_tilemap;
        }

        public static List<Vector2Int> GetAllPathPoints()
        {
            return m_pathPoints;
        }
        #region BuildPointFunctions
        public static void PickBuildPoint()
        {
            m_buildPointChoice = m_buildPoints[Random.Range(0, m_buildPoints.Count)];
            Vector3Int pos = new Vector3Int(m_buildPointChoice.x, m_buildPointChoice.y, 0);
            if(m_tilemap.GetTile(pos) == null)
            m_pathPoints.Add(m_buildPointChoice);
        }
        public static Vector2Int GetBuildPoint()
        {
            return m_buildPointChoice;
        }
        public static List<Vector2Int> GetAllBuildPoints()
        {
            return m_buildPoints;
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
        #endregion

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

