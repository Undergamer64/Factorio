using UnityEngine;

public class Factory : Structure
{
    private float _cooldown = 0;
    [SerializeField] protected Recipe _recipe;


    private void Update()
    {
        if (_cooldown >= 0)
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0)
            {
                Craft();
                TryToStartCraft();
            }
        }
    }

    public override void Process()
    {
        TryToStartCraft();
    }

    public void Craft()
    {
        if (_recipe != null)
        {

            foreach (var InputItem in _recipe._InputItem)
            {
                _Inventory.TryRemoveItems(InputItem._Item, InputItem._Quantity);
            }
            foreach (var OutputItem in _recipe._OutputItem)
            {
                _Inventory.TryAddItems(OutputItem._Item, OutputItem._Quantity);
            }
        }
    }

    public bool TryToStartCraft()
    {
        if (_cooldown > 0 || _recipe == null) 
        {
            return false;
        }
        foreach (var inputItem in _recipe._InputItem)
        {
            if(_Inventory.CountItem(inputItem._Item) < inputItem._Quantity)
            {
                return false;
            }
        }
        _cooldown = _recipe._Cooldown;
        return true;
    }
}
