using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DungeonGeneration
{
    public class ConnectRoom
    {
        public static void FindClosestDoor()
        {
            DungeonUtility.OrderDoorPositions();
            for (int i =0; i < DungeonUtility.GetDoorPositions().Count; ++i)
            {
                Vector3Int startPoint = DungeonUtility.GetDoorPositions()[i];
                if(i < DungeonUtility.GetDoorPositions().Count-1)
                {
                    Vector3Int endPoint = DungeonUtility.GetDoorPositions()[i + 1];
                }
         
            }
        }
    }   
}

