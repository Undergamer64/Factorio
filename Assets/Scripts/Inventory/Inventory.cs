using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Slot> _Slots = new List<Slot>();
    public List<Input> _Inputs = new List<Input>();
    public List<Output> _Outputs = new List<Output>();

    public List<ItemBase> _WhiteListItems = new List<ItemBase>();
    public List<ItemBase> _BlackListItems = new List<ItemBase>();

    public void AddItem(ItemBase item, int quantity = 1)
    {

    }

    /// <summary>
    /// Removes the quantity "quantity" of last instance of the item "item" in inventory.   
    /// WARNING : you need to check the number of items in inventory or it will pull the maximum to reach quantity
    /// </summary>
    public void RemoveItems(ItemBase item, int quantity)
    {
        int tempQuantity = quantity;
        for (int i = _Slots.Count - 1; i >= 0; i--)
        {
            if (tempQuantity <= 0)
            {
                return;
            }
            if (_Slots[i].Item == null)
            {
                continue;
            }
            if (_Slots[i].Item == item && _Slots[i].Quantity > 0)
            {
                ItemsWithQuantity itemsWithQuantity = new ItemsWithQuantity();
                itemsWithQuantity.Item = item;

                if (tempQuantity > _Slots[i].Quantity)
                {
                    tempQuantity -= _Slots[i].Quantity;
                    _Slots[i].UpdateQuantity(0);

                    itemsWithQuantity.Quantity = -(tempQuantity - _Slots[i].Quantity);
                }
                else
                {
                    _Slots[i].UpdateQuantity(_Slots[i].Quantity - tempQuantity);

                    itemsWithQuantity.Quantity = -tempQuantity;

                    return;
                }
            }
        }
    }

    public int SearchItemQuantity(ItemBase item)
    {
        int quantity = 0;

        foreach (Slot slot in _Slots)
        {
            if (slot._Item == item)
            {
                quantity += slot._Quantity;
            }
        }

        return quantity;
    }
}
