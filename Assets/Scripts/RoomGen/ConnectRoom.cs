using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
namespace DungeonGeneration
{
    public class ConnectRoom
    {
        static Vector2Int m_startPos;
        static Vector2Int m_endPos;
        static List<Vector2Int> m_roomsToConnect = new List<Vector2Int>();
        static List<Vector3Int> m_pathPositions = new List<Vector3Int>();
        public static void PlacePositions(int _index)
        {
            m_roomsToConnect.Add(DungeonUtility.GetAllPathPoints()[Random.Range(0, DungeonUtility.GetAllPathPoints().Count)]);
        }
        public static List<Vector2Int> GetPositions()
        {
            return m_roomsToConnect;
        }
        public static void FindOtherRoom()
        {
            m_startPos = m_roomsToConnect[Random.Range(0, m_roomsToConnect.Count)];
            m_endPos = m_roomsToConnect[Random.Range(0, m_roomsToConnect.Count)];
          
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
                for (int x = 0; x > xAmount; --x)//Left
                {
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0)) == null)
                    {
                        BuildTilePiece.BuildPiece(m_startPos.x - x, m_startPos.y, 0, false, TileType.Path);
                        m_pathPositions.Add(new Vector3Int(m_startPos.x - x, m_startPos.y, 0));
                    }
            
                }
            }
            else if (xAmount > 0)
            {
                for (int x = 0; x < xAmount; ++x)//Right
                {
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0)) == null)
                    {
                        BuildTilePiece.BuildPiece(m_startPos.x - x, m_startPos.y, 0, false, TileType.Path);
                        m_pathPositions.Add(new Vector3Int(m_startPos.x - x, m_startPos.y, 0));
                    }
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0)) != null)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0)).name.Contains("Wall"))
                        {
                            BuildTilePiece.BuildPiece(m_startPos.x - x, m_startPos.y, 0, false, TileType.Path);
                        }
                    }
                }
            }
            if (yAmount < 0)
            {
                for (int y = 0; y > yAmount; --y)
                {
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y, 0)) == null)
                    {
                        BuildTilePiece.BuildPiece(m_startPos.x - xAmount, m_startPos.y - y, 0, false, TileType.Path);
                        m_pathPositions.Add(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y, 0));
                    }
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 1, 0)) != null)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 1, 0)).name.Contains("Wall"))
                        {

                            BuildTilePiece.RemoveTilePiece(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 1, 0));
   
                            if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 2, 0)) == null)
                                BuildTilePiece.BuildPiece(m_startPos.x - xAmount, m_startPos.y - y - 1, 0, false, TileType.Wall);
                                else
                                BuildTilePiece.BuildPiece(m_startPos.x - xAmount, m_startPos.y - y - 1, 0, false, TileType.Door);
                        }
                    }
                }
            }
            else
            {
                if (yAmount > 0)
                {
                    for (int y = 0; y < yAmount; ++y)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y, 0)) == null)
                        {
                            BuildTilePiece.BuildPiece(m_startPos.x + -xAmount, m_startPos.y - y, 0, false, TileType.Path);
                            m_pathPositions.Add(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y, 0));
                        }
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0)) != null)
                        {
                            if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0)).name.Contains("Wall"))
                            {
                                BuildTilePiece.RemoveTilePiece(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0));
                                if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y + 2, 0)) == null)
                                    BuildTilePiece.BuildPiece(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0, false, TileType.Wall);
                                else
                                    BuildTilePiece.BuildPiece(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0, false, TileType.Door);
                            }
                        }
                    }
                }
            }

        }
        public static void ReducePaths()
        {
            for (int i = 0; i < m_pathPositions.Count; ++i)
            {

                DungeonUtility.GetSurroundingPositions(i, m_pathPositions);


                DungeonUtility.ClearSurroundPositions();
                /*
                 *        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y + 1, 0)) != null && DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y -1, 0)) != null)
                {
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y - 1, 0)).name.Contains("Path") && DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y +1, 0)).name.Contains("Path"))
                        BuildTilePiece.RemoveTilePiece((Vector3Int)m_pathPositions[i]);
                }
                 *   if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x + 1, m_pathPositions[i].y, 0)) == null && DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x - 1, m_pathPositions[i].y, 0)) != null)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x - 1, m_pathPositions[i].y, 0)).name.Contains("Path"))
                            BuildTilePiece.RemoveTilePiece((Vector3Int)m_pathPositions[i]);
                    }
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x + 1, m_pathPositions[i].y, 0)) != null && DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x - 1, m_pathPositions[i].y, 0)) == null)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x + 1, m_pathPositions[i].y, 0)).name.Contains("Path"))
                            BuildTilePiece.RemoveTilePiece((Vector3Int)m_pathPositions[i]);
                    }
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y + 1, 0)) == null && DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y -1, 0)) != null)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y - 1, 0)).name.Contains("Path"))
                            BuildTilePiece.RemoveTilePiece((Vector3Int)m_pathPositions[i]);
                    }
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y + 1, 0)) != null && DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x , m_pathPositions[i].y -1, 0)) == null)
                    {
                        if(DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_pathPositions[i].x, m_pathPositions[i].y + 1, 0)).name.Contains("Path"))
                        BuildTilePiece.RemoveTilePiece((Vector3Int)m_pathPositions[i]);
                    }
                 */

            }
        }
    }
}

