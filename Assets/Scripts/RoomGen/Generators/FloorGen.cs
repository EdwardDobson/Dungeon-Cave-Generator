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
        public static void FillFloorDiamond(int _minRowLength, int _maxRowAmount)
        {
            int CurrentRowLength;
            CurrentRowLength = _minRowLength;
            for (int y = 0; y < _maxRowAmount; ++y)
            {
                if (y < _maxRowAmount / 2)
                {
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        if(DungeonUtility.GetBuildPoint().x + xLength < DungeonUtility.GetDungeonDimensions().x + 1 && DungeonUtility.GetBuildPoint().y + y < DungeonUtility.GetDungeonDimensions().y +1 
                            && DungeonUtility.GetBuildPoint().x + xLength > -1 && DungeonUtility.GetBuildPoint().y + y > -1)
                        PlaceFloorTile(xLength, y, DungeonUtility.GetBuildPoint());
                    }
                    CurrentRowLength++;
                }
                if (y >= _maxRowAmount / 2)
                {
                    CurrentRowLength--;
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        if (DungeonUtility.GetBuildPoint().x + xLength < DungeonUtility.GetDungeonDimensions().x + 1 && DungeonUtility.GetBuildPoint().y + y < DungeonUtility.GetDungeonDimensions().y + 1
               && DungeonUtility.GetBuildPoint().x + xLength > -1 && DungeonUtility.GetBuildPoint().y + y > -1)
                            PlaceFloorTile(xLength, y,DungeonUtility.GetBuildPoint());
                    }
                }
            }
            CurrentRowLength = _minRowLength;
            for (int y = 0; y < _maxRowAmount; ++y)
            {
                if (y < _maxRowAmount / 2)
                {

                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        if (DungeonUtility.GetBuildPoint().x + -xLength < DungeonUtility.GetDungeonDimensions().x + 1&& DungeonUtility.GetBuildPoint().y + y < DungeonUtility.GetDungeonDimensions().y + 1
               && DungeonUtility.GetBuildPoint().x + -xLength > -1 && DungeonUtility.GetBuildPoint().y + y > -1)
                            PlaceFloorTile(-xLength, y, DungeonUtility.GetBuildPoint());
                    }
                    CurrentRowLength++;
                }
                if (y >= _maxRowAmount / 2)
                {
                    CurrentRowLength--;
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        if (DungeonUtility.GetBuildPoint().x + -xLength < DungeonUtility.GetDungeonDimensions().x + 1 && DungeonUtility.GetBuildPoint().y + y < DungeonUtility.GetDungeonDimensions().y + 1
               && DungeonUtility.GetBuildPoint().x + -xLength > -1 && DungeonUtility.GetBuildPoint().y + y > -1)
                            PlaceFloorTile(-xLength, y, DungeonUtility.GetBuildPoint());
                    }

                }
            }
        }
        static void CircleFill(List<Vector2Int> _circlePoints, Vector2Int _startPoint, int _indexX, int _indexY)
        {
            Vector2Int point = new Vector2Int(_startPoint.x + _indexX, _startPoint.y + _indexY);
            if (point.x < DungeonUtility.GetDungeonDimensions().x + 1 && point.y < DungeonUtility.GetDungeonDimensions().y + 1 && point.x > -1 && point.y > -1)
                _circlePoints.Add(point);
        }
        public static void FillFloorCircle(int _startAmount, int _middleAmount)
        {
            Vector2Int startPoint = new Vector2Int(DungeonUtility.GetBuildPoint().x +_startAmount / 2, DungeonUtility.GetBuildPoint().y);
            List<Vector2Int> CirclePoints = new List<Vector2Int>();
            //Right Side
            for(int i =0; i < _startAmount; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, 0);
            }
            for (int i = 0; i < _startAmount + 2; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, 1);
            }
            for (int i = 0; i < _startAmount + 3; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, 2);
            }
            for (int i = 0; i < _startAmount + 4; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, 3);

            }
            for (int i = 0; i < _startAmount + 4; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, 4);
            }
            int num = 0;
            for (int a = 0; a < _middleAmount; ++a)
            {
                for (int i = 0; i < _startAmount + 5; ++i)
                {
                    CircleFill(CirclePoints, startPoint, i, 5 + a);
                }
                num = 5 + a;
            }
            for (int i = 0; i < _startAmount + 4; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, num + 1);
            }
            for (int i = 0; i < _startAmount + 4; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, num + 2);
            }
            for (int i = 0; i < _startAmount + 3; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, num + 3);
            }
            for (int i = 0; i < _startAmount + 1; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, num + 4);
            }
            for (int i = 0; i < _startAmount; ++i)
            {
                CircleFill(CirclePoints, startPoint, i, num + 5);
            }
            //LeftSide
            for (int i = 0; i < _startAmount ; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, 1);
            }
            for (int i = 0; i < _startAmount  + 1; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, 2);
            }
            for (int i = 0; i < _startAmount  + 2; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, 3);
            }
            for (int i = 0; i < _startAmount + 2; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, 4);
            }
            int num2 = 0;
            for(int a = 0; a < _middleAmount; ++a)
            {
                for (int i = 0; i < _startAmount + 3; ++i)
                {
                    
                    CircleFill(CirclePoints, startPoint, -i, 5 + a);
                }
                num2 = 5 + a;
            }
            for (int i = 0; i < _startAmount + 2; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, num2 + 1);
            }
            for (int i = 0; i < _startAmount+ 2; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, num2 + 2);
            }
            for (int i = 0; i < _startAmount  + 1; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, num2 + 3);
            }
            for (int i = 0; i < _startAmount  - 1; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, num2 + 4);
            }

            for (int Cp = 0; Cp < CirclePoints.Count; ++Cp)
            {
                PlaceFloorTile(0, 0, CirclePoints[Cp]);
            }
        }
    }
}
