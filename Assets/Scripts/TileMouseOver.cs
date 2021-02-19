using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileMouseOver : MonoBehaviour
{
    public Tilemap map;
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gPos = map.WorldToCell(mPos);
            TileBase clickedTile = map.GetTile(gPos);
            if (clickedTile != null)
            {
              //  Debug.Log(TileManager.GetCustomTile(TileManager.Get).Speed);
            }
        }
  
    }

}
