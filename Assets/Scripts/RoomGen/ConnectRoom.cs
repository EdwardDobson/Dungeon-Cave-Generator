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
        static void BuildPathTile(Vector3Int _pos)
        {
                TileHolder tileHolder = TileManager.GetTileHolder(TileType.Path);
                float randomFreq = Random.Range(1, tileHolder.Tiles.OrderByDescending(t => t.PickChance).First().PickChance);
                List<CustomTile> tilesWithinRange = new List<CustomTile>();
                tilesWithinRange = tileHolder.Tiles.Where(t => t.PickChance >= randomFreq).ToList();
                int tempTileIndex;
                tempTileIndex = Random.Range(0, tilesWithinRange.Count);
                TileManager.BuildPiece(_pos, tilesWithinRange[tempTileIndex].Tile[0], DungeonUtility.GetTilemap());
                TileManager.FillDictionary(_pos, tilesWithinRange[tempTileIndex], DungeonUtility.GetTilemap(),DictionaryType.Floor);
                TileManager.ChangeTileColour(DungeonUtility.GetTilemap(), _pos, tilesWithinRange[tempTileIndex]);
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
                        BuildPathTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0));
                    }
                }
            }
            else if (xAmount > 0)
            {
                for (int x = 0; x < xAmount; ++x)//Right
                {
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0)) == null)
                    {
                        BuildPathTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0));
                    }
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0)) != null)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0)).name.Contains("Wall"))
                        {
                            BuildPathTile(new Vector3Int(m_startPos.x - x, m_startPos.y, 0));
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
                        BuildPathTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y, 0));
                    }
                    if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 1, 0)) != null)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 1, 0)).name.Contains("Wall"))
                        {

                            TileManager.RemoveTilePiece(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 1, 0), DungeonUtility.GetTilemap());

                            if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 2, 0)) == null)
                                BuildPathTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 1, 0));
                            else
                                BuildPathTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y - 1, 0));
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
                            BuildPathTile(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y, 0));
                        }
                        if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0)) != null)
                        {
                            if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0)).name.Contains("Wall"))
                            {
                                TileManager.RemoveTilePiece(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0), DungeonUtility.GetTilemap());
                                if (DungeonUtility.GetTilemap().GetTile(new Vector3Int(m_startPos.x - xAmount, m_startPos.y - y + 2, 0)) == null)
                                    BuildPathTile(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0));
                                else
                                    BuildPathTile(new Vector3Int(m_startPos.x + -xAmount, m_startPos.y - y + 1, 0));
                            }
                        }
                    }
                }
            }
        }
    }
}

