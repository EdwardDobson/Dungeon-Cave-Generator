using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockDrop : MonoBehaviour
{
    public CustomTile Tile;
    public SpriteRenderer Sprite;
    InventoryDisplay m_display;
    GameManager m_manager;
    private void Start()
    {
        if (!m_manager.Creative)
            m_display = GameObject.Find("Inventory").GetComponent<InventoryDisplay>();

    }
    public void SetUp(CustomTile _tile)
    {
        CustomTile tempTile = TileManager.GetTileHolder(_tile.Type).Tiles.Where(t => t.TileName == _tile.TileName).First();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Tile = _tile;
        Sprite = GetComponent<SpriteRenderer>();
        Tile.Item = Instantiate(_tile.Item);
        if (_tile.Item.CanBePlaced)
            Tile.Item.ItemID = _tile.ID;
        if (!_tile.Item.CanBePlaced)
            Tile.Item.ItemID = tempTile.ID;
        if (Tile.Item != null)
        {
            if (!Tile.Item.CanBePlaced)
            {
                Sprite.sprite = Tile.Item.Sprite;
            }
        }

        if (Tile.Item.CanBePlaced)
        {
            Sprite.sprite = Tile.DisplaySprite;
        }
        if (_tile.ShouldUseColour)
            Sprite.color = _tile.TileColour;
        float randomPlacementX = Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f);
        float randomPlacementY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
        transform.position = new Vector3(randomPlacementX, randomPlacementY, -0.1f);

    }
    private void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, 0, -1), Time.deltaTime * 50);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("PlayerPick"))
        {
            bool m_foundSlot = false;
            for (int i = 0; i < collision.transform.parent.GetComponent<InventoryBackpack>().Storage.Count; ++i)
            {
                for (int a= 0; a < collision.transform.parent.GetComponent<InventoryBackpack>().Storage[i].Items.Count; ++a)
                {
                    if(collision.transform.parent.GetComponent<InventoryBackpack>().Storage[i].Items[a].Name == Tile.Item.Name)
                    {
                        m_foundSlot = true;
                        collision.transform.parent.GetComponent<InventoryBackpack>().AddToStorage(Tile);
                        Debug.Log("Storage slot");
                        Destroy(gameObject);
                        break;
                    }
                }
            }
            if (!m_foundSlot)
            {
                if (collision.transform.parent.GetComponent<InventoryBackpack>().Storage.Count < collision.transform.parent.GetComponent<InventoryBackpack>().StorageCapacity)
                {
                    collision.transform.parent.GetComponent<InventoryBackpack>().AddToStorage(Tile);
                    Destroy(gameObject);
                }
            }
        }
    }
}
