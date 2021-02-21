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
    Vector3Int m_hitLocation;
    RaycastHit2D m_hit;
    public List<CustomTile> WallsTouched = new List<CustomTile>();
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            HitPosFunction(Vector2.right, 0, 0);
        if (Input.GetKey(KeyCode.W))
            HitPosFunction(Vector2.up, 0, 0);
        if (Input.GetKey(KeyCode.S))
            HitPosFunction(Vector2.down, 0, 1);
        if (Input.GetKey(KeyCode.A))
            HitPosFunction(Vector2.left, 1, 0);

        FindTile();
    }
    void HitPosFunction(Vector2 _dir, int _valueX, int _valueY)
    {
        m_hit = Physics2D.Raycast(transform.position, _dir, Distance);
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
    void FindTile()
    {
        if (WallTileMap.GetTile(m_hitLocation) != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (WallsTouched.All(w => w.Pos != m_hitLocation))
                {
                    CustomTile copy = Instantiate(TileManager.GetTileDictionary()[m_hitLocation].CustomTile);
                    copy.Pos = m_hitLocation;
                    WallsTouched.Add(copy);
                }
                for (int i = 0; i < WallsTouched.Count; ++i)
                {
                    if (WallsTouched[i].Pos == m_hitLocation)
                    {
                        if (WallsTouched[i].Health > 0)
                        {
                            WallsTouched[i].Health--;
                        }
                        if (WallsTouched[i].Health <= 0)
                        {
                            
                            TileManager.RemoveTilePiece(m_hitLocation, WallTileMap);
                            TileManager.ChangeTilePiece(m_hitLocation, 0, TileType.Path, Map);
                            TileManager.GetTileDictionary().Remove(m_hitLocation);
                            TileManager.FillDictionary(m_hitLocation, TileManager.GetAllTiles(TileType.Path), 0, Map);
                            WallsTouched.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
