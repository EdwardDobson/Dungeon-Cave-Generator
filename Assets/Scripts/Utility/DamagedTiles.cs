using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamagedTiles : MonoBehaviour
{
    public List<CustomTile> DamagedTilesList = new List<CustomTile>();

    public void Add(CustomTile _tile)
    {
        for (int i = 0; i < DamagedTilesList.Count; ++i)
        {
            if (DamagedTilesList[i].Pos == _tile.Pos)
            {
                DamagedTilesList.Remove(DamagedTilesList[i]);
                DamagedTilesList.Add(_tile);
            }
        }
        if (DamagedTilesList.All(t => t.Pos != _tile.Pos))
        {
            DamagedTilesList.Add(_tile);
        }
    }
    public void RemoveDamagedTiles()
    {
        for (int i = 0; i < DamagedTilesList.Count; ++i)
        {
            if (DamagedTilesList[i].Health <= 0)
            {
                DamagedTilesList.RemoveAt(i);
            }
        }
    }
}

