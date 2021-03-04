using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonGeneration;
using System.Linq;
public class BombManager : MonoBehaviour
{
    [SerializeField]
    List<CustomTile> m_tilesAround = new List<CustomTile>();
    public PlaceTile pTile;
    public GameObject BreakingEffectPrefab;
    public List<Vector3Int> Directions = new List<Vector3Int>();
    public float BombDamage;
    private void Update()
    {
        if (transform.childCount > 0)
        {
            if (BombDamage > 0)
            {
                DirectionsCalculate();
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
                for (int b = 0; b < m_tilesAround.Count; ++b)
                {
                    if (Directions.Contains(m_tilesAround[b].Pos))
                    {
                        if (m_tilesAround[b].Health > 0)
                            m_tilesAround[b].Health -= BombDamage;
                        if (m_tilesAround[b].Health <= 0)
                        {
                      
                            Vector3 breakingPos = new Vector3(m_tilesAround[b].Pos.x + 0.5f, m_tilesAround[b].Pos.y + 0.5f, -2);
                            GameObject breakingEffectClone = Instantiate(BreakingEffectPrefab, breakingPos, Quaternion.identity);
                            ParticleSystem.MainModule breakingEffect = breakingEffectClone.GetComponent<ParticleSystem>().main;
                            breakingEffect.startColor = m_tilesAround[b].TileBreakingColour;
                            pTile.GetComponent<Scoring>().IncreaseScore(m_tilesAround[b].ScoreDispense);
                            if (TileManager.GetTileDictionaryWalls().ContainsKey(m_tilesAround[b].Pos))
                            {
                                TileManager.RemoveTilePiece(m_tilesAround[b].Pos, WallGen.GetTilemap());
                                TileManager.ChangeTilePiece(m_tilesAround[b].Pos, 0, TileType.Path, DungeonUtility.GetTilemap());
                                TileManager.GetTileDictionaryWalls().Remove(m_tilesAround[b].Pos);
                                TileManager.FillDictionary(m_tilesAround[b].Pos, TileManager.GetTileHolder(TileType.Path).Tiles[0], DungeonUtility.GetTilemap(), DictionaryType.Floor);
                            }
                            if (TileManager.GetTileDictionaryFloor().ContainsKey(m_tilesAround[b].Pos))
                            {
                                TileManager.ChangeTilePieceDig(m_tilesAround[b].Pos, TileManager.GetTileDictionaryFloor()[m_tilesAround[b].Pos].TileBase, DungeonUtility.GetTilemap());
                                TileManager.ChangeTileColour(DungeonUtility.GetTilemap(), m_tilesAround[b].Pos, TileManager.GetTileDictionaryFloor()[m_tilesAround[b].Pos].CustomTile);
                            }
                            for (int a = 0; a < TileManager.GetTileHolder(m_tilesAround[b].Type).Tiles.Count; ++a)
                            {
                                if (TileManager.GetTileHolder(m_tilesAround[b].Type).Tiles[a].ID == m_tilesAround[b].ID)
                                {
                                    m_tilesAround[b] = TileManager.GetTileHolder(m_tilesAround[b].Type).Tiles[a];
                                    pTile.gameObject.GetComponent<InventoryBackpack>().AddToStorage(m_tilesAround[b]);
                                }
                            }
                            m_tilesAround.RemoveAt(b);
                        }
                    }
                }
            }
            Directions.Clear();
        }
    }
    void DirectionsCalculate()
    {
        Vector3Int tempPosX = new Vector3Int((int)(transform.GetChild(0).position.x - 1), (int)transform.GetChild(0).position.y, 0);
        if (!Directions.Contains(tempPosX))
            Directions.Add(tempPosX);
        Vector3Int tempPosXPlus = new Vector3Int((int)(transform.GetChild(0).position.x + 1), (int)transform.GetChild(0).position.y, 0);
        if (!Directions.Contains(tempPosXPlus))
            Directions.Add(tempPosXPlus);
        Vector3Int tempPosY = new Vector3Int((int)transform.GetChild(0).position.x, (int)(transform.GetChild(0).position.y - 1), 0);
        if (!Directions.Contains(tempPosY))
            Directions.Add(tempPosY);
        Vector3Int tempPosYPlus = new Vector3Int((int)transform.GetChild(0).position.x, (int)(transform.GetChild(0).position.y + 1), 0);
        if (!Directions.Contains(tempPosYPlus))
            Directions.Add(tempPosYPlus);
        Vector3Int tempPosYXRight = new Vector3Int((int)(transform.GetChild(0).position.x + 1), (int)(transform.GetChild(0).position.y + 1), 0);
        if (!Directions.Contains(tempPosYXRight))
            Directions.Add(tempPosYXRight);
        Vector3Int tempPosYXDownLeft = new Vector3Int((int)(transform.GetChild(0).position.x - 1), (int)(transform.GetChild(0).position.y - 1), 0);
        if (!Directions.Contains(tempPosYXDownLeft))
            Directions.Add(tempPosYXDownLeft);
        Vector3Int tempPosYXDownRight = new Vector3Int((int)(transform.GetChild(0).position.x + 1), (int)(transform.GetChild(0).position.y - 1), 0);
        if (!Directions.Contains(tempPosYXDownRight))
            Directions.Add(tempPosYXDownRight);
        Vector3Int tempPosYXUpLeft = new Vector3Int((int)(transform.GetChild(0).position.x - 1), (int)(transform.GetChild(0).position.y + 1), 0);
        if (!Directions.Contains(tempPosYXUpLeft))
            Directions.Add(tempPosYXUpLeft);
    }
}
