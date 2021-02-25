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
public enum Attributes
{
    Health,
    Damage,
    Speed
}
[CreateAssetMenu(fileName = "Custom Tile", menuName = "Create Custom Tile", order = 1)]
public class CustomTile : ScriptableObject
{
    public List< TileBase> Tile;
    public TileType Type;
    public Attributes[] Attributes;
    public Color TileColour;
    public float Speed;
    public float Health;
    public float Damage;
    public float CurrentAttackCoolDown;
    public float MaxAttackCoolDown;
    public Vector3Int Pos;
    [Range(1,100)]
    public float PickChance;
}
