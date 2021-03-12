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
    public Item Item;
    public List< TileBase> Tile;
    public Sprite[] SpriteVariations;
    public string TileName;
    public TileType Type;
    public Attributes[] Attributes;
    public AudioClip BlockSound;
    public bool ShouldUseColour;
    public Color TileColour;
    public Color TileBreakingColour;
    public Sprite DisplaySprite;
    public float Speed;
    public float Health;
    public float Damage;
    public float CurrentAttackCoolDown;
    public float MaxAttackCoolDown;
    public bool ShouldGiveScore;
    public float ScoreDispenseMin;
    public float ScoreDispenseMax;
    public float ScoreDispense;
    public int ID;
    public Vector3Int Pos;
    [Range(1,100)]
    public float PickChance;
    public int DropMax;
}
