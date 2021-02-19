using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonGeneration
{
    public class BuildDoor
    {
        public static void PlaceDoor(int _index)
        {
            int randomPosChoice = Random.Range(0, RoomManager.GetAllRooms()[_index].WallPositions.Count);
            Vector3Int randomWallPos = RoomManager.GetAllRooms()[_index].WallPositions[randomPosChoice];
            DungeonUtility.AddDoorPosition(randomWallPos);
            RoomManager.GetAllRooms()[_index].DoorPositions.Add(randomWallPos);
            TileManager.ChangeTilePiece(randomWallPos, 0, TileType.Door, DungeonUtility.GetTilemap()) ;
       
         //   RoomManager.GetAllRooms()[_index].WallPositions.RemoveAt(randomPosChoice);
        }
        public static void RemoveDoors(int _index, int _indexWallPos)
        {
            if (!RoomManager.GetAllRooms()[_index].HasDoor)
            {
                Vector3Int x1 = new Vector3Int(RoomManager.GetAllRooms()[_index].WallPositions[_indexWallPos].x + 1, RoomManager.GetAllRooms()[_index].WallPositions[_indexWallPos].y, 0);
                Vector3Int x2 = new Vector3Int(RoomManager.GetAllRooms()[_index].WallPositions[_indexWallPos].x - 1, RoomManager.GetAllRooms()[_index].WallPositions[_indexWallPos].y, 0);
                Vector3Int y1 = new Vector3Int(RoomManager.GetAllRooms()[_index].WallPositions[_indexWallPos].x, RoomManager.GetAllRooms()[_index].WallPositions[_indexWallPos].y + 1, 0);
                Vector3Int y2 = new Vector3Int(RoomManager.GetAllRooms()[_index].WallPositions[_indexWallPos].x, RoomManager.GetAllRooms()[_index].WallPositions[_indexWallPos].y - 1, 0);
                RoomManager.TileCheckEmpty(x1, y1, _index, _indexWallPos);
                RoomManager.TileCheckEmpty(x2, y2, _index, _indexWallPos);
                RoomManager.TileCheckEmpty(x1, y2, _index, _indexWallPos);
                RoomManager.TileCheckEmpty(x2, y1, _index, _indexWallPos);
                RoomManager.TileCheckNotEmpty(x1, x2, y1, y2, _index, _indexWallPos, "Floor");
            }
        }

        public static void CheckForDoor()
        {
            for (int i = 0; i < RoomManager.GetAllRooms().Count; ++i)
            {
                //   Debug.Log("Room Wall positions: " + DungeonUtility.GetAllRooms()[i].WallPositions.Count);
                for (int w = 0; w < RoomManager.GetAllRooms()[i].WallPositions.Count; ++w)
                {
                    RemoveDoors(i, w);
                }
            }
        }
    }
}

