using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int _slotNumbers = 0;

    public List<Slot> _Slots = new List<Slot>();
    public List<Input> _Inputs = new List<Input>();
    public List<Output> _Outputs = new List<Output>();

    public List<ItemBase> _WhiteListItems = new List<ItemBase>();
    //public List<ItemBase> _BlackListItems = new List<ItemBase>();


    private void Start()
    {
        for (int i = 0; i < _slotNumbers; i++)
        {
            Slot slot = new Slot();
            _Slots.Add(slot);
        }
    }

    public void EmptyInventory()
    {
        foreach(var slot in _Slots)
        {
            slot.UpdateQuantity(0);
        }
    }

    public bool IsInventoryEmpty()
    {
        foreach(Slot slot in _Slots)
        {
            if (!IsEmpty(slot)) return false;
        }
        return true;
    }

    public bool CanAddItem(ItemBase ItemToADD)
    {
        foreach(ItemBase item in _WhiteListItems)
        {
             if(ItemToADD == item && FindFirstSlotAvailable(ItemToADD) == null) return true;
        }
        return false;
    }

    public void UpdateWhiteList(List<ItemBase> items)
    {
        _WhiteListItems.Clear();
        foreach(var item in items)
        {
            _WhiteListItems.Add(item);
        }
    }

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

        if (IsEmpty(itemSlot))
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
    public int TryRemoveItems(ItemBase item, int quantity)
    {
        int tempQuantity = quantity;
        for (int i = _Slots.Count - 1; i > 0; i--)
        {
            Slot itemSlot = _Slots[i];
            if (tempQuantity <= 0)
            {
                return 0;
            }
            if (IsEmpty(itemSlot))
            {
                continue;
            }
            if (itemSlot.Item == item && itemSlot.Quantity > 0)
            {
                if (tempQuantity > itemSlot.Quantity)
                {
                    tempQuantity -= itemSlot.Quantity;
                    itemSlot.UpdateQuantity(0);
                }
                else
                {
                    itemSlot.UpdateQuantity(itemSlot.Quantity - tempQuantity);
                    return 0;
                }
            }
        }
        return tempQuantity;
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
            if (IsEmpty(itemSlot))
            {
                return itemSlot;
            }
            if (itemSlot.Item == item && itemSlot.Quantity < itemSlot.Item.MaxStack)
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
            if (IsEmpty(itemSlot))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsInventoryFull(ItemBase item, int quantity = 1)
    {
        if (!IsInventoryFull()) 
        { 
            return false; 
        }

        int remainingQuantity = quantity;
        foreach (Slot itemSlot in _Slots)
        {
            if (itemSlot.Item == item && itemSlot.Item.MaxStack < itemSlot.Quantity)
            {
                remainingQuantity = itemSlot.Item.MaxStack - itemSlot.Quantity;
                if (remainingQuantity <= 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool IsEmpty(Slot itemSlot)
    {
        return itemSlot.Item == null;
    }
}
