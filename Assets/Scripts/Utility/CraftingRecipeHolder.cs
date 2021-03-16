using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Recipe Holder", menuName = "Recipe Holder", order = 1)]
public class CraftingRecipeHolder : ScriptableObject
{
    public List<Recipe> Recipes = new List<Recipe>();

}