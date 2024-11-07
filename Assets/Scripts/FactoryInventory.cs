using UnityEngine;

public class FactoryInventory : Inventory
{
    [SerializeField] private Factory Factory;

    public override int TryAddItems(ItemBase item, int quantity = 1)
    {
        Factory.Process();
        return base.TryAddItems(item, quantity);
    }

}