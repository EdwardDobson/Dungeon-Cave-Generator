using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Item requirements
[Serializable]
public class RecipeItem
{
    public  Item ItemType;
    public int Amount;
    public string ItemName;
    public bool RequiredIngredientsMet;
}
[CreateAssetMenu(fileName = "Recipe", menuName = "Create Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public List<RecipeItem> Items;
    public bool CanBuild;
    public CustomTile Output;
    public int RecipeID;
    public void CheckCanBuild()
    {
       if(Items.All(i => i.RequiredIngredientsMet))
        {
            CanBuild = true;
        }
    }
}
