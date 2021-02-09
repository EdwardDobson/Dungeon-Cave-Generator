using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneration
{
    public class WallGen
    {
        public static void BuildWall()
        {
            DungeonUtility.GetTilePositions().Clear();
     
            for (int i = 0; i < DungeonUtility.GetWallDimensions().x + 1; ++i)
            {
          
                BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y, 0);
                DungeonUtility.CheckIfWall();
                BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + DungeonUtility.GetWallDimensions().y, 0);
                DungeonUtility.RemoveBuildPoint();
                Vector3Int tempPos = new Vector3Int(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y, 0);
                DungeonUtility.AddWallPositions(tempPos);
            }
            for (int a = 0; a < DungeonUtility.GetWallDimensions().y + 1; ++a)
            {
                BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x, DungeonUtility.GetBuildPoint().y + a, 0);
                DungeonUtility.CheckIfWall();
                BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + DungeonUtility.GetWallDimensions().x, DungeonUtility.GetBuildPoint().y + a, 0);
                DungeonUtility.RemoveBuildPoint();
                Vector3Int tempPos = new Vector3Int(DungeonUtility.GetBuildPoint().x, DungeonUtility.GetBuildPoint().y + a, 0);
                DungeonUtility.AddWallPositions(tempPos);
            }
        }
        public static void RemoveWalls()
        {
            for (int i = 0; i < DungeonUtility.GetWallPositions().Count; ++i)
            {
                Vector3Int x1 = new Vector3Int(DungeonUtility.GetWallPositions()[i].x + 1, DungeonUtility.GetWallPositions()[i].y,0);
                Vector3Int x2 = new Vector3Int(DungeonUtility.GetWallPositions()[i].x - 1, DungeonUtility.GetWallPositions()[i].y, 0);
                Vector3Int y1 = new Vector3Int(DungeonUtility.GetWallPositions()[i].x, DungeonUtility.GetWallPositions()[i].y + 1, 0);
                Vector3Int y2 = new Vector3Int(DungeonUtility.GetWallPositions()[i].x, DungeonUtility.GetWallPositions()[i].y -1, 0);
                if(DungeonUtility.GetTilemap().GetTile(x1) && DungeonUtility.GetTilemap().GetTile(x2))
                {
        
                    if(DungeonUtility.GetTilemap().GetTile(x1).name == "Floor" && DungeonUtility.GetTilemap().GetTile(x2).name == "Floor" || DungeonUtility.GetTilemap().GetTile(x1).name == "Wall" && DungeonUtility.GetTilemap().GetTile(x2).name == "Floor" || DungeonUtility.GetTilemap().GetTile(x1).name == "Floor" && DungeonUtility.GetTilemap().GetTile(x2).name == "Wall")
                    {
                      //  Debug.Log("Tiles at x Positions");
                        DungeonUtility.GetTilemap().SetTile(DungeonUtility.GetWallPositions()[i], DungeonUtility.GetTiles()[1]);
                    }
                }
                if (DungeonUtility.GetTilemap().GetTile(y1) && DungeonUtility.GetTilemap().GetTile(y2))
                {
                    if (DungeonUtility.GetTilemap().GetTile(y1).name == "Floor" && DungeonUtility.GetTilemap().GetTile(y2).name == "Floor" || DungeonUtility.GetTilemap().GetTile(y1).name == "Wall" && DungeonUtility.GetTilemap().GetTile(y2).name == "Floor" ||  DungeonUtility.GetTilemap().GetTile(y1).name == "Floor" && DungeonUtility.GetTilemap().GetTile(y2).name == "Wall")
                    {
                   //     Debug.Log("Tiles at y Positions");
                        DungeonUtility.GetTilemap().SetTile(DungeonUtility.GetWallPositions()[i], DungeonUtility.GetTiles()[1]);
                    }
                }
            }
        }
    }
}

