using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Create Item", order = 1)]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public int ItemID;
    public bool CanBePlaced;
}
