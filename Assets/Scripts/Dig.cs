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
    public float MaxDigSpeed;
    public float CurrentDigSpeed;
    public List<CustomTile> WallsTouched = new List<CustomTile>();
    PlaceTile m_pTile;
    [SerializeField]
    AudioSource m_source;
    private void Start()
    {
        CurrentDigSpeed = MaxDigSpeed;
        m_pTile = GetComponent<PlaceTile>();

    }
    void Update()
    {
        if (Time.timeScale > 0)
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
                if (Input.GetMouseButton(0))
                {
                    CurrentDigSpeed -= Time.deltaTime;
                    if (CurrentDigSpeed <= 0)
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
                                    WallsTouched[i].Health -= DigDamage;
                                    if(WallsTouched[i].BlockSound != null)
                                    {
                                        m_source.clip = WallsTouched[i].BlockSound;
                                        m_source.Play();
                                    }                        
                                }
                                if (WallsTouched[i].Health <= 0)
                                {
                                    if (!m_pTile.PlacedOnTiles.ContainsKey(v))
                                    {
                                        TileManager.RemoveTilePiece(v, WallTileMap);
                                        TileManager.ChangeTilePiece(v, 0, TileType.Path, Map);
                                        TileManager.GetTileDictionary().Remove(v);
                                        TileManager.FillDictionary(v, TileManager.GetTileHolder(TileType.Path).Tiles[0], Map);
                                    }
                                    //   TileManager.FillDictionary(v, TileManager.GetAllTiles(TileType.Path), 0, Map);
                                    for (int a = 0; a < m_pTile.PlacedOnTiles.Count; ++a)
                                    {
                                        if (m_pTile.PlacedOnTiles.ContainsKey(v))
                                        {
                                            TileManager.RemoveTilePiece(v, WallTileMap);
                                            TileManager.ChangeTilePieceDig(v, m_pTile.PlacedOnTiles[v].Tile[0], Map);
                                            TileManager.GetTileDictionary().Remove(v);
                                            TileManager.FillDictionary(v, m_pTile.PlacedOnTiles[v], Map);
                                            TileManager.ChangeTileColour(Map, v, m_pTile.PlacedOnTiles[v]);
                                            m_pTile.PlacedOnTiles.Remove(v);
                                        }
                                    }
                                
                                    WallsTouched.RemoveAt(i);
                                }
                            }
                        }
                        CurrentDigSpeed = MaxDigSpeed;
                    }
                }
            }
        }
    }
}
