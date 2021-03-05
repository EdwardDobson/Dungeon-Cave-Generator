using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedBomb : MonoBehaviour
{
    public List<Vector3Int> Directions = new List<Vector3Int>();
    [SerializeField]
    List<CustomTile> m_tilesAround = new List<CustomTile>();
    public GameObject BreakingEffectPrefab;
    public float BombDamage;
    InventoryBackpack m_inventoryBackpack;
    public GameObject BlockDrop;
    AudioSource m_source;
    GameManager m_manager;
    void Start()
    {
        m_inventoryBackpack = GameObject.Find("Player").GetComponent<InventoryBackpack>();
        m_source = GameObject.Find("Player").GetComponent<AudioSource>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        DirectionsCalculate();
        GetSurroundingTiles();
        DamageTiles();
        Destroy(gameObject,0.5f);
    }
    void DamageTiles()
    {
        for (int i = 0; i < m_tilesAround.Count; ++i)
        {
            if (Directions.Contains(m_tilesAround[i].Pos))
            {
                if (m_tilesAround[i].Health > 0)
                {
                    m_tilesAround[i].Health -= BombDamage;
                    if (m_tilesAround[i].BlockSound != null)
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
            if (TileManager.GetTileDictionaryFloor().ContainsKey(m_tilesAround[_index].Pos))
            {
                TileManager.RemoveTilePiece(m_tilesAround[_index].Pos, WallGen.GetTilemap());
                TileManager.GetTileDictionaryWalls().Remove(m_tilesAround[_index].Pos);
                TileManager.ChangeTilePieceDig(m_tilesAround[_index].Pos, TileManager.GetTileDictionaryFloor()[m_tilesAround[_index].Pos].TileBase, DungeonUtility.GetTilemap());
                TileManager.ChangeTileColour(DungeonUtility.GetTilemap(), m_tilesAround[_index].Pos, TileManager.GetTileDictionaryFloor()[m_tilesAround[_index].Pos].CustomTile);
                TileManager.FillDictionary(m_tilesAround[_index].Pos, m_tilesAround[_index], DungeonUtility.GetTilemap(), DictionaryType.Floor);
                PlayBreakingEffect(_index);
                AddToStorage(_index);
            }
            if (TileManager.GetTileDictionaryWalls().ContainsKey(m_tilesAround[_index].Pos))
            {
                TileManager.RemoveTilePiece(m_tilesAround[_index].Pos, WallGen.GetTilemap());
                TileManager.ChangeTilePiece(m_tilesAround[_index].Pos, 0, TileType.Path, DungeonUtility.GetTilemap());
                TileManager.GetTileDictionaryWalls().Remove(m_tilesAround[_index].Pos);
                TileManager.FillDictionary(m_tilesAround[_index].Pos, TileManager.GetTileHolder(TileType.Path).Tiles[0], DungeonUtility.GetTilemap(), DictionaryType.Floor);
                PlayBreakingEffect(_index);
                AddToStorage(_index);
            }
        }
    }
    void AddToStorage(int _index)
    {
        for (int a = 0; a < TileManager.GetTileHolder(m_tilesAround[_index].Type).Tiles.Count; ++a)
        {
            if (TileManager.GetTileHolder(m_tilesAround[_index].Type).Tiles[a].ID == m_tilesAround[_index].ID)
            {
                if(!m_manager.Creative)
                {
                    Vector3 pos = new Vector3(m_tilesAround[_index].Pos.x + 0.5f, m_tilesAround[_index].Pos.y + 0.5f, 0);
                    GameObject c = Instantiate(BlockDrop, pos, Quaternion.identity);
                    c.GetComponent<BlockDrop>().SetUp(m_tilesAround[_index]);
                }
                m_tilesAround[_index] = TileManager.GetTileHolder(m_tilesAround[_index].Type).Tiles[a];
            }
        }
    }
    void GetSurroundingTiles()
    {
        for (int i = 0; i < Directions.Count; ++i)
        {
            if (TileManager.GetTileDictionaryWalls().ContainsKey(Directions[i]))
            {
                CustomTile copy = Instantiate(TileManager.GetTileDictionaryWalls()[Directions[i]].CustomTile);
                copy.Pos = Directions[i];
                if (!m_tilesAround.Contains(copy))
                    m_tilesAround.Add(copy);
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
}
