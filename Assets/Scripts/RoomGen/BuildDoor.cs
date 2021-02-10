using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonGeneration
{
    public class BuildDoor
    {
        public static void PlaceDoor(int _index)
        {
            int randomPosChoice = Random.Range(0, DungeonUtility.GetAllRooms()[_index].WallPositions.Count);
            Vector3Int randomWallPos = DungeonUtility.GetAllRooms()[_index].WallPositions[randomPosChoice];
            DungeonUtility.AddDoorPosition(randomWallPos);
            DungeonUtility.GetAllRooms()[_index].DoorPositions.Add(randomWallPos);
            DungeonUtility.GetTilemap().SetTile(randomWallPos, DungeonUtility.GetTiles()[2]);
            DungeonUtility.GetAllRooms()[_index].WallPositions.RemoveAt(randomPosChoice);
        }
        public static void RemoveDoors(int _index, int _indexWallPos)
        {
            if (!DungeonUtility.GetAllRooms()[_index].HasDoor)
            {
                Vector3Int x1 = new Vector3Int(DungeonUtility.GetAllRooms()[_index].WallPositions[_indexWallPos].x + 1, DungeonUtility.GetAllRooms()[_index].WallPositions[_indexWallPos].y, 0);
                Vector3Int x2 = new Vector3Int(DungeonUtility.GetAllRooms()[_index].WallPositions[_indexWallPos].x - 1, DungeonUtility.GetAllRooms()[_index].WallPositions[_indexWallPos].y, 0);
                Vector3Int y1 = new Vector3Int(DungeonUtility.GetAllRooms()[_index].WallPositions[_indexWallPos].x, DungeonUtility.GetAllRooms()[_index].WallPositions[_indexWallPos].y + 1, 0);
                Vector3Int y2 = new Vector3Int(DungeonUtility.GetAllRooms()[_index].WallPositions[_indexWallPos].x, DungeonUtility.GetAllRooms()[_index].WallPositions[_indexWallPos].y - 1, 0);
                DungeonUtility.TileCheckEmpty(x1, y1, _index, _indexWallPos);
                DungeonUtility.TileCheckEmpty(x2, y2, _index, _indexWallPos);
                DungeonUtility.TileCheckEmpty(x1, y2, _index, _indexWallPos);
                DungeonUtility.TileCheckEmpty(x2, y1, _index, _indexWallPos);
                DungeonUtility.TileCheckNotEmpty(x1, x2, y1, y2, _index, _indexWallPos, "Floor");
            }
        }

        public static void CheckForDoor()
        {
            for (int i = 0; i < DungeonUtility.GetAllRooms().Count; ++i)
            {
                //   Debug.Log("Room Wall positions: " + DungeonUtility.GetAllRooms()[i].WallPositions.Count);
                for (int w = 0; w < DungeonUtility.GetAllRooms()[i].WallPositions.Count; ++w)
                {
                    RemoveDoors(i, w);
                }
            }
        }
    }
}

