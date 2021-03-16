using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftItem : MonoBehaviour
{
    public CraftingRecipeHolder RecipeHolder;
 
    InventoryBackpack m_backPack;
    InventoryDisplay m_inventoryDisplay;
    Recipe m_tempRecipe;
    int m_recipeIndex;
    void Start()
    {
        m_inventoryDisplay = GameObject.Find("Inventory").GetComponent<InventoryDisplay>();
        RecipeHolder = Resources.Load<CraftingRecipeHolder>("Crafting Recipes/Recipe Holder");
        m_backPack = GetComponent<InventoryBackpack>();
    }
    public void RemoveItems( List<Item> _storageSlot, int _indexri)
    {
        CustomTile t = Instantiate(m_tempRecipe.Output);
        t.Item.ItemID = m_tempRecipe.Output.ID;
        m_backPack.AddToStorage(t);
        m_backPack.RemoveMultipleItems(m_tempRecipe.Items[_indexri].Amount, _storageSlot);
    }
    public void CreateItem(int _recipeIndex)
    {
        if (m_backPack.Storage.Count <= m_backPack.StorageCapacity)
        {
            m_tempRecipe = RecipeHolder.Recipes[_recipeIndex];
            if (m_tempRecipe != null)
            {
                for (int i = 0; i < m_tempRecipe.Items.Count; ++i)
                {
                    for (int a = 0; a < m_backPack.Storage.Count; ++a)
                    {
                        for (int b = 0; b < m_backPack.Storage[a].Items.Count; ++b)
                        {
                            if (m_backPack.Storage[a].Items.Count >= m_tempRecipe.Items[i].Amount)
                            {
                                m_tempRecipe.Items[i].RequiredIngredientsMet = true;
                                if (m_backPack.Storage[a].Items[b].Name.Contains(m_tempRecipe.Items[i].ItemName))
                                {
                                    RemoveItems(m_backPack.Storage[a].Items, i);
                                    m_tempRecipe.CheckCanBuild();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
