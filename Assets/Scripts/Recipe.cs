using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemsWithQuantity
{
    public ItemBase _Item;
    public int _Quantity;
}


[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public float _Cooldown = 0;
    public List<ItemsWithQuantity> _InputItem = new ();
    public List<ItemsWithQuantity> _OutputItem = new();
}
