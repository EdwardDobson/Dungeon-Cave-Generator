using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlaceTile : MonoBehaviour
{
    [SerializeField]
    int m_index;
    public Image[] PlaceImages;
    [SerializeField]
    List<CustomTile> m_customTilesToPlace = new List<CustomTile>();
    [SerializeField]
    List<CustomTile> m_tilesForHotBar = new List<CustomTile>();
    public float MaxRange;
    public TextMeshProUGUI BlockInfo;
    public Dictionary<Vector3Int, CustomTile> PlacedOnTiles;
    [SerializeField]
    GameObject m_audioPlaceSource;
    public HotBarScrolling HotBarScrolling;
    public InventoryBackpack BackPack;
    GameManager m_manager;
    private void Start()
    {
        PlacedOnTiles = new Dictionary<Vector3Int, CustomTile>();
        BackPack = GetComponent<InventoryBackpack>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void FillTilesList()
    {
        for (int i = 0; i < TileManager.GetTileHolder(TileType.Wall).Tiles.Count; ++i)
        {
            if (!m_customTilesToPlace.Contains(TileManager.GetTileHolder(TileType.Wall).Tiles[i]))
                m_customTilesToPlace.Add(TileManager.GetTileHolder(TileType.Wall).Tiles[i]);
        }
        for (int i = 0; i < TileManager.GetTileHolder(TileType.Floor).Tiles.Count; ++i)
        {
            if (!m_customTilesToPlace.Contains(TileManager.GetTileHolder(TileType.Floor).Tiles[i]))
                m_customTilesToPlace.Add(TileManager.GetTileHolder(TileType.Floor).Tiles[i]);
        }
    }
    private void Awake()
    {
        for (int i = 0; i < m_customTilesToPlace.Count; ++i)
        {
            if (i < 9)
                m_tilesForHotBar.Add(m_customTilesToPlace[i]);

        }
        for (int i = 0; i < m_tilesForHotBar.Count; ++i)
        {
            PlaceImages[i].GetComponent<HoldCustomTile>().CustomTile = m_tilesForHotBar[i];
            PlaceImages[i].color = m_tilesForHotBar[i].TileColour;
            PlaceImages[i].sprite = m_tilesForHotBar[i].DisplaySprite;
        }
    }
    public void RefreshTileImages()
    {
        for (int i = 0; i < m_tilesForHotBar.Count; ++i)
        {
            m_tilesForHotBar[i] = PlaceImages[i].GetComponent<HoldCustomTile>().CustomTile;
            PlaceImages[i].color = PlaceImages[i].GetComponent<HoldCustomTile>().CustomTile.TileColour;
            PlaceImages[i].sprite = m_tilesForHotBar[i].DisplaySprite;
        }
        for (int i = 0; i < PlaceImages.Length; ++i)
        {
            if (i == m_index)
            {
                PlaceImages[i].color = m_tilesForHotBar[i].TileColour;
                PlaceImages[i].sprite = m_tilesForHotBar[i].DisplaySprite;
            }
            else
            {
                PlaceImages[i].color = new Color(m_tilesForHotBar[i].TileColour.r, m_tilesForHotBar[i].TileColour.g, m_tilesForHotBar[i].TileColour.b, 0.6f);
            }
        }
        BlockInfo.text = m_tilesForHotBar[m_index].name + "\nType: " + m_tilesForHotBar[m_index].Type.ToString();
    }
    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (!m_manager.Creative)
                PlaceTileClick(HotBarScrolling.TileToPlace());
        }
    }
    public void PlaceTileClick(CustomTile _tile)
    {
        if (_tile != null)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int v = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0);
            float distance = Vector3Int.Distance(v, new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z));
            CustomTile newCopy = Instantiate(_tile);
            if (distance <= MaxRange)
            {
                if (Input.GetMouseButton(1))
                {
                    if (WallGen.GetTilemap().GetTile(v) == null)
                    {
                        if (newCopy.Type == TileType.Wall)
                        {
                         
                            if (new Vector3Int((int)transform.position.x, (int)transform.position.y, 0) != v)
                            {
                                if (!m_manager.Creative && BackPack.GetStorageTypeCount(_tile) > 0)
                                {
                                    for (int a = 0; a < TileManager.GetTileHolder(newCopy.Type).Tiles.Count; ++a)
                                    {
                                        if (TileManager.GetTileHolder(newCopy.Type).Tiles[a].ID == newCopy.ID)
                                        {
                                            newCopy = TileManager.GetTileHolder(newCopy.Type).Tiles[a];
                                        }
                                    }
                                    TileManager.PlaceTile(v, m_index, DungeonUtility.GetTilemap(), WallGen.GetTilemap(), newCopy, DictionaryType.Walls);
                                    Instantiate(m_audioPlaceSource);
                                    BackPack.RemoveFromStorage(newCopy);
                                    newCopy = BackPack.GetNewItem(_tile);
                                }
                                if (m_manager.Creative)
                                {
                                    for (int a = 0; a < TileManager.GetTileHolder(newCopy.Type).Tiles.Count; ++a)
                                    {
                                        if (TileManager.GetTileHolder(newCopy.Type).Tiles[a].ID == newCopy.ID)
                                        {
                                            newCopy = TileManager.GetTileHolder(newCopy.Type).Tiles[a];
                                        }
                                    }
                                    TileManager.PlaceTile(v, m_index, DungeonUtility.GetTilemap(), WallGen.GetTilemap(), newCopy, DictionaryType.Walls);
                                    Instantiate(m_audioPlaceSource);
                                }
                                if (!PlacedOnTiles.ContainsKey(v))
                                {
                                    PlacedOnTiles.Add(v, TileManager.GetTileDictionaryFloor()[v].CustomTile);
                                    Debug.Log(TileManager.GetTileDictionaryFloor()[v].CustomTile.name);
                                }
                            }
                        }
                    }
                    if(TileManager.GetTileDictionaryFloor()[v].CustomTile.ID != _tile.ID)
                    {
                        if (DungeonUtility.GetTilemap().GetTile(v) != null && WallGen.GetTilemap().GetTile(v) == null)
                        {
                            if (newCopy.Type == TileType.Floor || newCopy.Type == TileType.Path)
                            {
                                if (!m_manager.Creative && BackPack.GetStorageTypeCount(_tile) > 0)
                                {
                                    for (int a = 0; a < TileManager.GetTileHolder(newCopy.Type).Tiles.Count; ++a)
                                    {
                                        if (TileManager.GetTileHolder(newCopy.Type).Tiles[a].ID == newCopy.ID)
                                        {
                                            newCopy = TileManager.GetTileHolder(newCopy.Type).Tiles[a];
                                        }
                                    }
                                    TileManager.PlaceTile(v, m_index, DungeonUtility.GetTilemap(), DungeonUtility.GetTilemap(), newCopy, DictionaryType.Floor);

                                    Instantiate(m_audioPlaceSource);
                                    BackPack.RemoveFromStorage(newCopy);
                                    newCopy = BackPack.GetNewItem(_tile);
                                }
                                if (m_manager.Creative)
                                {
                                    for (int a = 0; a < TileManager.GetTileHolder(newCopy.Type).Tiles.Count; ++a)
                                    {
                                        if (TileManager.GetTileHolder(newCopy.Type).Tiles[a].ID == newCopy.ID)
                                        {
                                            newCopy = TileManager.GetTileHolder(newCopy.Type).Tiles[a];
                                        }
                                    }
                                    TileManager.PlaceTile(v, m_index, DungeonUtility.GetTilemap(), DungeonUtility.GetTilemap(), newCopy, DictionaryType.Floor);
                                    Instantiate(m_audioPlaceSource);
                                    Debug.Log("Placing floor: " + _tile.name);
                                }
                            }
                        }
                    }
          
                }
            }
        }

    }
    public List<CustomTile> GetCustomTiles()
    {
        return m_customTilesToPlace;
    }
}