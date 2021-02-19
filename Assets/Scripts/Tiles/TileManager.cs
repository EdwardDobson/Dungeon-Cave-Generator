using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace DungeonGeneration
{
    public struct TileData
    {
        public TileBase TileBase;
       public CustomTile CustomTile;
    }
    public class TileManager
    {
        static Object[] tiledatas;
        static TileHolder m_tileHolderToReturn;
        static List<TileHolder> m_tileHolders;
        static CustomTile m_tileToReturn;
        static List<CustomTile> m_tilesToReturn = new List<CustomTile>();
        static Dictionary<Vector3Int, TileData> m_tileDatas;
        public  static void LoadTileManager()
        {
            tiledatas = Resources.LoadAll("Tiles", typeof(TileHolder));
            m_tileDatas = new Dictionary<Vector3Int, TileData>();
            m_tileHolders = new List<TileHolder>();
            foreach (TileHolder t in tiledatas)
            {
                if(!m_tileHolders.Contains(t))
                m_tileHolders.Add(t);
            }
        }
        public static void FillDictionary(Vector3Int _pos, List<CustomTile> _customTiles, int _index, Tilemap _map)
        {
            TileData td = new TileData();
            td.CustomTile = _customTiles[_index];
            td.TileBase = _map.GetTile(_pos);
            if (!m_tileDatas.ContainsKey(_pos))
                m_tileDatas.Add(_pos, td);
        }

        public static Dictionary<Vector3Int, TileData> GetTileDictionary()
        {
            return m_tileDatas;
        }
        public static CustomTile GetCertainTile(TileType _type, int _index)
        {
            foreach (TileHolder t in m_tileHolders)
            {
                if(t.name.Contains(_type.ToString()))
                    m_tileToReturn = t.Tiles[_index];
            }
            return m_tileToReturn;
        }
        public static List<CustomTile> GetAllTiles(TileType _type)
        {
            foreach (TileHolder t in m_tileHolders)
            {
                if (t.name.Contains(_type.ToString()))
                    m_tilesToReturn = t.Tiles;
            }
            return m_tilesToReturn;
        }
        public static TileHolder GetTileHolder(TileType _type)
        {
            foreach (TileHolder t in m_tileHolders)
            {
                if (t.name.Contains(_type.ToString()))
                    m_tileHolderToReturn = t;
            }
            return m_tileHolderToReturn;
        }

        public static void BuildPiece(int _value1, int _value2, int _tileIndex, bool _isWallPiece, TileType _type, Tilemap _map)
        {
            Vector3Int posY = new Vector3Int(_value1, _value2, 0);
            if (_map.GetTile(posY) == null)
                _map.SetTile(posY, GetCertainTile(_type, _tileIndex).Tile[0]);
            DungeonUtility.SetTilePosition(posY);
            if (_isWallPiece && !WallGen.GetWallPositions().Contains(posY))
                WallGen.AddWallPositions(posY);
        }
        public static void ChangeTilePiece(Vector3Int _pos, int _tileIndex, TileType _type, Tilemap _map)
        {
            _map.SetTile(_pos, GetCertainTile(_type, _tileIndex).Tile[0]);
        }
        public static void RemoveTilePiece(Vector3Int _pos, Tilemap _map)
        {
            _map.SetTile(_pos, null);
        }
        public static void ChangeTileColour(Tilemap _map, Vector3Int _tilePos, CustomTile _customTile)
        {
            _map.SetTileFlags(_tilePos, TileFlags.None);
            _map.SetColor(_tilePos, _customTile.TileColour);
        }
    }
}
