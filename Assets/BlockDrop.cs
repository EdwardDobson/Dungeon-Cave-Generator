using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDrop : MonoBehaviour
{
    public CustomTile Tile;
    public SpriteRenderer Sprite;
    public void SetUp(CustomTile _tile)
    {
        Tile = _tile;
        Sprite = GetComponent<SpriteRenderer>();
        if(_tile.DisplaySprite != null)
        {
            Sprite.sprite = _tile.DisplaySprite;
        
        }
        Sprite.color = _tile.TileColour;
    }
    private void Update()
    {
        transform.RotateAround(transform.position,new Vector3(0,0,-1), Time.deltaTime * 50);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player"))
        {
            collision.GetComponent<InventoryBackpack>().AddToStorage(Tile);
            Destroy(gameObject);
        }
    }
}
