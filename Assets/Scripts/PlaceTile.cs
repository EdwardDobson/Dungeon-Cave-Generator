using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlaceTile : MonoBehaviour
{
    [SerializeField]
    int m_index;
    public Image PlaceImage;
    [SerializeField]
    List<CustomTile> m_customTilesToPlace;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void FillTilesList()
    {
        for (int i = 0; i < TileManager.GetTileHolder(TileType.Wall).Tiles.Count; ++i)
        {
            if (!m_customTilesToPlace.Contains(TileManager.GetTileHolder(TileType.Wall).Tiles[i]))
                m_customTilesToPlace.Add(TileManager.GetTileHolder(TileType.Wall).Tiles[i]);
        }

    }
    void Update()
    {

        float d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            m_index++;
            if (m_index > m_customTilesToPlace.Count - 1)
            {
                m_index = 0;
            }

        }
        else if (d < 0f)
        {
    
                m_index--;
            if (m_index < 0)
                m_index = m_customTilesToPlace.Count - 1;
        }
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int v = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0);
        if (Input.GetMouseButtonDown(1))
        {
            TileManager.RemoveTilePiece(v, DungeonUtility.GetTilemap());
            TileManager.GetTileDictionary().Remove(v);
            TileManager.BuildPiece(v.x, v.y, m_index, false, TileType.Wall, WallGen.GetTilemap());
            TileManager.FillDictionary(v, m_customTilesToPlace, m_index, WallGen.GetTilemap());
            TileManager.ChangeTileColour(WallGen.GetTilemap(), v, m_customTilesToPlace[m_index]);
            Debug.Log("Building Tile: " + WallGen.GetTilemap().GetTile(v).name + worldPosition);
        }


        PlaceImage.color = m_customTilesToPlace[m_index].TileColour;
        if (m_index > m_customTilesToPlace.Count - 1)
        {
            m_index = 0;
        }
    }
}
