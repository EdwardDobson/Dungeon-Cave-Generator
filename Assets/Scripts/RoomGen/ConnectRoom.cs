using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DungeonGeneration
{
    public class ConnectRoom
    {
        static Vector2Int m_startPos;
        static Vector2Int m_endPos;
        static List<Vector2Int> m_roomsToConnect = new List<Vector2Int>();
 
        public  static void PlacePositions(int _index)
        {
              m_roomsToConnect.Add((Vector2Int)RoomManager.GetAllRooms()[_index].WallPositions[Random.Range(0, RoomManager.GetAllRooms()[_index].WallPositions.Count)]);
        }
        public static List<Vector2Int> GetPositions()
        {
            return m_roomsToConnect;
        }
        public static void FindOtherRoom()
        {
            m_startPos = m_roomsToConnect[Random.Range(0, m_roomsToConnect.Count)];
            m_endPos = m_roomsToConnect[Random.Range(0, m_roomsToConnect.Count)];
            DungeonUtility.GetTilemap().SetTile(new Vector3Int(m_startPos.x, m_startPos.y, 0), DungeonUtility.GetTiles()[1]);
            DungeonUtility.GetTilemap().SetTile(new Vector3Int(m_endPos.x, m_endPos.y, 0), DungeonUtility.GetTiles()[1]);
      //      Debug.Log("Rooms to connect: " + m_roomsToConnect.Count);
            BuildToRoom();
        }
        static void BuildToRoom()
        {
            int xAmount = m_startPos.x - m_endPos.x;
            int yAmount = m_startPos.y - m_endPos.y;
        
           //    Debug.Log("StartPos: " + m_startPos);
               //    Debug.Log("EndPos: " + m_endPos);
                if (xAmount < 0)
                {
                    for (int x = 0; x > xAmount; --x)
                    {
                        BuildTilePiece.BuildPiece(m_startPos.x - x, m_startPos.y, 4, false);
                    }
                }
                else if (xAmount > 0)
                {
                    for (int x = 0; x < xAmount; ++x)
                    {
                        BuildTilePiece.BuildPiece(m_startPos.x - x, m_startPos.y, 4, false);
                    }
                }
                if (yAmount < 0)
                {
                    for (int y = 0; y > yAmount; --y)
                    {
                        BuildTilePiece.BuildPiece(m_startPos.x - xAmount, m_startPos.y - y, 4, false);
                    }
                }
                else
                {
                    if (yAmount > 0)
                    {
                        for (int y = 0; y < yAmount; ++y)
                        {
                            BuildTilePiece.BuildPiece(m_startPos.x + -xAmount, m_startPos.y - y, 4, false);
                        }
                    }
                }
                //     Debug.Log("StartPos: " + m_startPos);
                //   Debug.Log("EndPos: " + m_endPos);
        }   
    }
}

