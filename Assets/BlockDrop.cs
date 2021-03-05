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
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Tile = _tile;
        Sprite = GetComponent<SpriteRenderer>();
        if(_tile.DisplaySprite != null)
        {
            Sprite.sprite = _tile.DisplaySprite;
        }
        if(_tile.ShouldUseColour)
        Sprite.color = _tile.TileColour;
        float randomPlacementX = Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f);
        float randomPlacementY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
        transform.position = new Vector3(randomPlacementX, randomPlacementY, -0.1f);
   
    }
    private void Update()
    {
        transform.RotateAround(transform.position,new Vector3(0,0,-1), Time.deltaTime * 50);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player"))
        {
            if(collision.GetComponent<InventoryBackpack>().Storage.Any(i => i.Items.Count <=0) || m_display.HotBar.SlotsHotbar.Any(i => i.transform.GetChild(0).GetComponent<HoldCustomTile>().CustomTile == null))
            {
                collision.GetComponent<InventoryBackpack>().AddToStorage(Tile);
                Destroy(gameObject);
            }
            
        }
    }
}
