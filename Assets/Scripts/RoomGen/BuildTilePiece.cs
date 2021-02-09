using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneration
{
    public class BuildTilePiece
    {
        public static void BuildPiece(int _value1, int _value2, int _tileIndex)
        {
            Vector3Int posY = new Vector3Int(_value1, _value2, 0);
            if (DungeonUtility.GetTilemap().GetTile(posY) == null)
                DungeonUtility.GetTilemap().SetTile(posY, DungeonUtility.GetTiles()[_tileIndex]);
            DungeonUtility.SetTilePosition(posY);
        }
    }
}

