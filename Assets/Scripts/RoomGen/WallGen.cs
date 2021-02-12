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
                BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y, 0, true);
                DungeonUtility.CheckIfWall();
                BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + DungeonUtility.GetWallDimensions().y, 0, true);
                for (int a = 0; a < DungeonUtility.GetWallDimensions().y + 1; ++a)
                {
                    BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x, DungeonUtility.GetBuildPoint().y + a, 0, true);
                    DungeonUtility.CheckIfWall();
                    BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + DungeonUtility.GetWallDimensions().x, DungeonUtility.GetBuildPoint().y + a, 0, true);
                    DungeonUtility.RemoveBuildPoint();
                }
            }
        }
        public static void RemoveWalls()
        {
            for (int i = 0; i < DungeonUtility.GetWallPositions().Count; ++i)
            {
                DungeonUtility.GetSurroundingPositions(i,DungeonUtility.GetWallPositions());
      
                if (DungeonUtility.GetTileSurrounding(0) && DungeonUtility.GetTileSurrounding(1))
                {
                    if (DungeonUtility.GetTileSurrounding(0).name == "Floor" && DungeonUtility.GetTileSurrounding(1).name == "Floor" || DungeonUtility.GetTileSurrounding(0).name == "Wall" && DungeonUtility.GetTileSurrounding(1).name == "Floor" || DungeonUtility.GetTileSurrounding(0).name == "Floor" && DungeonUtility.GetTileSurrounding(1).name == "Wall")
                    {
                        //  Debug.Log("Tiles at x Positions");
                        DungeonUtility.GetTilemap().SetTile(DungeonUtility.GetWallPositions()[i], DungeonUtility.GetTiles()[1]);
                    }
                }
                if (DungeonUtility.GetTileSurrounding(2) && DungeonUtility.GetTileSurrounding(3))
                {
                    if (DungeonUtility.GetTileSurrounding(2).name == "Floor" && DungeonUtility.GetTileSurrounding(3).name == "Floor" || DungeonUtility.GetTileSurrounding(2).name == "Wall" && DungeonUtility.GetTileSurrounding(3).name == "Floor" || DungeonUtility.GetTileSurrounding(2).name == "Floor" && DungeonUtility.GetTileSurrounding(3).name == "Wall")
                    {
                        //     Debug.Log("Tiles at y Positions");
                        DungeonUtility.GetTilemap().SetTile(DungeonUtility.GetWallPositions()[i], DungeonUtility.GetTiles()[1]);
                    }
                }
                if (DungeonUtility.GetTileSurrounding(0) == null || DungeonUtility.GetTileSurrounding(1) == null || DungeonUtility.GetTileSurrounding(2) == null || DungeonUtility.GetTileSurrounding(3) == null)
                {
                    DungeonUtility.GetTilemap().SetTile(DungeonUtility.GetWallPositions()[i], DungeonUtility.GetTiles()[0]);
                }
                DungeonUtility.ClearSurroundPositions();
            }

        }

        public static void AddWallPosition()
        {
            for (int a = 0; a < DungeonUtility.GetFloorPositions().Count; ++a)
            {
                DungeonUtility.GetSurroundingPositions(a, DungeonUtility.GetFloorPositions());
                for (int i = 0; i < 4; ++i)
                {
                    if (DungeonUtility.GetTileSurrounding(i).name == "Wall")
                    {
                        if (!DungeonUtility.GetWallForDoorsPositions().Contains(DungeonUtility.GetSurroundingPosition(i)))
                            DungeonUtility.AddWallForDoorsPositions(DungeonUtility.GetSurroundingPosition(i));
                    }
                }
                DungeonUtility.ClearSurroundPositions();
            }
        }
    }
}

