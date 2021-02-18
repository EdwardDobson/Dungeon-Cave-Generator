using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum TileType
{
    Wall,
    Floor,
    Path,
    Door

}
[CreateAssetMenu(fileName = "Custom Tile", menuName = "Create Custom Tile", order = 1)]
public class CustomTile : ScriptableObject
{
    public TileBase Tile;
    public TileType Type;
    public float Speed;
    public float Health;
    public Vector3Int Pos;
}
