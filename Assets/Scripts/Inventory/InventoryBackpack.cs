using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class ItemInventory
{
    public List<Item> Items = new List<Item>();
    public int ID;
}

public class InventoryBackpack : MonoBehaviour
{
    [SerializeField]
    public List<ItemInventory> Storage;
    public int StorageCapacity;
    public InventoryDisplay Display;
    public HotBarScrolling HotBarScrolling;
    public List<Item> ItemsToAddBackIn = new List<Item>();
    public float PickupRange;
    private void Start()
    {

        transform.GetChild(0).GetComponent<CircleCollider2D>().radius = PickupRange;
    }
    private void Update()
    {
        transform.GetChild(0).GetComponent<CircleCollider2D>().radius = PickupRange;
        for(int i = 0; i < Storage.Count; ++i)
        {
            if(Storage[i].Items.Count <= 0)
            {
                Storage.RemoveAt(i);
            }
        }
    }
    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.ItemID == _item.ItemID))
                {
                    Storage[i].Items.RemoveAt(0);
                    HotBarScrolling.UpdateCountDisplay(Storage[i].Items.Count);
                }
            }
        }
    }
    public void RemoveMultipleItems(int _amount,List<Item> _storageSlot)
    {

        for (int i = 0; i < _amount; ++i)
        {
            ItemsToAddBackIn.Add(_storageSlot[0]);
            _storageSlot.RemoveAt(0);
 
        }
    }

    public void AddMultipleItems(List<Item> _storageSlot)
    {
        for (int i = 0; i < ItemsToAddBackIn.Count; ++i)
        {
            _storageSlot.Add(ItemsToAddBackIn[i]);
            Display.UpdateCountDisplaySlot();
        }
    }
    public void RemoveFromStorage(CustomTile _customTile)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.Name == _customTile.Item.Name))
                {
                    Storage[i].Items.RemoveAt(0);
                    HotBarScrolling.UpdateCountDisplay(Storage[i].Items.Count);
                }
            }
        }
    }
    public void ClearStorage(CustomTile _customTile)
    {
        if(_customTile != null)
        {
            for (int i = 0; i < Storage.Count; ++i)
            {
                if (Storage[i].Items.Count > 0)
                {
                    if (Storage[i].Items.Any(t => t.ItemID == _customTile.ID))
                    {
                        Storage.RemoveAt(i);
                        break;
                    }
                }
            }
        }

    }
    public List<Item> GetItems(CustomTile _customTile)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.ItemID == _customTile.ID))
                {
                    return Storage[i].Items;
                }
            }
        }
        return null;
    }
    public Item GetNewItem(CustomTile _customTile)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.ItemID == _customTile.ID))
                {
                    return Storage[i].Items[0];
                }
            }
        }
        return null;
    }
    public int GetStorageTypeCount(CustomTile _customTile)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.ItemID == _customTile.ID))
                {
                    return Storage[i].Items.Count;
                }
            }
        }
        return 0;
    }
    public void AddToStorage(CustomTile _customTile)
    {
        if (Storage.Count <= StorageCapacity)
        {
            for (int i = 0; i < Storage.Count; ++i)
            {
                if (Storage[i].Items.Count <= 0)
                {
                    Storage.RemoveAt(i);
                }
            }
            if (Storage.Any(t => t.Items.All(t => t.ItemID == _customTile.Item.ItemID)))
            {
                    ItemInventory itemIv = Storage.Where(s => s.Items.Any(t => t.ItemID == _customTile.Item.ItemID)).First();
                itemIv.ID = _customTile.Item.ItemID;
                    itemIv.Items.Add(_customTile.Item);
            }
            if (Storage.All(t=>t.Items.All(t => t.ItemID != _customTile.Item.ItemID)))
            {
                ItemInventory itemIv = new ItemInventory();
                itemIv.ID = _customTile.Item.ItemID;
                itemIv.Items.Add(_customTile.Item);
                Storage.Add(itemIv);
                Display.AddToSlot(_customTile);
            }
        }
    }
}
