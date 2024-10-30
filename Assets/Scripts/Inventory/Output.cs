using UnityEngine;

public class Output : Tunnel
{
    public Input _Input;
    public int pulledItem;

    public bool Pull(ItemBase item,int quantity)
    {
        pulledItem = _ParentInventory.TryRemoveItems(item, quantity);
        if (pulledItem < quantity)
        {
            pulledItem = quantity - pulledItem;
            return true;
        }
        return false;
    }
}
