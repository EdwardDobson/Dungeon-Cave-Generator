using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonGeneration;
using System.Linq;
public class BombManager : MonoBehaviour
{
    [SerializeField]
    List<CustomTile> m_tilesAround = new List<CustomTile>();
    Vector3Int m_hitLocation;
    RaycastHit2D m_hit;
    public float Distance;
    void HitPosFunction(Vector2 _dir, int _valueX, int _valueY)
    {
        m_hit = Physics2D.Raycast(transform.GetChild(0).position, _dir, Distance);
        Debug.DrawRay(transform.position, _dir, Color.red);
        if (m_hit.collider != null)
        {
            if (m_hit.collider.tag == "Wall")
            {
                m_hitLocation = new Vector3Int((int)m_hit.point.x - _valueX, (int)m_hit.point.y - _valueY, 0);
                // Debug.Log("Hit " + m_hit.collider.name + " @ " + m_hitLocation);
            }
        }
    }
    private void Update()
    {
        if(transform.childCount > 0)
        {
            HitPosFunction(Vector2.right, 0, 0);
            HitPosFunction(Vector2.up, 0, 0);
            HitPosFunction(Vector2.down, 0, 1);
            HitPosFunction(Vector2.left, 1, 0);
            if (WallGen.GetTilemap().GetTile(m_hitLocation) != null)
            {
                if (m_tilesAround.All(w => w.Pos != m_hitLocation))
                {
                    CustomTile copy = Instantiate(TileManager.GetTileDictionary()[m_hitLocation].CustomTile);
                    copy.Pos = m_hitLocation;
                    m_tilesAround.Add(copy);
                }

                for (int i = 0; i < m_tilesAround.Count; ++i)
                {
                    if (m_tilesAround[i].Pos == m_hitLocation)
                    {
                        m_tilesAround[i].Health--;
                
                        if (m_tilesAround[i].Health <= 0)
                        {
                            TileManager.RemoveTilePiece(m_hitLocation, WallGen.GetTilemap());
                            TileManager.ChangeTilePiece(m_hitLocation, 0, TileType.Path, DungeonUtility.GetTilemap());
                            TileManager.GetTileDictionary().Remove(m_hitLocation);
                            TileManager.FillDictionary(m_hitLocation, TileManager.GetAllTiles(TileType.Path), 0, DungeonUtility.GetTilemap());
                            m_tilesAround.RemoveAt(i);
                        }
                    }
                }  
            }
        }
    }
}
