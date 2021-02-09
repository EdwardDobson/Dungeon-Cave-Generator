using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonGeneration
{
    public class BuildDoor
    {
        public static void PlaceDoor()
        {
            for(int i = 0; i < DungeonUtility.GetDoorAmount(); ++i)
            {
                int randomPosChoice = Random.Range(0, DungeonUtility.GetWallPositions().Count);
                Vector3Int randomWallPos = DungeonUtility.GetWallPositions()[randomPosChoice];
                DungeonUtility.GetTilemap().SetTile(randomWallPos, DungeonUtility.GetTiles()[2]);
                DungeonUtility.AddDoorPosition(randomWallPos);
            }
        }
    }
}

