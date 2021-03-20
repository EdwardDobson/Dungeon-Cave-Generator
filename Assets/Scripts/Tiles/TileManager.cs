using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace DungeonGeneration
{
    public enum DictionaryType
    {
        Walls,
        Floor
    }
    public struct TileData
    {
        public TileBase TileBase;
        public CustomTile CustomTile;
    }
    public class TileManager
    {
        static Object[] tiledatas;
        static Object[] ingredients;
        static TileHolder m_tileHolderToReturn;
        static List<TileHolder> m_tileHolders;
        static CustomTile m_tileToReturn;
        static List<CustomTile> m_tilesToReturn = new List<CustomTile>();
        static List<CustomTile> m_allTiles = new List<CustomTile>();
        static Dictionary<Vector3Int, TileData> m_tileDatasFloor;
        static Dictionary<Vector3Int, TileData> m_tileDatasWalls;
       public static int idIndex = 1;
        public  static void LoadTileManager()
        {
            idIndex = 1;
            tiledatas = Resources.LoadAll("Tiles", typeof(TileHolder));
            ingredients = Resources.LoadAll("Crafting Ingredients", typeof(Item));
            m_tileDatasFloor = new Dictionary<Vector3Int, TileData>();
            m_tileDatasWalls = new Dictionary<Vector3Int, TileData>();
            m_tileHolders = new List<TileHolder>();
            foreach (TileHolder t in tiledatas)
            {
                if(!m_tileHolders.Contains(t))
                m_tileHolders.Add(t);
            }
            for (int i = 0; i < m_tileHolders.Count; ++i)
            {
                for(int a = 0; a < m_tileHolders[i].Tiles.Count; ++a)
                {
                    m_tileHolders[i].Tiles[a].ID = 0;
                    if(!m_allTiles.Contains(m_tileHolders[i].Tiles[a]))
                    m_allTiles.Add(m_tileHolders[i].Tiles[a]);
                }
            }
            for (int i = 0; i < m_allTiles.Count; ++i)
            {
                //Makes sure items are assigned to tiles
                foreach (Item t in ingredients)
                {
                    if (m_allTiles[i].TileName == t.Name)
                        m_allTiles[i].Item = t;
                }
               m_allTiles[i].ID = idIndex;
                idIndex++;
                Debug.Log(m_allTiles.Count);
            }
        }
        public static void FillDictionary(Vector3Int _pos, CustomTile _customTile, Tilemap _map,DictionaryType _dirType)
        {
            TileData td = new TileData();
            td.CustomTile = _customTile;
            td.TileBase = _map.GetTile(_pos);
            switch (_dirType)
            {
                case DictionaryType.Walls:
                    if (!m_tileDatasWalls.ContainsKey(_pos))
                        m_tileDatasWalls.Add(_pos, td);
                    break;
                case DictionaryType.Floor:
                    if (!m_tileDatasFloor.ContainsKey(_pos))
                        m_tileDatasFloor.Add(_pos, td);
                    break;
            }
        }
        public static Dictionary<Vector3Int, TileData> GetTileDictionaryFloor()
        {
            return m_tileDatasFloor;
        }
        public static Dictionary<Vector3Int, TileData> GetTileDictionaryWalls()
        {
            return m_tileDatasWalls;
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
        public static TileBase GetCertainTileBase(CustomTile _tile)
        {
            return _tile.Tile[0];
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
        public static void PlaceTile(Vector3Int _pos,int _index, Tilemap _mapToRemove, Tilemap _mapToPlace, CustomTile _tile, DictionaryType _dirType)
        {
            RemoveTilePiece(_pos, _mapToRemove);
            if(_dirType == DictionaryType.Walls)
            {
                if(m_tileDatasWalls != null)
                m_tileDatasWalls.Remove(_pos);
            }
            if (_dirType == DictionaryType.Floor)
            {
                if (m_tileDatasFloor != null)
                    m_tileDatasFloor.Remove(_pos);
            }
            if (_mapToPlace.GetTile(_pos) == null)
                _mapToPlace.SetTile(_pos, _tile.Tile[0]);
            FillDictionary(_pos, _tile, _mapToPlace, _dirType);
            ChangeTileColour(_mapToPlace, _pos, _tile);
        }
        public static void ChangeTilePiece(Vector3Int _pos, int _tileIndex, TileType _type, Tilemap _map)
        {
            _map.SetTile(_pos, GetCertainTile(_type, _tileIndex).Tile[0]);
        }
        public static void ChangeTilePieceDig(Vector3Int _pos,TileBase _base, Tilemap _map)
        {
            _map.SetTile(_pos, _base);
        }
        public static void RemoveTilePiece(Vector3Int _pos, Tilemap _map)
        {
            if(_map != null)
            _map.SetTile(_pos, null);
        }
        public static void ChangeTileColour(Tilemap _map, Vector3Int _tilePos, CustomTile _customTile)
        {
            if (_customTile.ShouldUseColour)
            {
                _map.SetTileFlags(_tilePos, TileFlags.None);
                _map.SetColor(_tilePos, _customTile.TileColour);
            }
        }
    }
}
