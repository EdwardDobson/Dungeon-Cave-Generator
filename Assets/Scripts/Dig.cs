using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dig : MonoBehaviour
{
    public float Distance;
    public Tilemap WallTileMap;
    public Tilemap Map;
    public float MaxRange;
    public int DigDamage;
    public List<CustomTile> WallsTouched = new List<CustomTile>();
    void Update()
    {
        FindTile();
    }

    void FindTile()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int v = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0);
        float distance = Vector3Int.Distance(v, new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z));
        if (distance <= MaxRange)
        {
            if (WallTileMap.GetTile(v) != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (WallsTouched.All(w => w.Pos != v))
                    {
                        CustomTile copy = Instantiate(TileManager.GetTileDictionary()[v].CustomTile);
                        copy.Pos = v;
                        WallsTouched.Add(copy);
                    }
                    for (int i = 0; i < WallsTouched.Count; ++i)
                    {
                        if (WallsTouched[i].Pos == v)
                        {
                            if (WallsTouched[i].Health > 0)
                            {
                                WallsTouched[i].Health-= DigDamage;
                            }
                            if (WallsTouched[i].Health <= 0)
                            {

                                TileManager.RemoveTilePiece(v, WallTileMap);
                                TileManager.ChangeTilePiece(v, 0, TileType.Path, Map);
                                TileManager.GetTileDictionary().Remove(v);
                                TileManager.FillDictionary(v, TileManager.GetAllTiles(TileType.Path), 0, Map);
                                WallsTouched.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
    }
}
