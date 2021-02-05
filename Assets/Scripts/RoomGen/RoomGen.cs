using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomGen : MonoBehaviour
{
  [SerializeField]
  Tilemap m_tilemap;
  [SerializeField]
  Vector2Int m_wallDimensions;
  [SerializeField]
  List<TileBase> m_tiles = new List<TileBase>();
  [SerializeField]
  List<Vector3Int> m_tilePostions = new List<Vector3Int>();
  Vector3Int m_tempPosition;
  int m_doorAmount;

    void Start()
    {
      m_doorAmount = Random.Range(1, 4);
      RebuildRoom();
    }
    void BuildWalls()
    {
      m_tilePostions.Clear();
      for(int i = 0 ; i < m_wallDimensions.x + 1; ++i)
      {
        BuildPiece(i,0,0);
        if(m_tempPosition != new Vector3Int(0,0,0) && m_tempPosition != new Vector3Int(m_wallDimensions.x,0,0) && m_tempPosition != new Vector3Int(m_wallDimensions.x,m_wallDimensions.y,0))
        {
            m_tilePostions.Add(m_tempPosition);
        }
        BuildPiece(i,m_wallDimensions.y,0);
      }
      for(int a = 0 ; a < m_wallDimensions.y; ++a)
      {
        BuildPiece(0,a,0);
        if(m_tempPosition != new Vector3Int(0,0,0)&& m_tempPosition != new Vector3Int(0,m_wallDimensions.y,0)&& m_tempPosition != new Vector3Int(m_wallDimensions.x,m_wallDimensions.y,0))
        {
            m_tilePostions.Add(m_tempPosition);
        }
        BuildPiece(m_wallDimensions.x,a,0);
      }
      for(int i  =0 ; i < m_doorAmount; ++i)
      {
        PlaceDoor();
      }
      FillFloor();
    }
    void BuildPiece(int _value1, int _value2, int _tileIndex)
    {
      Vector3Int posY = new Vector3Int(_value1,_value2,0);
      if(m_tilemap.GetTile(posY) == null)
      m_tilemap.SetTile(posY,m_tiles[_tileIndex]);
      m_tempPosition = posY;

    }
    void FillFloor()
    {
      for(int i = 0 ; i < m_wallDimensions.x; ++i)
      {
          for(int a = 0 ; a < m_wallDimensions.y; ++a)
          {
            BuildPiece(i,a,1);
          }
      }
    }
    public void RebuildRoom()
    {
      RandomiseWallDimensions();
      BuildWalls();
    }
    void PlaceDoor()
    {
      int randomPosChoice = Random.Range(0, m_tilePostions.Count);
      Vector3Int randomWallPos = m_tilePostions[randomPosChoice];
      m_tilemap.SetTile(randomWallPos, null);
      Debug.Log(randomWallPos);
    }
    void RandomiseWallDimensions()
    {
      m_wallDimensions = new Vector2Int(Random.Range(5, 25),Random.Range(5, 25));
    }
}
