using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile Holder", menuName = "Tile Holder", order = 1)]
public class TileHolder : ScriptableObject
{
    public List<CustomTile> Tiles = new List<CustomTile>();

}
