using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Slot> _Slots = new List<Slot>();
    public List<Input> _Inputs = new List<Input>();
    public List<Output> _Outputs = new List<Output>();

    //public List<ItemBase> _WhiteListItems = new List<ItemBase>();
    //public List<ItemBase> _BlackListItems = new List<ItemBase>();


    /// <summary>
    /// Tries to Add "quantity" items of type "item"
    /// </summary>
    /// <returns>The remaining quantity of items left to add (0 if none are left)</returns>
    public int TryAddItems(ItemBase item, int quantity = 1)
    {
        int remainingItems = quantity;

        for (int i = 0; i < quantity; i++)
        {
            if (FindFirstSlotAvailable(item) == null)
            {
                return remainingItems;
            }

            AddItem(item);
            remainingItems--;
        }
        return remainingItems;
    }

    /// <summary>
    /// Adds the item "item" to the inventory if a slot is available
    /// </summary>
    private void AddItem(ItemBase item)
    {
        Slot itemSlot = FindFirstSlotAvailable(item);
        
        if (itemSlot == null)
        {
            return;
        }

        if (itemSlot.Item == null)
        {
            itemSlot.AddItemToSlot(item);
        }
        else
        {
            itemSlot.UpdateQuantity(itemSlot.Quantity + 1);
        }
    }

    /// <summary>
    /// Tries to remove the quantity "quantity" of last instance of the item "item" in inventory.
    /// </summary>
    /// <returns>The remaining quantity of items left to remove (0 if none are left)</returns>
    public void TryRemoveItems(ItemBase item, int quantity)
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
                if (tempQuantity > _Slots[i].Quantity)
                {
                    tempQuantity -= _Slots[i].Quantity;
                    _Slots[i].UpdateQuantity(0);
                }
                else
                {
                    _Slots[i].UpdateQuantity(_Slots[i].Quantity - tempQuantity);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Counts the number of item "item" in the inventory and returns it
    /// </summary>
    public int CountItem(ItemBase item)
    {
        int quantity = 0;

        foreach (Slot slot in _Slots)
        {
            if (slot.Item == item)
            {
                quantity += slot.Quantity;
            }
        }

        return quantity;
    }

    /// <summary>
    /// Try to stack the items from slot1 into slot2.
    /// Returns true if slot1 is empty by the end.
    /// Returns false if slot1 has some items by the end.
    /// </summary>
    public bool TryStackingItems(Slot slot1, Slot slot2)
    {
        bool isSlotEmpty = true;

        int remainingItems = slot2.Item.MaxStack - slot2.Quantity;
        if (remainingItems < slot1.Quantity)//if slot1 won't be empty
        {
            isSlotEmpty = false;
            slot2.UpdateQuantity(slot2.Item.MaxStack);
        }
        else
        {
            slot2.UpdateQuantity(slot2.Quantity + slot1.Quantity);
        }

        slot1.UpdateQuantity(slot1.Quantity - remainingItems);

        return isSlotEmpty;
    }

    /// <summary>
    /// Find the first available slot in the inventory (empty or holding the same item as item)
    /// </summary>
    public Slot FindFirstSlotAvailable(ItemBase item)
    {
        //search stacks first
        foreach (Slot itemSlot in _Slots)
        {
            if (itemSlot.Item == item && itemSlot.Quantity < itemSlot.Item.MaxStack)
            {
                return itemSlot;
            }
        }

        //search empty slots
        foreach (Slot itemSlot in _Slots)
        {
            if (itemSlot.Item == null)
            {
                return itemSlot;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds if the inventory is full (if there is no empty inventory slot)
    /// </summary>
    public bool IsInventoryFull()
    {
        foreach (Slot itemSlot in _Slots)
        {
            if (itemSlot.Item == null)
            {
                return false;
            }
        }
        return true;
    }
}
