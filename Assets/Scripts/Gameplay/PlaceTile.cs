using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
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
    EnemySpawner m_enemySpawner;
    public GameObject BlockDrop;
    float m_currentDropTimer;
    public float MaxDropTimer;


    private void Start()
    {
        m_currentDropTimer = MaxDropTimer;
        PlacedOnTiles = new Dictionary<Vector3Int, CustomTile>();
        BackPack = GetComponent<InventoryBackpack>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }
    public void FillTilesList()
    {
        List<CustomTile> Walls = TileManager.GetTileHolder(TileType.Wall).Tiles;
        List<CustomTile> Floor = TileManager.GetTileHolder(TileType.Floor).Tiles;
        List<CustomTile> Path = TileManager.GetTileHolder(TileType.Path).Tiles;
        List<CustomTile> Door = TileManager.GetTileHolder(TileType.Door).Tiles;
        for (int i = 0; i < Walls.Count; ++i)
        {
            if(!m_customTilesToPlace.Contains(Walls[i]))
            m_customTilesToPlace.Add(Walls[i]);
        }
        for (int i = 0; i < Floor.Count; ++i)
        {
            if (!m_customTilesToPlace.Contains(Floor[i]))
                m_customTilesToPlace.Add(Floor[i]);
        }
        for (int i = 0; i < Path.Count; ++i)
        {
            if (!m_customTilesToPlace.Contains(Path[i]))
                m_customTilesToPlace.Add(Path[i]);
        }
        for (int i = 0; i < Door.Count; ++i)
        {
            if (!m_customTilesToPlace.Contains(Door[i]))
                m_customTilesToPlace.Add(Door[i]);
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
        if(m_manager.CanPerformAction)
        {
            if (Time.timeScale > 0)
            {
                if (!m_manager.Creative)
                    PlaceTileClick(HotBarScrolling.TileToPlace());
                if (Input.GetKeyUp(KeyCode.F))
                {
                    m_currentDropTimer = 0;
                }
            }
       
        }

    }
    public void PlaceTileClick(CustomTile _tile)
    {
        if (_tile != null)
        {
            DropBlock(_tile);
        }
        if (_tile != null && _tile.Item != null && _tile.Item.CanBePlaced || m_manager.Creative)
        {
        
  
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int v = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0);
            if (m_enemySpawner.Enemies.All(g => new Vector3Int((int)g.transform.position.x, (int)g.transform.position.y, (int)g.transform.position.z) != v))
            {
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
                                        ApplySpriteVariation(newCopy, WallGen.GetTilemap(), v);
                                        Instantiate(m_audioPlaceSource);
                                        BackPack.RemoveFromStorage(newCopy);
                                        if(BackPack.GetNewItem(_tile) != null)
                                        newCopy.Item = Instantiate( BackPack.GetNewItem(_tile));
                               
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
                                        ApplySpriteVariation(newCopy, WallGen.GetTilemap(), v);
                                        Instantiate(m_audioPlaceSource);
                                    }
                                    if (!PlacedOnTiles.ContainsKey(v))
                                    {
                                        PlacedOnTiles.Add(v, TileManager.GetTileDictionaryFloor()[v].CustomTile);
                                    }
                                }
                            }
                        }
                        if (TileManager.GetTileDictionaryFloor().ContainsKey(v) && TileManager.GetTileDictionaryFloor()[v].CustomTile.ID != _tile.ID)
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
                                        ApplySpriteVariation(newCopy, DungeonUtility.GetTilemap(), v);
                                        Instantiate(m_audioPlaceSource);
                                        BackPack.RemoveFromStorage(newCopy);
                                        if (BackPack.GetNewItem(_tile) != null)
                                            newCopy.Item = Instantiate(BackPack.GetNewItem(_tile));
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
                                        ApplySpriteVariation(newCopy, DungeonUtility.GetTilemap(), v);
                                        Instantiate(m_audioPlaceSource);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    void DropBlock(CustomTile _tile)
    {
        if (Input.GetKey(KeyCode.F) && !m_manager.Creative && Input.GetAxis("Mouse ScrollWheel") == 0)
        {
            m_currentDropTimer -= Time.deltaTime;
            if(m_currentDropTimer <= 0)
            {
                ShouldBlockDrop(_tile);
                m_currentDropTimer = MaxDropTimer;
            }
        }
    }
    void ShouldBlockDrop(CustomTile _tile)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int v = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0);
        if (Vector2.Distance(transform.position,worldPosition) > BackPack.PickupRange)
        {
            if(WallGen.GetTilemap().GetTile(v) == null)
            {
                GameObject c = Instantiate(BlockDrop, worldPosition, Quaternion.identity);
                c.GetComponent<BlockDrop>().SetUp(_tile);
                BackPack.RemoveFromStorage(_tile);
            }
        }
    }
    void ApplySpriteVariation(CustomTile _tile, Tilemap _map,Vector3Int _pos)
    {
        if (_tile.SpriteVariations.Length > 0)
        {
            Tile tileT = _map.GetTile<Tile>(_pos);
            Sprite sT = _tile.DisplaySprite;
            if (sT != null)
                tileT.sprite = sT;
        }
    }
    public List<CustomTile> GetCustomTiles()
    {
        return m_customTilesToPlace;
    }
}