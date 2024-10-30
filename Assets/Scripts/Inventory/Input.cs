using UnityEngine;

public class Input : Tunnel
{
    public Output _Output;
    
    
    //check if there's space in inventory
    //if yes try and remove item from partner
    //if yes add item to inventory

    public void FindPartner()
    {
        _Output = Physics2D.OverlapBox(transform.position, transform.localScale, 0).GetComponentInParent<Output>();
        _Output._Input = this;
    }

    public void push(ItemBase item, int quantity)
    {
        if (!_ParentInventory.IsInventoryFull())
        {
            if (_Output.Pull(item, quantity))
            {
                _ParentInventory.TryAddItems(item, _Output.pulledItem);
            }
        }
    }

}
