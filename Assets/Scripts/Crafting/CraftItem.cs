using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftItem : MonoBehaviour
{
    public List<Recipe> Recipes = new List<Recipe>();
    Object[] m_recipes;
    InventoryBackpack m_backPack;
    InventoryDisplay m_inventoryDisplay;
    Recipe m_tempRecipe;
    int m_recipeIndex;
    void Start()
    {
        m_recipes = Resources.LoadAll("Crafting Recipes", typeof(Recipe));
        m_inventoryDisplay = GameObject.Find("Inventory").GetComponent<InventoryDisplay>();
        foreach (Recipe r in m_recipes)
        {
            Recipe temp = Instantiate(r);
            temp.RecipeID = m_recipeIndex;
            if (!Recipes.Contains(r))
                Recipes.Add(temp);
            m_recipeIndex++;
        }
        m_backPack = GetComponent<InventoryBackpack>();
    }
    public void RemoveItems( int _indexa, int _indexri)
    {
        CustomTile t = Instantiate(m_tempRecipe.Output);
        t.Item.ItemID = m_tempRecipe.Output.ID;
        m_backPack.AddToStorage(t);
        m_backPack.RemoveMultipleItems(m_tempRecipe.Items[_indexri].Amount, _indexa);
    }
    public void CreateItem(int _recipeIndex)
    {
        m_tempRecipe = Recipes[_recipeIndex];
    
        if(m_tempRecipe != null)
        {
            for (int i = 0; i < m_tempRecipe.Items.Count; ++i)
            {
                for (int a = 0; a < m_backPack.Storage.Count; ++a)
                {
                    for (int b = 0; b < m_backPack.Storage[a].Items.Count; ++b)
                    {
                        if(m_backPack.Storage[a].Items.Count >= m_tempRecipe.Items[i].Amount)
                        {
                            m_tempRecipe.Items[i].RequiredIngredientsMet = true;
                                if (m_backPack.Storage[a].Items[b].Name == m_tempRecipe.Items[i].ItemName)
                                {
                                    RemoveItems(a, i);
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
