using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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
    public GameObject BlockDrop;
    PlaceTile m_pTile;
    [SerializeField]
    AudioSource m_source;
    public GameObject BreakingEffectPrefab;
    GameManager m_manager;

    private void Start()
    {
        CurrentDigSpeed = MaxDigSpeed;
        m_pTile = GetComponent<PlaceTile>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (Time.timeScale > 0)
            FindTile();
        if (m_manager.Creative)
            DigDamage = 1000;
        else
            DigDamage = 1;
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
                            CustomTile copy = Instantiate(TileManager.GetTileDictionaryWalls()[v].CustomTile);
                            copy.Pos = v;
                            WallsTouched.Add(copy);
                        }
                        for (int i = 0; i < WallsTouched.Count; ++i)
                        {
                            if (WallsTouched[i].Pos == v)
                            {
                                for (int dT = 0; dT < m_manager.DamagedTiles.DamagedTilesList.Count; ++dT)
                                {
                                    if (m_manager.DamagedTiles.DamagedTilesList[dT].Pos == WallsTouched[i].Pos)
                                    {
                                        WallsTouched[i].Health = m_manager.DamagedTiles.DamagedTilesList[dT].Health;
                                    }
                                }
                                if (WallsTouched[i].Health > 0)
                                {
                                    WallsTouched[i].Health -= DigDamage;

                                    m_manager.DamagedTiles.Add(WallsTouched[i]);
                                    if (WallsTouched[i].BlockSound != null)
                                    {
                                        m_source.clip = WallsTouched[i].BlockSound;
                                        m_source.Play();
                                    }
                                }
                                if (WallsTouched[i].Health <= 0)
                                {
                                    if (!m_manager.Creative)
                                    {
                                        for (int a = 0; a < TileManager.GetTileHolder(WallsTouched[i].Type).Tiles.Count; ++a)
                                        {
                                            if (TileManager.GetTileHolder(WallsTouched[i].Type).Tiles[a].ID == WallsTouched[i].ID)
                                            {
                                                ShouldBlockDrop(i);
                                                    WallsTouched[i] = TileManager.GetTileHolder(WallsTouched[i].Type).Tiles[a];
                                            }
                                        }
                                    }
                                    Vector3 breakingPos = new Vector3(v.x + 0.5f, v.y + 0.5f, -2);
                                    GameObject breakingEffectClone = Instantiate(BreakingEffectPrefab, breakingPos, Quaternion.identity);
                                    ParticleSystem.MainModule breakingEffect = breakingEffectClone.GetComponent<ParticleSystem>().main;
                                    breakingEffect.startColor = WallsTouched[i].TileBreakingColour;
                                    if (WallsTouched[i].ShouldGiveScore)
                                    {
                                        WallsTouched[i].ScoreDispense = Random.Range(WallsTouched[i].ScoreDispenseMin, WallsTouched[i].ScoreDispenseMax);
                                        WallsTouched[i].ScoreDispense = (int)WallsTouched[i].ScoreDispense;
                                        GetComponent<Scoring>().IncreaseScore(WallsTouched[i].ScoreDispense);
                                    }
                                    if (!m_pTile.PlacedOnTiles.ContainsKey(v))
                                    {
                                        TileManager.RemoveTilePiece(v, WallTileMap);
                                        TileManager.ChangeTilePiece(v, 0, TileType.Path, Map);
                                        TileManager.GetTileDictionaryWalls().Remove(v);
                                        TileManager.FillDictionary(v, TileManager.GetTileHolder(TileType.Path).Tiles[0], Map, DictionaryType.Floor);
                                        TileManager.ChangeTileColour(Map, v, TileManager.GetTileHolder(TileType.Path).Tiles[0]);
                                    }
                                    //   TileManager.FillDictionary(v, TileManager.GetAllTiles(TileType.Path), 0, Map);
                                    for (int a = 0; a < m_pTile.PlacedOnTiles.Count; ++a)
                                    {
                                        if (m_pTile.PlacedOnTiles.ContainsKey(v))
                                        {
                                            TileManager.RemoveTilePiece(v, WallTileMap);
                                            TileManager.ChangeTilePieceDig(v, m_pTile.PlacedOnTiles[v].Tile[0], Map);
                                            TileManager.GetTileDictionaryWalls().Remove(v);
                                            TileManager.FillDictionary(v, m_pTile.PlacedOnTiles[v], Map, DictionaryType.Floor);
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
    void ShouldBlockDrop(int _blockIndex)
    {
        if (!m_manager.Creative && !m_manager.ScoreMode)
        {
            Vector3 pos = new Vector3(WallsTouched[_blockIndex].Pos.x + 0.5f, WallsTouched[_blockIndex].Pos.y + 0.5f, 0);
            GameObject c = Instantiate(BlockDrop, pos, Quaternion.identity);
            c.GetComponent<BlockDrop>().SetUp(WallsTouched[_blockIndex]);
        }
        if (m_manager.ScoreMode && !WallsTouched[_blockIndex].ShouldGiveScore)
        {
            Vector3 pos = new Vector3(WallsTouched[_blockIndex].Pos.x + 0.5f, WallsTouched[_blockIndex].Pos.y + 0.5f, 0);
            GameObject c = Instantiate(BlockDrop, pos, Quaternion.identity);
            c.GetComponent<BlockDrop>().SetUp(WallsTouched[_blockIndex]);
        }
    }
}
