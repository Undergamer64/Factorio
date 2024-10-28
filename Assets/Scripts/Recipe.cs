using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemsWithQuantity
{
    public ItemBase Item;
    public int Quantity;
}

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public List<ItemsWithQuantity> InputItem = new ();
    public List<ItemsWithQuantity> OutputItem = new();
}
