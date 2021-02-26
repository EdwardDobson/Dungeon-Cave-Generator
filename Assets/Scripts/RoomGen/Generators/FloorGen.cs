using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DungeonGeneration
{
    public class FloorGen
    {
        static List<Vector3Int> m_floorPositions = new List<Vector3Int>();
        public static void FillFloor()
        {
            for (int i = 0; i < WallGen.GetWallDimensions().x + 1; ++i)
            {
                for (int a = 0; a < WallGen.GetWallDimensions().y; ++a)
                {
                    PlaceFloorTile(i, a, DungeonUtility.GetBuildPoint());
                }
            }
        }
        public static List<Vector3Int> GetFloorPositions()
        {
            return m_floorPositions;
        }
        public static void AddFloorPositions(Vector3Int _pos)
        {
            m_floorPositions.Add(_pos);
        }
        static void PlaceFloorTile(int _index1, int _index2 , Vector2Int _buildPoint)
        {
            TileHolder tileHolder = TileManager.GetTileHolder(TileType.Floor);
            Vector3Int temp = new Vector3Int();
            float randomFreq = Random.Range(1, tileHolder.Tiles.OrderByDescending(t => t.PickChance).First().PickChance);
            List<CustomTile> tilesWithinRange = new List<CustomTile>();
            tilesWithinRange = tileHolder.Tiles.Where(t => t.PickChance >= randomFreq).ToList();
            int tempTileIndex;

            tempTileIndex = Random.Range(0, tilesWithinRange.Count);
            Vector3Int t = new Vector3Int(_buildPoint.x + _index1, _buildPoint.y + _index2, 0);
            if (!TileManager.GetTileDictionary().ContainsKey(t))
            {
                TileManager.BuildPiece(_buildPoint.x + _index1, _buildPoint.y + _index2, tempTileIndex, false, TileType.Floor, DungeonUtility.GetTilemap());
                TileManager.ChangeTileColour(DungeonUtility.GetTilemap(), new Vector3Int(_buildPoint.x + _index1, _buildPoint.y + _index2, 0), tilesWithinRange[tempTileIndex]);
                TileManager.FillDictionary(new Vector3Int(_buildPoint.x + _index1, _buildPoint.y + _index2, 0), tilesWithinRange[tempTileIndex], DungeonUtility.GetTilemap());
                temp = new Vector3Int(_buildPoint.x + _index1, _buildPoint.y + _index2, 0);
                if (DungeonUtility.GetTilemap().GetTile(temp).name.Contains("Floor"))
                    m_floorPositions.Add(temp);

            }
        }
        public static void FillFloorDiamond()
        {
            int MinRowLength = 1;
            int CurrentRowLength;
            int MaxRowAmount = 10;
            CurrentRowLength = MinRowLength;
            for (int y = 0; y < MaxRowAmount; ++y)
            {
                if (y < MaxRowAmount / 2)
                {
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        PlaceFloorTile(xLength, y, DungeonUtility.GetBuildPoint());
                    }
                    CurrentRowLength++;
                }
                if (y >= MaxRowAmount / 2)
                {
                    CurrentRowLength--;
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        PlaceFloorTile(xLength, y,DungeonUtility.GetBuildPoint());
                    }
                }
            }
            CurrentRowLength = MinRowLength;
            for (int y = 0; y < MaxRowAmount; ++y)
            {
                if (y < MaxRowAmount / 2)
                {

                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        PlaceFloorTile(-xLength, y, DungeonUtility.GetBuildPoint());
                    }
                    CurrentRowLength++;
                }
                if (y >= MaxRowAmount / 2)
                {
                    CurrentRowLength--;
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        PlaceFloorTile(-xLength, y, DungeonUtility.GetBuildPoint());
                    }

                }
            }
        }

        public static void FillFloorCircle()
        {
            int startAmount = 3;
            int index = 0;
            Vector2Int startPoint = new Vector2Int(DungeonUtility.GetBuildPoint().x + startAmount/2, DungeonUtility.GetBuildPoint().y);
            for (int i = 0; i < startAmount; ++i)
            {
                PlaceFloorTile(i, index, startPoint);
              PlaceFloorTile(-i, index, startPoint);
            }

            index++;
            for (int i = 0; i < startAmount + 2; ++i)
            {
                PlaceFloorTile(i, index, startPoint);
              PlaceFloorTile(-i, index, startPoint);
            }

            index++;
            for (int i = 0; i < startAmount + 3; ++i)
            {
                PlaceFloorTile(i, index, startPoint);
             PlaceFloorTile(-i, index, startPoint);
            }

            for (int a = 0; a < 2; ++a)
            {
                index++;
                for (int i = 0; i < startAmount + 4; ++i)
                {
                    PlaceFloorTile(i, index, startPoint);
                 PlaceFloorTile(-i, index, startPoint);
                }
            }
            for (int a = 0; a < 6; ++a)
            {
                index++;
                for (int i = 0; i < startAmount + 5; ++i)
                {
                    PlaceFloorTile(i, index, startPoint);
                  PlaceFloorTile(-i, index, startPoint);
                }
            }
            for (int a = 0; a < 2; ++a)
            {
                index++;
                for (int i = 0; i < startAmount + 4; ++i)
                {
                    PlaceFloorTile(i, index, startPoint);
                 PlaceFloorTile(-i, index, startPoint);
                }
            }

            index++;
            for (int i = 0; i < startAmount + 3; ++i)
            {
                PlaceFloorTile(i, index, startPoint);
                PlaceFloorTile(-i, index, startPoint);
            }

            index++;
            for (int i = 0; i < startAmount + 2; ++i)
            {
                PlaceFloorTile(i, index, startPoint);
               PlaceFloorTile(-i, index, startPoint);
            }

            index++;
            for (int i = 0; i < startAmount; ++i)
            {
                PlaceFloorTile(i, index, startPoint);
               PlaceFloorTile(-i, index, startPoint);
            }
        }
    }
}
