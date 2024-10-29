using UnityEngine;

public class Factory : Structure
{
    [SerializeField] private float Cooldown = 0;
    [SerializeField] private float MaxCooldown;
    [SerializeField] private Recipe Recipe;

    private void Update()
    {
        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
            if (Cooldown <= 0)
            {
                TryToCraft();
            }
        }
    }

    public override void Process()
    {
        TryToCraft();
    }

    public void craft()
    {
        Cooldown = MaxCooldown;
        foreach (var InputItem in Recipe.InputItem)
        {
            _Inventory.TryRemoveItems(InputItem.Item, InputItem.Quantity);
        }
        foreach (var OutputItem in Recipe.OutputItem)
        {
            _Inventory.TryAddItems(OutputItem.Item, OutputItem.Quantity);
        }
    }

    public bool TryToCraft()
    {
        if (Cooldown > 0) 
        {
            return false;
        }
        foreach (var inputItem in Recipe.InputItem)
        {
            if(_Inventory.CountItem(inputItem.Item) < inputItem.Quantity)
            {
                return false;
            }
        }
        craft();
        return true;
    }
}
