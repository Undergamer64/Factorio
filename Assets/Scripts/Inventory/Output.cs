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
    /// push item out of inventory
    /// </summary>
    /// <param name="item"> item to push</param>
    /// <param name="quantity">number to push</param>
    /// <returns></returns>
    public bool PullOutInventory(ItemBase item, int quantity) // need to add a split function
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
}
