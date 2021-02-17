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
                BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y, 0, true, TileType.Wall, DungeonUtility.GetTilemap());
                DungeonUtility.CheckIfWall();
                BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + i, DungeonUtility.GetBuildPoint().y + DungeonUtility.GetWallDimensions().y, 0, true, TileType.Wall, DungeonUtility.GetTilemap());
                for (int a = 0; a < DungeonUtility.GetWallDimensions().y + 1; ++a)
                {
                    BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x, DungeonUtility.GetBuildPoint().y + a, 0, true, TileType.Wall, DungeonUtility.GetTilemap());
                    DungeonUtility.CheckIfWall();
                    BuildTilePiece.BuildPiece(DungeonUtility.GetBuildPoint().x + DungeonUtility.GetWallDimensions().x, DungeonUtility.GetBuildPoint().y + a, 0, true, TileType.Wall, DungeonUtility.GetTilemap());
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
                    if (DungeonUtility.GetTileSurrounding(0).name.Contains("Floor") && DungeonUtility.GetTileSurrounding(1).name.Contains("Floor") || DungeonUtility.GetTileSurrounding(0).name == "Wall" && DungeonUtility.GetTileSurrounding(1).name.Contains("Floor") || DungeonUtility.GetTileSurrounding(0).name.Contains("Floor") && DungeonUtility.GetTileSurrounding(1).name.Contains("Wall"))
                    {
                        BuildTilePiece.ChangeTilePiece(DungeonUtility.GetWallPositions()[i], 0, TileType.Floor, DungeonUtility.GetTilemap());
                    }
                }
                if (DungeonUtility.GetTileSurrounding(2) && DungeonUtility.GetTileSurrounding(3))
                {
                    if (DungeonUtility.GetTileSurrounding(2).name.Contains("Floor") && DungeonUtility.GetTileSurrounding(3).name.Contains("Floor") || DungeonUtility.GetTileSurrounding(2).name.Contains("Wall") && DungeonUtility.GetTileSurrounding(3).name.Contains("Floor") || DungeonUtility.GetTileSurrounding(2).name.Contains("Floor") && DungeonUtility.GetTileSurrounding(3).name.Contains("Wall"))
                    {
                        BuildTilePiece.ChangeTilePiece(DungeonUtility.GetWallPositions()[i], 0, TileType.Floor, DungeonUtility.GetTilemap());
                    }
                }
                if (DungeonUtility.GetTileSurrounding(0) == null || DungeonUtility.GetTileSurrounding(1) == null || DungeonUtility.GetTileSurrounding(2) == null || DungeonUtility.GetTileSurrounding(3) == null)
                {
                    BuildTilePiece.ChangeTilePiece(DungeonUtility.GetWallPositions()[i], 0, TileType.Wall, DungeonUtility.GetTilemap());
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
                    if(DungeonUtility.GetTileSurrounding(i) != null)
                    {
                        if (DungeonUtility.GetTileSurrounding(i).name.Contains("Wall"))
                        {
                            if (!DungeonUtility.GetWallForDoorsPositions().Contains(DungeonUtility.GetSurroundingPosition(i)))
                                DungeonUtility.AddWallForDoorsPositions(DungeonUtility.GetSurroundingPosition(i));
                        }
                    }
                 
                }
                DungeonUtility.ClearSurroundPositions();
            }
        }
    }
}

