using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonGeneration
{
    public class BuildFloor
    {
        public static void FillFloor()
        {
            TileHolder tileHolder = TileManager.GetTileHolder(TileType.Floor);
            Vector3Int temp = new Vector3Int();
            for (int i = 0; i < DungeonUtility.GetWallDimensions().x + 1; ++i)
            {
                for (int a = 0; a < DungeonUtility.GetWallDimensions().y; ++a)
                {
                    BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, Random.Range(0, tileHolder.Tiles.Count), false, TileType.Floor);
                    temp = new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a,0);
                    if(DungeonUtility.GetTilemap().GetTile(temp).name.Contains("Floor"))
                    DungeonUtility.AddFloorPositions(temp);
                }
            }
        }
    }
}
