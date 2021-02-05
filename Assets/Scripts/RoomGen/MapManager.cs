using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapManager : MonoBehaviour
{
  [SerializeField]
  Tilemap m_prefab;
  Tilemap m_oneToSpawn;
  [SerializeField]
  int m_amountOfRooms;
  [SerializeField]
  Vector2Int m_dungeonDimensions;
  [SerializeField]
  List<Vector2Int> m_buildPoints = new List<Vector2Int>();

  [SerializeField]
  List<TileBase> m_tiles = new List<TileBase>();
    void Start()
    {
      for(int x =0; x < m_dungeonDimensions.x; ++x)
      {
        for(int y =0; y < m_dungeonDimensions.y; ++y)
        {
          Vector2Int temp = new Vector2Int(x,y);
          m_buildPoints.Add(temp);
        }
      }
      for(int i =0 ; i < m_amountOfRooms; ++i)
      {
          m_oneToSpawn =  Instantiate(m_prefab);
          m_oneToSpawn.transform.SetParent(gameObject.transform);
      }

    }
    public void RemoveBuildPoints(Vector2Int _pos)
    {
      m_buildPoints.Remove(_pos);
    }
    public List<Vector2Int> GetBuildPoints()
    {
      return m_buildPoints;
    }
    public List<TileBase> GetTileTypes()
    {
      return m_tiles;
    }
}
