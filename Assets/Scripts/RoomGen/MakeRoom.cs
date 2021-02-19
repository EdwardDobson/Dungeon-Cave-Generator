using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DungeonGeneration
{
    /// <summary>
    /// Holds all of the variables a room needs
    /// </summary>
    public class Room
    {
        public List<Vector3Int> WallPositions = new List<Vector3Int>();
        public List<Vector3Int> TilePositions = new List<Vector3Int>();
        public List<Vector3Int> DoorPositions = new List<Vector3Int>();
        public Vector2Int BuildPoint;
        public bool HasDoor;
    }
    public class RoomManager
    {
        static List<Room> m_rooms = new List<Room>();

        public static List<Room> GetAllRooms()
        {
            return m_rooms;
        }
        public static void RemoveRoom()
        {
            for (int i = 0; i < m_rooms.Count; ++i)
            {
                for (int w = 0; w < m_rooms[i].WallPositions.Count; ++w)
                {
                    DungeonUtility.GetTilemap().SetTile(m_rooms[i].WallPositions[w], null);
                    m_rooms[i].WallPositions.RemoveAt(w);
                }
            }
        }
        /// <summary>
        /// Used to check tiles around a given wall
        /// </summary>
        public static void TileCheckEmpty(Vector3Int _pos1, Vector3Int _pos2, int _indexRoom, int _indexWall)
        {
            if (DungeonUtility.GetTilemap().GetTile(_pos1) == null && DungeonUtility.GetTilemap().GetTile(_pos2) == null)
            {
                TileManager.ChangeTilePiece(m_rooms[_indexRoom].WallPositions[_indexWall], 0, TileType.Wall, DungeonUtility.GetTilemap());
            }
        }
        public static void TileCheckNotEmpty(Vector3Int _pos1, Vector3Int _pos2, Vector3Int _pos3, Vector3Int _pos4, int _indexRoom, int _indexWall, string _tileName)
        {
            if (DungeonUtility.GetTilemap().GetTile(_pos1) != null && DungeonUtility.GetTilemap().GetTile(_pos2) != null && DungeonUtility.GetTilemap().GetTile(_pos3) != null && DungeonUtility.GetTilemap().GetTile(_pos4) != null)
            {
                if (DungeonUtility.GetTilemap().GetTile(_pos1).name == _tileName && DungeonUtility.GetTilemap().GetTile(_pos2).name == _tileName)
                {
                    TileManager.ChangeTilePiece(m_rooms[_indexRoom].WallPositions[_indexWall], 0,  TileType.Floor, DungeonUtility.GetTilemap());
               
                }
                if (DungeonUtility.GetTilemap().GetTile(_pos3).name == _tileName && DungeonUtility.GetTilemap().GetTile(_pos4).name == _tileName)
                {
                    TileManager.ChangeTilePiece(m_rooms[_indexRoom].WallPositions[_indexWall], 0, TileType.Floor, DungeonUtility.GetTilemap());
                }
            }
        }
        public static void InitialiseRoomSingle(List<Vector3Int> _wallPositions, List<Vector3Int> _tilePositions, Vector2Int _buildPoint)
        {
            Room m_single = new Room();
            m_single.WallPositions = _wallPositions;
            m_single.TilePositions = _tilePositions;
            m_single.BuildPoint = _buildPoint;
            m_rooms.Add(m_single);

        }
    }
}

