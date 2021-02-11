using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonGeneration
{
    public class BuildDoor
    {
        public static void PlaceDoor()
        {
            int randomPosChoice = Random.Range(0, DungeonUtility.GetWallPositions().Count);
            Vector3Int randomWallPos = DungeonUtility.GetWallPositions()[randomPosChoice];
            DungeonUtility.GetTilemap().SetTile(randomWallPos, DungeonUtility.GetTiles()[2]);
            DungeonUtility.AddDoorPosition(randomWallPos);
            DungeonUtility.GetWallPositions().Remove(randomWallPos);
        }
        public static void RemoveDoors()
        {
            for(int i = 0; i < DungeonUtility.GetDoorPositions().Count; ++i)
            {
                Vector3Int x1 = new Vector3Int(DungeonUtility.GetDoorPositions()[i].x + 1, DungeonUtility.GetDoorPositions()[i].y, 0);
                Vector3Int x2 = new Vector3Int(DungeonUtility.GetDoorPositions()[i].x - 1, DungeonUtility.GetDoorPositions()[i].y, 0);
                Vector3Int y1 = new Vector3Int(DungeonUtility.GetDoorPositions()[i].x, DungeonUtility.GetDoorPositions()[i].y + 1, 0);
                Vector3Int y2 = new Vector3Int(DungeonUtility.GetDoorPositions()[i].x, DungeonUtility.GetDoorPositions()[i].y - 1, 0);
          if(DungeonUtility.GetTilemap().GetTile(x1) && DungeonUtility.GetTilemap().GetTile(x2) && DungeonUtility.GetTilemap().GetTile(y1) && DungeonUtility.GetTilemap().GetTile(y2))
                {
                    if (DungeonUtility.GetTilemap().GetTile(x1).name == "Floor" && DungeonUtility.GetTilemap().GetTile(x2).name == "Floor" && DungeonUtility.GetTilemap().GetTile(y1).name == "Floor" && DungeonUtility.GetTilemap().GetTile(y2).name == "Floor")
                    {
                        //  Debug.Log("Tiles at x Positions");
                        DungeonUtility.GetTilemap().SetTile(DungeonUtility.GetDoorPositions()[i], DungeonUtility.GetTiles()[1]);
                    }
                }
            }
        }
    }
}

