using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public struct Floor{

}
public class FloorGen : MonoBehaviour
{
    [SerializeField]
    Tilemap m_tilemap;
    [SerializeField]
    Vector2Int m_floorDimensions;
    [SerializeField]
    List<TileBase> m_tiles = new List<TileBase>();
    [SerializeField]
    List<int> m_points = new List<int>();
    [SerializeField]
    Vector2Int m_dungeonDimensions;
    [SerializeField]
    List<Vector3Int> m_tileEdge = new List<Vector3Int>();

    void Start()
    {
        m_tilemap = GameObject.Find("Map").GetComponent<Tilemap>();
        for(int i = 0; i < m_dungeonDimensions.x; ++i)
        {
          m_points.Add(i);
        }
        for(int i = 0; i < m_dungeonDimensions.y; ++i)
        {
          m_points.Add(i);
        }
    }

    public void RebuildFloor()
    {
      RandomiseFloorDimensions();
      BuildFloor();

    }
    void RandomiseFloorDimensions()
    {
        int randomPoint = Random.Range(0, m_points.Count);
        Debug.Log(randomPoint);
        m_floorDimensions = new Vector2Int(Random.Range(0, 25),Random.Range(0, 25));
        m_points.Remove(randomPoint);
    }
    void BuildFloor()
    {
      m_tileEdge.Clear();
      for(int i = 0 ; i < m_floorDimensions.x; ++i)
      {
          for(int a = 0 ; a < m_floorDimensions.y; ++a)
          {
              Vector3Int posY = new Vector3Int(i,a,0);
              if(m_tilemap.GetTile(posY) == null)
              m_tilemap.SetTile(posY,m_tiles[0]);

              if(m_points.Contains(i))
                m_points.Remove(i);
              if(m_points.Contains(a))
                m_points.Remove(a);
                Vector3Int yEdge = new Vector3Int(0,a,0);
                if(m_tilemap.GetTile(yEdge) != null)
                {
                  m_tileEdge.Add(yEdge);
                }
          }
          Vector3Int xEdge = new Vector3Int(i,0,0);
          if(m_tilemap.GetTile(xEdge) != null)
          {
            m_tileEdge.Add(xEdge);
          }
      }
      BuildWall();
    }
    void BuildWall()
    {
      for(int i =0 ; i < m_tileEdge.Count; ++i)
      {
        m_tilemap.SetTile(m_tileEdge[i] , m_tiles[1]);
      }
    }
}
