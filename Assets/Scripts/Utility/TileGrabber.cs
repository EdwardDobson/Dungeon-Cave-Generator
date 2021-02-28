using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
/// <summary>
/// Grabs the info from a tile
/// </summary>
public class TileGrabber : MonoBehaviour
{
    public bool ShouldDetect;


    public string TileName;
    public string TileType;
    public float MaxTileHealth;
    public float TileSpeed;
    public float TileDamage;
    public float TileScore;
    public void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            if (ShouldDetect)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int mousePosAdjusted = new Vector3Int((int)mousePos.x, (int)mousePos.y, 0);
                if (TileManager.GetTileDictionary().ContainsKey(mousePosAdjusted))
                {
                    CustomTile cT = TileManager.GetTileDictionary()[mousePosAdjusted].CustomTile;
                    TileName = cT.name;
                    TileType = cT.Type.ToString();
                    MaxTileHealth = cT.Health;
                    TileSpeed = cT.Speed;
                    TileDamage = cT.Damage;
                    TileScore = cT.ScoreDispense;
                }
            }
        }
    }
}
