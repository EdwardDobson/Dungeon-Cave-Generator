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
    public GameObject BlockDrop;
    public bool CanDig;
    PlaceTile m_pTile;
    [SerializeField]
    AudioSource m_source;
    public GameObject BreakingEffectPrefab;
    GameManager m_manager;
    FileManager m_fileManager;
    private void Start()
    {
        CurrentDigSpeed = MaxDigSpeed;
        m_pTile = GetComponent<PlaceTile>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_fileManager = GameObject.Find("Save").GetComponent<FileManager>();
    }
    void Update()
    {
        if (Time.timeScale > 0)
        {
            if(CanDig && m_manager.CanPerformAction)
            {
                FindTile();
                if (m_manager.Creative)
                    DigDamage = 1000;
                else
                    DigDamage = 1;
            }
        }
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
                        bool foundTile = false;
                        for(int WT = 0; WT < WallsTouched.Count; ++WT)
                        {
                            if(WallsTouched[WT].Pos == v)
                                foundTile = true;
                        }
                        if(!foundTile)
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
                                    if (!m_fileManager.PlacedOnTiles.ContainsKey(v))
                                    {
                                        TileManager.PlaceTile(v, 0, WallTileMap, Map, TileManager.GetTileHolder(TileType.Path).Tiles[0], DictionaryType.Floor);
                                        DataToSave tempDataFloor = new DataToSave
                                        {
                                            PosX = v.x,
                                            PosY = v.y,
                                            PosZ = v.z,
                                            ID = TileManager.GetTileHolder(TileType.Path).Tiles[0].ID,
                                            IsNull = false,
                                            IsPlacedTile = false
                                        };
                                      m_fileManager.Input(tempDataFloor);
                                    }
                                    for (int a = 0; a < m_fileManager.PlacedOnTiles.Count; ++a)
                                    {
                                        if (m_fileManager.PlacedOnTiles.ContainsKey(v))
                                        {
                                            TileManager.PlaceTile(v, 0, WallTileMap, Map, m_fileManager.PlacedOnTiles[v], DictionaryType.Floor);
                                            Debug.Log("Place Floor tile");
                                            m_fileManager.PlacedOnTiles.Remove(v);
                                        }
                                    }
                                    DataToSave tempData = new DataToSave
                                    {
                                        PosX = v.x,
                                        PosY = v.y,
                                        PosZ = v.z,
                                        ID = WallsTouched[i].ID,
                                        IsNull = true,
                                        IsPlacedTile = false
                                    };
                                  
                                    if (m_fileManager.Save.DataPacks.Any(t =>t.PosX == v.x && t.PosY == v.y && t.PosZ == v.z))
                                    {
                                     DataToSave temp =   m_fileManager.Save.DataPacks.Where(t => t.PosX == v.x && t.PosY == v.y && t.PosZ == v.z).First();
                                        if(!temp.IsNull)
                                        {
                                        m_fileManager.Save.DataPacks.Remove(temp);
                                          
                                        }
                                    }
                                    else
                                    {
                                        m_fileManager.TilesToSave.Add(tempData);
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
        int amountToDrop = Random.Range(1, WallsTouched[_blockIndex].DropMax);
        for(int i = 0; i < amountToDrop; ++i)
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
}
