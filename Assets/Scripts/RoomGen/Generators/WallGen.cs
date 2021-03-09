using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
namespace DungeonGeneration
{
    public class WallGen
    {
        static Tilemap m_walls;
        static Vector2Int m_wallDimensions = new Vector2Int();
        static List<Vector3Int> m_wallPositions = new List<Vector3Int>();
        public static void SetWallsTileMap(Tilemap _walls)
        {
            m_walls = _walls;
        }
        public static Tilemap GetTilemap()
        {
            return m_walls;
        }
        public static void PlaceWalls()
        {
            BoundsInt Bounds = DungeonUtility.GetTilemap().cellBounds;
            TileBase[] allTiles = DungeonUtility.GetTilemap().GetTilesBlock(Bounds);
            List<CustomTile> tilesWithinRange = new List<CustomTile>();
            TileHolder tileHolder = TileManager.GetTileHolder(TileType.Wall);
            for (int x = 0; x < Bounds.size.x; x++)
            {
                for (int y = 0; y < Bounds.size.y; y++)
                {
                    float randomFreq = Random.Range(1, tileHolder.Tiles.OrderByDescending(t => t.PickChance).First().PickChance);
                    tilesWithinRange = tileHolder.Tiles.Where(t => t.PickChance >= randomFreq).ToList();
                    TileBase tile = allTiles[x + y * Bounds.size.x];
                    int tempTileIndex;
                    tempTileIndex = Random.Range(0, tilesWithinRange.Count);
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (tile == null)
                    {
                        TileManager.PlaceTile(pos, tempTileIndex, null, m_walls, tilesWithinRange[tempTileIndex], DictionaryType.Walls);      
                        Tile tileT = m_walls.GetTile<Tile>(pos);
                        if (tilesWithinRange[tempTileIndex].SpriteVariations.Length >0)
                        {
                            Sprite sT = tilesWithinRange[tempTileIndex].SpriteVariations[Random.Range(0, tilesWithinRange[tempTileIndex].SpriteVariations.Length)];
                            if (sT != null)
                                tileT.sprite = sT;
                        }
                    }
                }
            }
        }
        public static void SetWallSizes(Vector2Int _walls)
        {
            m_wallDimensions = _walls;
        }
        public static void RandomiseWallSizes(int _wallMinX, int _wallMaxX, int _wallMinY, int _wallMaxY)
        {
            m_wallDimensions = new Vector2Int(Random.Range(_wallMinX, _wallMaxX), Random.Range(_wallMinY, _wallMaxY));
        }
        public static Vector2Int GetWallDimensions()
        {
            return m_wallDimensions;
        }
        public static List<Vector3Int> GetWallPositions()
        {
            return m_wallPositions;
        }
        public static void AddWallPositions(Vector3Int _pos)
        {
            m_wallPositions.Add(_pos);
        }
    }
}

