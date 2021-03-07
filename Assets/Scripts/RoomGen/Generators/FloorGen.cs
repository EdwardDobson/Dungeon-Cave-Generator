using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneration
{
    public class FloorGen
    {
        static List<Vector3Int> m_floorPositions = new List<Vector3Int>();
        static List<Vector2Int> m_floorTilePositions = new List<Vector2Int>();
        public static void Square()
        {
            for (int i = 0; i < WallGen.GetWallDimensions().x + 1; ++i)
            {
                for (int a = 0; a < WallGen.GetWallDimensions().y; ++a)
                {
                    Vector2Int pos = new Vector2Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a);
                    AddToFloorTilePositions(pos);
                }
            }
        }
        public static void AddToFloorTilePositions(Vector2Int _pointToAdd)
        {
            if (_pointToAdd.x > -1 && _pointToAdd.y > -1)
                m_floorTilePositions.Add(_pointToAdd);
        }
        public static List<Vector2Int> GetFloorTilePositions()
        {
            return m_floorTilePositions;
        }
        public static List<Vector3Int> GetFloorPositions()
        {
            return m_floorPositions;
        }
        public static void AddFloorPositions(Vector3Int _pos)
        {
            if (_pos.x > -1 && _pos.y > -1)
                m_floorPositions.Add(_pos);
        }
        public static void PlaceFloorTile(Vector2Int _buildPoint)
        {
            Vector3Int t = new Vector3Int(_buildPoint.x, _buildPoint.y, 0);
            if (!TileManager.GetTileDictionaryFloor().ContainsKey(t))
            {
                TileHolder tileHolder = TileManager.GetTileHolder(TileType.Floor);
                float randomFreq = Random.Range(1, tileHolder.Tiles.OrderByDescending(t => t.PickChance).First().PickChance);
                List<CustomTile> tilesWithinRange = new List<CustomTile>();
                tilesWithinRange = tileHolder.Tiles.Where(t => t.PickChance >= randomFreq).ToList();
                int tempTileIndex;
                tempTileIndex = Random.Range(0, tilesWithinRange.Count);
                TileManager.BuildPiece(t, tilesWithinRange[tempTileIndex].Tile[0], DungeonUtility.GetTilemap());
                TileManager.ChangeTileColour(DungeonUtility.GetTilemap(), new Vector3Int(_buildPoint.x, _buildPoint.y, 0), tilesWithinRange[tempTileIndex]);
                TileManager.FillDictionary(new Vector3Int(_buildPoint.x, _buildPoint.y, 0), tilesWithinRange[tempTileIndex], DungeonUtility.GetTilemap(), DictionaryType.Floor);
                m_floorPositions.Add(t);
                Tile tileT = DungeonUtility.GetTilemap().GetTile<Tile>(t);
                if (tilesWithinRange[tempTileIndex].SpriteVariations.Length > 0)
                {
                    Sprite sT = tilesWithinRange[tempTileIndex].SpriteVariations[Random.Range(0, tilesWithinRange[tempTileIndex].SpriteVariations.Length)];
                    if (sT != null)
                        tileT.sprite = sT;
                }
            }
        }
        public static void LShape(int _roomLength, int _roomHeight, int _directionIndex)
        {
            Square();
            for (int i = 0; i < _roomLength + 1; ++i)
            {
                for (int a = 0; a < _roomHeight; ++a)
                {
                    switch (_directionIndex)
                    {
                        case 0://Right
                            Vector2Int pos = ShapeDirectionVector(_roomLength + i + 1, 0 + a);
                            AddToFloorTilePositions(pos);
                            break;
                        case 1://Left
                            Vector2Int pos2 = ShapeDirectionVector(0 - i, 0 + a);
                            AddToFloorTilePositions(pos2);
                            break;
                        case 2://UpRight
                            Vector2Int pos3 = ShapeDirectionVector(WallGen.GetWallDimensions().x + 1 + i, WallGen.GetWallDimensions().y - _roomHeight + a);
                            AddToFloorTilePositions(pos3);
                            break;
                        case 3://UpLeft
                            Vector2Int pos4 = ShapeDirectionVector(0 - i, WallGen.GetWallDimensions().y - _roomHeight + a);
                            AddToFloorTilePositions(pos4);
                            break;
                    }

                }
            }
        }
        static Vector2Int ShapeDirectionVector(int _roomLength, int _roomHeight)
        {
            Vector2Int pos = new Vector2Int(DungeonUtility.GetBuildPoint().x + _roomLength, DungeonUtility.GetBuildPoint().y + _roomHeight);
            return pos;
        }
        public static void TShape(int _stemWidth, int _stemHeight, int _roomLength, int _roomHeight, int _directionIndex)
        {
            for (int i = 0; i < _stemWidth + 1; ++i)
            {
                for (int a = 0; a < _stemHeight; ++a)
                {
                    Vector2Int pos = new Vector2Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a);
                    AddToFloorTilePositions(pos);
                }
            }
            for (int i = 0; i < _roomLength + 1; ++i)
            {
                for (int a = 0; a < _roomHeight; ++a)
                {
                    switch (_directionIndex)
                    {
                        case 0://Right
                            Vector2Int pos = ShapeDirectionVector(_stemWidth + i, ((_stemHeight - _roomHeight) / 2) + a);
                            AddToFloorTilePositions(pos);
                            break;
                        case 1://Left
                            Vector2Int pos2 = ShapeDirectionVector((_stemWidth - _roomHeight) / 2 - i, ((_stemHeight - _roomHeight) / 2) + a);
                            AddToFloorTilePositions(pos2);
                            break;
                        case 2://Up
                            Vector2Int pos3 = ShapeDirectionVector(_stemWidth / 2 - i, _stemHeight + a);
                            Vector2Int pos3a = ShapeDirectionVector(_stemWidth / 2 + i, _stemHeight + a);
                            AddToFloorTilePositions(pos3);
                            AddToFloorTilePositions(pos3a);
                            break;
                        case 3://Down
                            Vector2Int pos4 = ShapeDirectionVector(_stemWidth / 2 - i, a);
                            Vector2Int pos4a = ShapeDirectionVector(_stemWidth / 2 + i, a);
                            AddToFloorTilePositions(pos4);
                            AddToFloorTilePositions(pos4a);
                            break;
                    }
                }
            }
        }
        public static void Diamond(int _minRowLength, int _maxRowAmount)
        {
            int CurrentRowLength;
            CurrentRowLength = _minRowLength;
            for (int y = 0; y < _maxRowAmount; ++y)
            {
                if (y < _maxRowAmount / 2)
                {
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        Vector2Int pos = ShapeDirectionVector(xLength, y);
                        AddToFloorTilePositions(pos);
                    }
                    CurrentRowLength++;
                }
                if (y >= _maxRowAmount / 2)
                {
                    CurrentRowLength--;
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        Vector2Int pos = ShapeDirectionVector(xLength, +y);
                        AddToFloorTilePositions(pos);
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
                        Vector2Int pos = ShapeDirectionVector(-xLength, y);
                        AddToFloorTilePositions(pos);
                    }
                    CurrentRowLength++;
                }
                if (y >= _maxRowAmount / 2)
                {
                    CurrentRowLength--;
                    for (int xLength = 0; xLength < CurrentRowLength; ++xLength)
                    {
                        Vector2Int pos = ShapeDirectionVector(-xLength, y);
                        AddToFloorTilePositions(pos);
                    }
                }
            }
        }
        static void CircleFill(List<Vector2Int> _circlePoints, Vector2Int _startPoint, int _indexX, int _indexY)
        {
            Vector2Int point = new Vector2Int(_startPoint.x + _indexX, _startPoint.y + _indexY);
            if (point.x > -1 && point.y > -1)
                AddToFloorTilePositions(point);
        }
        public static void Circle(int _startAmount, int _middleAmount)
        {
            Vector2Int startPoint = new Vector2Int(DungeonUtility.GetBuildPoint().x + _startAmount / 2, DungeonUtility.GetBuildPoint().y);
            List<Vector2Int> CirclePoints = new List<Vector2Int>();
            //Right Side
            for (int i = 0; i < _startAmount; ++i)
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
            for (int i = 0; i < _startAmount; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, 1);
            }
            for (int i = 0; i < _startAmount + 1; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, 2);
            }
            for (int i = 0; i < _startAmount + 2; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, 3);
            }
            for (int i = 0; i < _startAmount + 2; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, 4);
            }
            int num2 = 0;
            for (int a = 0; a < _middleAmount; ++a)
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
            for (int i = 0; i < _startAmount + 2; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, num2 + 2);
            }
            for (int i = 0; i < _startAmount + 1; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, num2 + 3);
            }
            for (int i = 0; i < _startAmount - 1; ++i)
            {
                CircleFill(CirclePoints, startPoint, -i, num2 + 4);
            }
        }
    }
}
