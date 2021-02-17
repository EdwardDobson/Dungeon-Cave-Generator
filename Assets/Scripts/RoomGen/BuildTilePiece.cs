using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneration
{
    public class BuildTilePiece
    {
        public static void BuildPiece(int _value1, int _value2, int _tileIndex, bool _isWallPiece , TileType _type, Tilemap _map)
        {
            Vector3Int posY = new Vector3Int(_value1, _value2, 0);
            if (_map.GetTile(posY) == null)
                _map.SetTile(posY,TileManager.GetCertainTile(_type, _tileIndex));
            DungeonUtility.SetTilePosition(posY);
            if(_isWallPiece && !DungeonUtility.GetWallPositions().Contains(posY))
                DungeonUtility.AddWallPositions(posY);
        }
        public static void ChangeTilePiece(Vector3Int _pos, int _tileIndex, TileType _type,Tilemap _map)
        {
            _map.SetTile(_pos, TileManager.GetCertainTile(_type, _tileIndex));
        }
        public static void RemoveTilePiece(Vector3Int _pos, Tilemap _map)
        {
            _map.SetTile(_pos, null);
        }
    }
}

