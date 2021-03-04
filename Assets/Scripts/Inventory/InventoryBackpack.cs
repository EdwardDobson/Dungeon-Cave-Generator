using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class ItemInventory
{
    public List<CustomTile> Items = new List<CustomTile>();
}

public class InventoryBackpack : MonoBehaviour
{
    [SerializeField]
    public List<ItemInventory> Storage;
    public int StorageCapacity;
    public List<CustomTile> DisplayBuffer;
    public InventoryDisplay Display;
    private void Start()
    {
        for(int i  =0; i < StorageCapacity; ++i)
        {
            ItemInventory tempList = new ItemInventory();
            Storage.Add(tempList);
        }
    }
    public void AddToStorage(CustomTile _customTile)
    {
        for (int i = 0; i < Storage.Count; ++i)
        {
            if (Storage[i].Items.Count > 0)
            {
                if (Storage[i].Items.Any(t => t.ID == _customTile.ID))
                {
                    Storage[i].Items.Add(_customTile);
                    if(DisplayBuffer.All(t => t.ID != _customTile.ID))
                    DisplayBuffer.Add(_customTile);
                    if(Storage[i].Items.Count <= 1)
                        Display.AddToSlot(_customTile);

                    break;
                }
            }
            if (Storage[i].Items.Count <= 0)
            {
                Storage[i].Items.Add(_customTile);
                if (DisplayBuffer.All(t => t.ID != _customTile.ID))
                    DisplayBuffer.Add(_customTile);
                Display.AddToSlot(_customTile);
                break;
            }
        }
    }
}
