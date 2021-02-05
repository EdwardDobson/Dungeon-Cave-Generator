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
  List<Vector3Int> m_tilePostions = new List<Vector3Int>();
  int m_doorAmount;

    void Start()
    {
      m_doorAmount = Random.Range(1, 4);
      RebuildRoom();
    }
    void BuildWalls()
    {
      for(int i = 0 ; i < m_wallDimensions.x + 1; ++i)
      {
        BuildPiece(i,0,0);
        BuildPiece(i,m_wallDimensions.y,0);
      }
      for(int a = 0 ; a < m_wallDimensions.y; ++a)
      {
        BuildPiece(0,a,0);
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
      m_tilePostions.Add(posY);
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
      Vector3Int randomWallPos = new Vector3Int(Random.Range(0,m_tilePostions[randomPosChoice].x),Random.Range(0,m_tilePostions[randomPosChoice].y),0);
      m_tilemap.SetTile(randomWallPos, null);
    }
    void RandomiseWallDimensions()
    {
      m_wallDimensions = new Vector2Int(Random.Range(5, 25),Random.Range(5, 25));
    }
}
