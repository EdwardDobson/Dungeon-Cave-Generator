using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacedBomb : MonoBehaviour
{
    public List<Vector3Int> Directions = new List<Vector3Int>();
    [SerializeField]
    List<CustomTile> m_tilesAround = new List<CustomTile>();
    public GameObject BreakingEffectPrefab;
    public float BombDamage;
    public GameObject BlockDrop;
    AudioSource m_source;
    GameManager m_manager;
    FileManager m_fileManager;
    void Start()
    {
        m_source = GameObject.Find("Player").GetComponent<AudioSource>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_fileManager = GameObject.Find("SaveHolder").GetComponent<FileManager>();
        DirectionsCalculate();
        GetSurroundingTiles();
        DamageTiles();
        Destroy(gameObject, 0.5f);
    }
    void DamageTiles()
    {
        for (int i = 0; i < m_tilesAround.Count; ++i)
        {
            if (Directions.Contains(m_tilesAround[i].Pos))
            {
                for (int dT = 0; dT < m_manager.DamagedTiles.DamagedTilesList.Count; ++dT)
                {
                    if (m_manager.DamagedTiles.DamagedTilesList[dT].Pos == m_tilesAround[i].Pos)
                    {
                        m_tilesAround[i].Health = m_manager.DamagedTiles.DamagedTilesList[dT].Health;
                    }
                }
                if (m_tilesAround[i].Health > 0)
                {
                    m_tilesAround[i].Health -= BombDamage;
                    m_manager.DamagedTiles.Add(m_tilesAround[i]);
                    if (m_tilesAround[i].BlockSound != null && WallGen.GetTilemap().GetTile(m_tilesAround[i].Pos) != null)
                    {
                        m_source.clip = m_tilesAround[i].BlockSound;
                        m_source.Play();
                    }
                }
                RemoveDeadTiles(i);
            }
        }
    }
    void PlayBreakingEffect(int _index)
    {
        Vector3 breakingPos = new Vector3(m_tilesAround[_index].Pos.x + 0.5f, m_tilesAround[_index].Pos.y + 0.5f, -2);
        GameObject breakingEffectClone = Instantiate(BreakingEffectPrefab, breakingPos, Quaternion.identity);
        ParticleSystem.MainModule breakingEffect = breakingEffectClone.GetComponent<ParticleSystem>().main;
        breakingEffect.startColor = m_tilesAround[_index].TileBreakingColour;
    }
    void RemoveDeadTiles(int _index)
    {
        if (m_tilesAround[_index].Health <= 0)
        {
            DataToSave tempData = new DataToSave
            {
                Position = new Vector2Int(m_tilesAround[_index].Pos.x, m_tilesAround[_index].Pos.y),
                ID = m_tilesAround[_index].ID,
                IsNull = true,
                IsPlacedTile = false
            };
            if (TileManager.GetTileDictionaryWalls().ContainsKey(m_tilesAround[_index].Pos))
            {
                if (WallGen.GetTilemap().GetTile(m_tilesAround[_index].Pos) != null)
                {
                    if (TileManager.GetTileDictionaryFloor().ContainsKey(m_tilesAround[_index].Pos))
                    {
                        TileManager.PlaceTile(m_tilesAround[_index].Pos, 0, WallGen.GetTilemap(), DungeonUtility.GetTilemap(), TileManager.GetTileDictionaryFloor()[m_tilesAround[_index].Pos].CustomTile, DictionaryType.Floor);
                        PlayBreakingEffect(_index);
                        AddToStorage(_index);
                        tempData.IsNull = true;
                        m_fileManager.Input(tempData);
                    }
                    else
                    {
                        TileManager.PlaceTile(m_tilesAround[_index].Pos, 0, WallGen.GetTilemap(), DungeonUtility.GetTilemap(), TileManager.GetTileHolder(TileType.Path).Tiles[0], DictionaryType.Floor);
                        PlayBreakingEffect(_index);
                        AddToStorage(_index);
                        tempData.ID = TileManager.GetTileHolder(TileType.Path).Tiles[0].ID;
                        tempData.IsNull = false;
                        tempData.IsPlacedTile = true;
                        m_fileManager.Input(tempData);
                    }
                }
            }
        }
    }
    void AddToStorage(int _index)
    {
        for (int a = 0; a < TileManager.GetTileHolder(m_tilesAround[_index].Type).Tiles.Count; ++a)
        {
            if (TileManager.GetTileHolder(m_tilesAround[_index].Type).Tiles[a].ID == m_tilesAround[_index].ID)
            {
                ShouldBlockDrop(_index);
                m_tilesAround[_index] = TileManager.GetTileHolder(m_tilesAround[_index].Type).Tiles[a];
            }
        }
        if (m_tilesAround[_index].ShouldGiveScore)
        {
            m_tilesAround[_index].ScoreDispense = Random.Range(m_tilesAround[_index].ScoreDispenseMin, m_tilesAround[_index].ScoreDispenseMax);
            m_tilesAround[_index].ScoreDispense = (int)m_tilesAround[_index].ScoreDispense;
            m_source.GetComponent<Scoring>().IncreaseScore(m_tilesAround[_index].ScoreDispense);
        }
    }
    void GetSurroundingTiles()
    {
        for (int i = 0; i < Directions.Count; ++i)
        {
            if (TileManager.GetTileDictionaryWalls().ContainsKey(Directions[i]))
            {
                if (TileManager.GetTileDictionaryWalls()[Directions[i]].CustomTile != null)
                {
                    CustomTile copy = Instantiate(TileManager.GetTileDictionaryWalls()[Directions[i]].CustomTile);
                    copy.Pos = Directions[i];
                    if (!m_tilesAround.Contains(copy))
                        m_tilesAround.Add(copy);
                }
            }
        }
    }
    void DirectionsCalculate()
    {
        Vector3Int tempPosX = new Vector3Int((int)(transform.position.x - 1), (int)transform.position.y, 0);
        if (!Directions.Contains(tempPosX))
            Directions.Add(tempPosX);
        Vector3Int tempPosXPlus = new Vector3Int((int)(transform.position.x + 1), (int)transform.position.y, 0);
        if (!Directions.Contains(tempPosXPlus))
            Directions.Add(tempPosXPlus);
        Vector3Int tempPosY = new Vector3Int((int)transform.position.x, (int)(transform.position.y - 1), 0);
        if (!Directions.Contains(tempPosY))
            Directions.Add(tempPosY);
        Vector3Int tempPosYPlus = new Vector3Int((int)transform.position.x, (int)(transform.position.y + 1), 0);
        if (!Directions.Contains(tempPosYPlus))
            Directions.Add(tempPosYPlus);
        Vector3Int tempPosYXRight = new Vector3Int((int)(transform.position.x + 1), (int)(transform.position.y + 1), 0);
        if (!Directions.Contains(tempPosYXRight))
            Directions.Add(tempPosYXRight);
        Vector3Int tempPosYXDownLeft = new Vector3Int((int)(transform.position.x - 1), (int)(transform.position.y - 1), 0);
        if (!Directions.Contains(tempPosYXDownLeft))
            Directions.Add(tempPosYXDownLeft);
        Vector3Int tempPosYXDownRight = new Vector3Int((int)(transform.position.x + 1), (int)(transform.position.y - 1), 0);
        if (!Directions.Contains(tempPosYXDownRight))
            Directions.Add(tempPosYXDownRight);
        Vector3Int tempPosYXUpLeft = new Vector3Int((int)(transform.position.x - 1), (int)(transform.position.y + 1), 0);
        if (!Directions.Contains(tempPosYXUpLeft))
            Directions.Add(tempPosYXUpLeft);
    }

    void ShouldBlockDrop(int _blockIndex)
    {
        if (!m_manager.Creative && !m_manager.ScoreMode)
        {
            Vector3 pos = new Vector3(m_tilesAround[_blockIndex].Pos.x + 0.5f, m_tilesAround[_blockIndex].Pos.y + 0.5f, 0);
            GameObject c = Instantiate(BlockDrop, pos, Quaternion.identity);
            c.GetComponent<BlockDrop>().SetUp(m_tilesAround[_blockIndex]);
        }
        if (m_manager.ScoreMode && !m_tilesAround[_blockIndex].ShouldGiveScore)
        {
            Vector3 pos = new Vector3(m_tilesAround[_blockIndex].Pos.x + 0.5f, m_tilesAround[_blockIndex].Pos.y + 0.5f, 0);
            GameObject c = Instantiate(BlockDrop, pos, Quaternion.identity);
            c.GetComponent<BlockDrop>().SetUp(m_tilesAround[_blockIndex]);
        }
    }
}
