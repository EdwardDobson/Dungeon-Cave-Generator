using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneration
{
    public class BuildTilePiece
    {
        public static void BuildPiece(int _value1, int _value2, int _tileIndex, bool _isWallPiece , TileType _type)
        {
            Vector3Int posY = new Vector3Int(_value1, _value2, 0);
            if (DungeonUtility.GetTilemap().GetTile(posY) == null)
                DungeonUtility.GetTilemap().SetTile(posY,TileManager.GetCertainTile(_type, _tileIndex));
            DungeonUtility.SetTilePosition(posY);
            if(_isWallPiece && !DungeonUtility.GetWallPositions().Contains(posY))
                DungeonUtility.AddWallPositions(posY);
        }
        public static void ChangeTilePiece(Vector3Int _pos, int _tileIndex, TileType _type)
        {
            DungeonUtility.GetTilemap().SetTile(_pos, TileManager.GetCertainTile(_type, _tileIndex));
        }
        public static void RemoveTilePiece(Vector3Int _pos)
        {
            DungeonUtility.GetTilemap().SetTile(_pos, null);
        }
    }
}

