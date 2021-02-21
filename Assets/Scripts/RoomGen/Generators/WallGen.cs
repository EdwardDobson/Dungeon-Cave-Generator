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
            for (int x = 0; x < Bounds.size.x; x++)
            {
                for (int y = 0; y < Bounds.size.y; y++)
                {
                    float randomFreq = Random.Range(1, TileManager.GetTileHolder(TileType.Wall).Tiles.OrderByDescending(t => t.PickChance).First().PickChance);
                    tilesWithinRange = TileManager.GetTileHolder(TileType.Wall).Tiles.Where(t => t.PickChance >= randomFreq).ToList();
                    TileBase tile = allTiles[x + y * Bounds.size.x];
                    int tempTileIndex;
                    tempTileIndex = Random.Range(0, tilesWithinRange.Count);
                    if (tile == null)
                    {
                        TileManager.BuildPiece(x, y, Random.Range(0, tilesWithinRange.Count), false, TileType.Wall, m_walls);
                        TileManager.ChangeTileColour(m_walls, new Vector3Int(x, y, 0), tilesWithinRange[tempTileIndex]);
                     //   DungeonUtility.AddWallPositions(new Vector3Int(x, y, 0));
                        TileManager.FillDictionary(new Vector3Int(x, y, 0), tilesWithinRange, tempTileIndex, m_walls);
                    }
                }
            }
        }
        public static void BuildWall()
        {
            for (int i = 0; i < m_wallDimensions.x + 1; ++i)
            {
                TileManager.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y, 0, true, TileType.Wall, DungeonUtility.GetTilemap());
                TileManager.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + m_wallDimensions.y, 0, true, TileType.Wall, DungeonUtility.GetTilemap());
                for (int a = 0; a < m_wallDimensions.y + 1; ++a)
                {
                    TileManager.BuildPiece(DungeonUtility.GetBuildPoint().x, DungeonUtility.GetBuildPoint().y + a, 0, true, TileType.Wall, DungeonUtility.GetTilemap());
                    TileManager.BuildPiece(DungeonUtility.GetBuildPoint().x + m_wallDimensions.x, DungeonUtility.GetBuildPoint().y + a, 0, true, TileType.Wall, DungeonUtility.GetTilemap());
                    DungeonUtility.RemoveBuildPoint();
                }
            }
        }
        public static void RemoveWalls()
        {
            for (int i = 0; i < m_wallPositions.Count; ++i)
            {
                DungeonUtility.GetSurroundingPositions(i, m_wallPositions);
      
                if (DungeonUtility.GetTileSurrounding(0) && DungeonUtility.GetTileSurrounding(1))
                {
                    if (DungeonUtility.GetTileSurrounding(0).name.Contains("Floor") && DungeonUtility.GetTileSurrounding(1).name.Contains("Floor") || DungeonUtility.GetTileSurrounding(0).name == "Wall" && DungeonUtility.GetTileSurrounding(1).name.Contains("Floor") || DungeonUtility.GetTileSurrounding(0).name.Contains("Floor") && DungeonUtility.GetTileSurrounding(1).name.Contains("Wall"))
                    {
                        TileManager.ChangeTilePiece(m_wallPositions[i], 0, TileType.Floor, DungeonUtility.GetTilemap());
                    }
                }
                if (DungeonUtility.GetTileSurrounding(2) && DungeonUtility.GetTileSurrounding(3))
                {
                    if (DungeonUtility.GetTileSurrounding(2).name.Contains("Floor") && DungeonUtility.GetTileSurrounding(3).name.Contains("Floor") || DungeonUtility.GetTileSurrounding(2).name.Contains("Wall") && DungeonUtility.GetTileSurrounding(3).name.Contains("Floor") || DungeonUtility.GetTileSurrounding(2).name.Contains("Floor") && DungeonUtility.GetTileSurrounding(3).name.Contains("Wall"))
                    {
                        TileManager.ChangeTilePiece(m_wallPositions[i], 0, TileType.Floor, DungeonUtility.GetTilemap());
                    }
                }
                if (DungeonUtility.GetTileSurrounding(0) == null || DungeonUtility.GetTileSurrounding(1) == null || DungeonUtility.GetTileSurrounding(2) == null || DungeonUtility.GetTileSurrounding(3) == null)
                {
                    TileManager.ChangeTilePiece(m_wallPositions[i], 0, TileType.Wall, DungeonUtility.GetTilemap());
                }
                DungeonUtility.ClearSurroundPositions();
            }

        }
        public static void SetWallSizes(Vector2Int _walls)
        {
            m_wallDimensions = _walls;
        }
        public static void RandomiseWallSizes(int _wallMaxX, int _wallMaxY, int _wallMinX, int _wallMinY)
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

