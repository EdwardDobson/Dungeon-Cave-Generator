using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockDrop : MonoBehaviour
{
    public CustomTile Tile;
    public SpriteRenderer Sprite;
    InventoryDisplay m_display;
    private void Start()
    {
        m_display = GameObject.Find("Inventory").GetComponent<InventoryDisplay>();
    }
    public void SetUp(CustomTile _tile)
    {
        Tile = _tile;
        Sprite = GetComponent<SpriteRenderer>();
        if(_tile.DisplaySprite != null)
        {
            Sprite.sprite = _tile.DisplaySprite;
        
        }
        Sprite.color = _tile.TileColour;
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
        m_display = GameObject.Find("Inventory").GetComponent<InventoryDisplay>();

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
