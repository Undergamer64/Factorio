using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public float _Cooldown = 0;
    public List<ItemsWithQuantity> _InputItem = new ();
    public List<ItemsWithQuantity> _OutputItem = new();
}
