using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class ItemInventory
{
    public List<Item> Items = new List<Item>();
}

public class InventoryBackpack : MonoBehaviour
{
    [SerializeField]
    public List<ItemInventory> Storage;
    public int StorageCapacity;
    public InventoryDisplay Display;
    public HotBarScrolling HotBarScrolling;
    public float PickupRange;
    private void Start()
    {
        for(int i  =0; i < StorageCapacity; ++i)
        {
            ItemInventory tempList = new ItemInventory();
            Storage.Add(tempList);
        }
        transform.GetChild(0).GetComponent<CircleCollider2D>().radius = PickupRange;
    }
    private void Update()
    {
        transform.GetChild(0).GetComponent<CircleCollider2D>().radius = PickupRange;
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
    public void RemoveMultipleItems(int _amount,int _storageIndex)
    {
        for (int i = 0; i < _amount; ++i)
        {

                        Storage[_storageIndex].Items.RemoveAt(0);
                        HotBarScrolling.UpdateCountDisplay(Storage[_storageIndex].Items.Count);
        Debug.Log("Removing Item");
        }
    }
    public void RemoveFromStorage(CustomTile _customTile)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.ItemID == _customTile.ID))
                {
                    Storage[i].Items.RemoveAt(0);
                    HotBarScrolling.UpdateCountDisplay(Storage[i].Items.Count);
                }
            }
        }
    }
    public void ClearStorage(CustomTile _customTile)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.ItemID == _customTile.ID))
                {
                    Storage[i].Items.Clear();
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
                return    Storage[i].Items.Count;
                }
            }
        }
        return 0;
    }
    public void AddToStorage(CustomTile _customTile)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.ItemID == _customTile.Item.ItemID))
                {
                    Storage[i].Items.Add(_customTile.Item);
                    if(Storage[i].Items.Count <= 1)
                        Display.AddToSlot(_customTile);
                    break;
                }
            }
            if (Storage[i].Items.Count <= 0)
            {
                Storage[i].Items.Add(_customTile.Item);
                Display.AddToSlot(_customTile);
                break;
            }
        }
    }
}
