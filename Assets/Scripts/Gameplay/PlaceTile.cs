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
    [SerializeField]
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
    Vector3Int m_placePos;
    FileManager m_fileManager;
    private void Start()
    {
        m_currentDropTimer = MaxDropTimer;
        PlacedOnTiles = new Dictionary<Vector3Int, CustomTile>();
        BackPack = GetComponent<InventoryBackpack>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_fileManager = GameObject.Find("Save").GetComponent<FileManager>();
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
            if (!m_customTilesToPlace.Contains(Walls[i]))
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
        if (m_manager.CanPerformAction)
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
            m_placePos = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0);
            if (m_enemySpawner.Enemies.All(g => new Vector3Int((int)g.transform.position.x, (int)g.transform.position.y, (int)g.transform.position.z) != m_placePos))
            {
                float distance = Vector3Int.Distance(m_placePos, new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z));
                CustomTile newCopy = Instantiate(_tile);
                if (distance <= MaxRange)
                {
                    if (Input.GetMouseButton(1))
                    {
                        if (newCopy.Type == TileType.Wall)
                        {
                            if (WallGen.GetTilemap().GetTile(m_placePos) == null)
                            {
                                if (new Vector3Int((int)transform.position.x, (int)transform.position.y, 0) != m_placePos)
                                {
                                    PTile(newCopy);
                                   if (!m_fileManager.PlacedOnTiles.ContainsKey(m_placePos))
                                    {
                                        m_fileManager.PlacedOnTiles.Add(m_placePos, TileManager.GetTileDictionaryFloor()[m_placePos].CustomTile);
                                    }
                                    if (TileManager.GetTileDictionaryFloor().ContainsKey(m_placePos))
                                    {
                                       int id = TileManager.GetTileDictionaryFloor()[m_placePos].CustomTile.ID;
                                        DataToSave tempData = new DataToSave
                                        {
                                            PosX = m_placePos.x,
                                            PosY = m_placePos.y,
                                            PosZ = m_placePos.z,
                                            IsPlacedTile = true,
                                            ID = id,
                                        };
                                        m_fileManager.Input(tempData);
                                    }
                                }
                            }
                        }
                        if (newCopy.Type == TileType.Floor || newCopy.Type == TileType.Path)
                        {
                            if(WallGen.GetTilemap().GetTile(m_placePos) == null)
                            {
                                if (TileManager.GetTileDictionaryFloor()[m_placePos].CustomTile.ID != _tile.ID)
                                {
                                    PTile(newCopy);
                                }
                            }
                      
                        }
                    }
                }
            }
        }
    }
    void PTile(CustomTile _copy)
    {
        Tilemap toPlace = null;
        Tilemap toRemove = null;
        DictionaryType _type = DictionaryType.Walls;
        switch (_copy.Type)
        {
            case TileType.Wall:
                toPlace = WallGen.GetTilemap();
                toRemove = DungeonUtility.GetTilemap();
                _type = DictionaryType.Walls;
                break;
            case TileType.Floor:
                toPlace = DungeonUtility.GetTilemap();
                toRemove = DungeonUtility.GetTilemap();
                _type = DictionaryType.Floor;
                break;
            case TileType.Path:
                toPlace = DungeonUtility.GetTilemap();
                toRemove = DungeonUtility.GetTilemap();
                _type = DictionaryType.Floor;
                break;
        }
        int id = 0;
        for (int a = 0; a < TileManager.GetTileHolder(_copy.Type).Tiles.Count; ++a)
        {
            if (TileManager.GetTileHolder(_copy.Type).Tiles[a].ID == _copy.ID)
            {
                _copy = TileManager.GetTileHolder(_copy.Type).Tiles[a];
                id = TileManager.GetTileHolder(_copy.Type).Tiles[a].ID;
            }
        }
        TileManager.PlaceTile(m_placePos, m_index, toRemove, toPlace, _copy, _type);
        ApplySpriteVariation(_copy, toPlace, m_placePos);
        Instantiate(m_audioPlaceSource);
        if (!m_manager.Creative && BackPack.GetStorageTypeCount(_copy) > 0)
        {
            BackPack.RemoveFromStorage(_copy);
            if (BackPack.GetNewItem(_copy) != null)
                _copy.Item = Instantiate(BackPack.GetNewItem(_copy));
        }
        DataToSave tempData = new DataToSave
        {
            PosX = m_placePos.x,
            PosY = m_placePos.y,
            PosZ = m_placePos.z,
            ID = id
        };
        m_fileManager.Input(tempData);
    }
    void DropBlock(CustomTile _tile)
    {
        if (Input.GetKey(KeyCode.F) && !m_manager.Creative && Input.GetAxis("Mouse ScrollWheel") == 0)
        {
            m_currentDropTimer -= Time.deltaTime;
            if (m_currentDropTimer <= 0)
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
        if (Vector2.Distance(transform.position, worldPosition) > BackPack.PickupRange)
        {
            if (WallGen.GetTilemap().GetTile(v) == null)
            {
                GameObject c = Instantiate(BlockDrop, worldPosition, Quaternion.identity);
                c.GetComponent<BlockDrop>().SetUp(_tile);
                BackPack.RemoveFromStorage(_tile);
            }
        }
    }
    void ApplySpriteVariation(CustomTile _tile, Tilemap _map, Vector3Int _pos)
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