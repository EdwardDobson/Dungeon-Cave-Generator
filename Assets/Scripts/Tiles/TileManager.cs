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
        [SerializeField]
        static Object[] tiledatas;
        static TileHolder m_floor;
        static TileHolder m_wall;
        static TileHolder m_path;
        static TileHolder m_door;
        static TileHolder m_tileHolderToReturn;
        static CustomTile m_tileToReturn;
        static List<CustomTile> m_tilesToReturn = new List<CustomTile>();
        static Dictionary<List<TileBase>, CustomTile> m_dataFromTilesFloor;
        static Dictionary<Vector3Int, TileData> m_tileDatas;
        static void LoadTileDatas()
        {
            tiledatas = Resources.LoadAll("Tiles", typeof(TileHolder));
            m_dataFromTilesFloor = new Dictionary<List<TileBase>, CustomTile>();
            m_tileDatas = new Dictionary<Vector3Int, TileData>();
        }

        public static void FillTilesList()
        {
            LoadTileDatas();
            foreach (TileHolder t in tiledatas)
            {
                if (t.name == "FloorTiles")
                    m_floor = t;
                if (t.name == "WallTiles")
                    m_wall = t;
                if (t.name == "PathTiles")
                    m_path = t;
                if (t.name == "DoorTiles")
                    m_door = t;
            }
        }
        public static void FillDictionary(Vector3Int _pos,TileData _tileDataStruct)
        {
            m_tileDatas.Add(_pos, _tileDataStruct);
        }
        public static Dictionary<Vector3Int, TileData> GetTileDictionary()
        {
            return m_tileDatas;
        }

        public static CustomTile GetCertainTile(TileType _type, int _index)
        {
            switch (_type)
            {
                case TileType.Floor:
                    m_tileToReturn = m_floor.Tiles[_index];
                    break;
                case TileType.Wall:
                    m_tileToReturn = m_wall.Tiles[_index];
                    break;
                case TileType.Path:
                    m_tileToReturn = m_path.Tiles[_index];
                    break;
                case TileType.Door:
                    m_tileToReturn = m_door.Tiles[_index];
                    break;
                default:
                    Debug.LogError("No Tile Set Exists");
                    break;
            }
            return m_tileToReturn;
        }
        public static List<CustomTile> GetAllTiles(TileType _type)
        {
            switch (_type)
            {
                case TileType.Floor:
                    m_tilesToReturn = m_floor.Tiles;
                    break;
                case TileType.Wall:
                    m_tilesToReturn = m_wall.Tiles;
                    break;
                case TileType.Path:
                    m_tilesToReturn = m_path.Tiles;
                    break;
                case TileType.Door:
                    m_tilesToReturn = m_door.Tiles;
                    break;
                default:
                    Debug.LogError("No Tile Set Exists");
                    break;
            }
            return m_tilesToReturn;
        }
        public static TileHolder GetTileHolder(TileType _type)
        {
            switch (_type)
            {
                case TileType.Floor:
                    m_tileHolderToReturn = m_floor;
                    break;
                case TileType.Wall:
                    m_tileHolderToReturn = m_wall;
                    break;
                case TileType.Path:
                    m_tileHolderToReturn = m_path;
                    break;
                case TileType.Door:
                    m_tileHolderToReturn = m_door;
                    break;
                default:
                    Debug.LogError("No Tile Holder Exists");
                    break;
            }
            return m_tileHolderToReturn;
        }

        public static void BuildPiece(int _value1, int _value2, int _tileIndex, bool _isWallPiece, TileType _type, Tilemap _map)
        {
            Vector3Int posY = new Vector3Int(_value1, _value2, 0);
            if (_map.GetTile(posY) == null)
                _map.SetTile(posY, GetCertainTile(_type, _tileIndex).Tile[0]);
            DungeonUtility.SetTilePosition(posY);
            if (_isWallPiece && !DungeonUtility.GetWallPositions().Contains(posY))
                DungeonUtility.AddWallPositions(posY);
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
