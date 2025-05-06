using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Output : Inventory
{
    public List<Input> _Inputs = new List<Input>();
    public List<Collider2D> _LinkingPoints = new List<Collider2D>();
    private void Start()
    {
        for (int i = 0; i < _LinkingPoints.Count; i++)
        {
            _Inputs.Add(null);
        }
        FindPartner();
    }
    public void FindPartner()
    {
        foreach (Collider2D col in _LinkingPoints)
        {
            List<Collider2D> Inputs = Physics2D.OverlapBoxAll(col.transform.position, col.bounds.size, 0).ToList();
            foreach (Collider2D collider in Inputs)
            {
                Input input = collider.GetComponentInParent<Input>();
                
                if (input != null)
                {
                    if (input.GetComponentInParent<Structure>().enabled)
                    {
                        if (_Inputs.Contains(input))
                        {
                            break;
                        }
                        _Inputs[_LinkingPoints.IndexOf(col)] = input;
                        input.FindPartner();
                        break;
                    }
                }
            }
        }
    }

    /*public bool Split()
    {
        
    }*/
    
    
    /// <summary>
    /// DEPRECATED
    /// push item out of inventory
    /// </summary>
    /// <param name="item"> item to push</param>
    /// <param name="quantity">number to push</param>
    /// <returns></returns>
    public bool OldPullOutInventory(ItemBase item, int quantity) // need to add a split function
    {
        foreach (Input input in _Inputs)
        {
            if (input == null)
                continue;
            if (!input.IsInventoryFull(item, 1))
            {
                if (input.CanAddItem(item))
                {
                    int LeftToAdd = input.TryAddItems(item, quantity);
                    TryRemoveItems(item, quantity - LeftToAdd);
                    input.GetComponentInParent<Structure>().UpdateSprite();
                    input.GetComponentInParent<Structure>().Process();
                    return true;
                }
            }
        }
        return false;
    } 
    
    /// <summary>
    /// push item out of inventory
    /// </summary>
    /// <returns></returns>
    public bool PullOutInventory() // need to add a split function
    {
        Slot slot = _Slots[FindFirstSlotNonEmpty()];
        
        SplitAndSendOut(slot, slot.Quantity);
        return true;
    } 
    
    /// <summary>
    /// push item out of inventory
    /// </summary>
    /// <returns></returns>
    public void SplitAndSendOut(Slot slot, int quantity) // need to add a split function
    {
        int inputCount = 0;

        foreach (Input input in _Inputs)
        {
            if (input != null && !input.CanAddItem(slot.Item)) inputCount++;
        }
        
        if (inputCount == 0) return;

        int RemainingItems = quantity;
        
        foreach (Input input in _Inputs)
        {
            if (input == null)
                continue;
            if (!input.IsInventoryFull(slot.Item, 1)) // not needed ?
            {
                if (input.CanAddItem(slot.Item))
                {
                    int LeftToAdd = input.TryAddItems(slot.Item, quantity / inputCount);
                    TryRemoveItems(slot.Item, (quantity / inputCount) - LeftToAdd);
                    RemainingItems -= LeftToAdd;
                    input.GetComponentInParent<Structure>()?.UpdateSprite();
                    input.GetComponentInParent<Structure>()?.Process();
                }
            }
        }

        if (RemainingItems <= inputCount)
        {
            if (RemainingItems == 0) return;
            
            foreach (Input input in _Inputs)
            {
                if (input == null)
                    continue;
                if (!input.IsInventoryFull(slot.Item, 1)) // not needed ?
                {
                    if (input.CanAddItem(slot.Item))
                    {
                        int LeftToAdd = input.TryAddItems(slot.Item, RemainingItems);
                        TryRemoveItems(slot.Item, RemainingItems - LeftToAdd);
                        RemainingItems -= LeftToAdd;
                        input.GetComponentInParent<Structure>()?.UpdateSprite();
                        input.GetComponentInParent<Structure>()?.Process();
                        return;
                    }
                }
            }
        }

        SplitAndSendOut(slot, RemainingItems);
    } 
    
    /// <summary>
    /// push item out of inventory into inputs in order of the recipe
    /// </summary>
    /// <returns></returns>
    public bool PullOutInventory(Recipe recipe) // need to add a split function
    {
        for (int i = 0; i < recipe._OutputItem.Count; i++)
        {
            Input input = _Inputs[i];
            if (input == null)
                continue;
            ItemBase item = recipe._OutputItem[i]._Item;
            int quantity = recipe._OutputItem[i]._Quantity;
            if (!input.IsInventoryFull(item, 1))
            {
                if (input.CanAddItem(item))
                {
                    int LeftToAdd = input.TryAddItems(item, quantity);
                    TryRemoveItems(item, quantity - LeftToAdd);
                    input.GetComponentInParent<Structure>()?.UpdateSprite();
                    input.GetComponentInParent<Structure>()?.Process();
                    return true;
                }
            }
        }
        return false;
    } 
}
