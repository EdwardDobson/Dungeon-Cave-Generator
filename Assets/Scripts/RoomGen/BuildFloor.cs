using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonGeneration
{
    public class BuildFloor
    {
        public static void FillFloor()
        {
            Vector3Int temp = new Vector3Int();
            for (int i = 0; i < DungeonUtility.GetWallDimensions().x + 1; ++i)
            {
                for (int a = 0; a < DungeonUtility.GetWallDimensions().y; ++a)
                {
                    BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a, 1, false);
                    temp = new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + a,0);
                    if(DungeonUtility.GetTilemap().GetTile(temp).name == "Floor")
                    DungeonUtility.AddFloorPositions(temp);
                }
            }
        }
    }
}
