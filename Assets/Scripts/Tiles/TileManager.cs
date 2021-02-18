using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace DungeonGeneration
{
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
        static Dictionary<TileBase, CustomTile> m_dataFromTilesFloor;
        static List<CustomTileClass> m_tiles = new List<CustomTileClass>();
        static void LoadTileDatas()
        {
            tiledatas = Resources.LoadAll("Tiles", typeof(TileHolder));
            m_dataFromTilesFloor = new Dictionary<TileBase, CustomTile>();
            
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
        public static void FillTileDictionary()
        {
            foreach (CustomTile c in m_floor.Tiles)
            {
                m_dataFromTilesFloor.Add(c.Tile, c);
            }
            foreach (CustomTile c in m_wall.Tiles)
            {
                m_dataFromTilesFloor.Add(c.Tile, c);
            }
            foreach (CustomTile c in m_path.Tiles)
            {
                m_dataFromTilesFloor.Add(c.Tile, c);
            }
            foreach (CustomTile c in m_door.Tiles)
            {
                m_dataFromTilesFloor.Add(c.Tile, c);
            }
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
        /// <summary>
        /// Gets a tile from the dictionary of custom tiles
        /// </summary>
        /// <param name="_tile"></param>
        /// <returns></returns>
        public static CustomTile GetCustomTile(TileBase _tile)
        {
            return m_dataFromTilesFloor[_tile];
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
    }
}
