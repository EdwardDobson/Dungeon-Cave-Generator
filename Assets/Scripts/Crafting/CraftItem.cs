using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftItem : MonoBehaviour
{
    public CraftingRecipeHolder RecipeHolder;
    public GameObject CraftingMenu;
    public GameObject CreativeInventory;
    public GameObject SurvivalInventory;
    public Minimap m_minimap;
    InventoryBackpack m_backPack;
    InventoryDisplay m_inventoryDisplay;
    Recipe m_tempRecipe;
    void Start()
    {
        m_inventoryDisplay = GameObject.Find("Inventory").GetComponent<InventoryDisplay>();
        m_minimap = GameObject.Find("HUD").GetComponent<Minimap>();
        RecipeHolder = Resources.Load<CraftingRecipeHolder>("Crafting Recipes/Recipe Holder");
        m_backPack = GetComponent<InventoryBackpack>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (!CraftingMenu.activeSelf)
            {
                CraftingMenu.SetActive(true);
                Time.timeScale = 0;
                m_minimap.ShowSmallMap();
            }
            else if (CraftingMenu.activeSelf)
            {
                CraftingMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
        if(CreativeInventory.activeSelf || SurvivalInventory.activeSelf)
        {
            CraftingMenu.SetActive(false);
        }
    }
    public void RemoveItems(List<Item> _storageSlot, int _indexri)
    {
        CustomTile t = Instantiate(m_tempRecipe.Output);
        t.Item.ItemID = m_tempRecipe.Output.ID;
        m_backPack.ItemsToAddBackIn.Clear();
        if(_storageSlot.All(i => i.Name != t.Item.Name))
        {
            m_backPack.RemoveMultipleItems(m_tempRecipe.Items[_indexri].Amount, _storageSlot);
        }
        bool canBuild = false ;
        if (m_backPack.Storage.Count + 1 <= m_backPack.StorageCapacity || m_backPack.Storage.Any(i => i.Items.Contains(t.Item)))
        {

            m_backPack.AddToStorage(t);
            canBuild = true;
            m_inventoryDisplay.UpdateCountDisplaySlot();
        }
        if(!canBuild)
        {
            m_backPack.AddMultipleItems( _storageSlot);
        }
    }
    public void CreateItem(int _recipeIndex)
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
