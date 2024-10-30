using UnityEngine;

public class Factory : Structure
{
    private float _cooldown = 0;
    [SerializeField] protected Recipe _recipe;

    private bool _canCraft = false;
    private int _failedCraftIndex = -1;
    private int _failedCraftQuantity = 0;


    private void Update()
    {
        if (_cooldown >= 0)
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0)
            {
                _canCraft = true;
            }
        }
        if (_canCraft)
        {
            if (_failedCraftIndex > 0)
            {
                RemoveCraftInput();
            }
            TryAddCraftOutput();

            if (_canCraft)
            {
                TryToStartCraft();
            }
        }
    }

    public override void Process()
    {
        _canCraft = true;
    }

    private void RemoveCraftInput()
    {
        if (_recipe != null)
        {
            foreach (var InputItem in _recipe._InputItem)
            {
                _Inventory.TryRemoveItems(InputItem._Item, InputItem._Quantity);
            }
        }
    }


    public void TryAddCraftOutput()
    {
        if (_recipe != null)
        {
            if (_failedCraftIndex < 0)
            {
                _failedCraftIndex = 0;
            }
            for (int i = _failedCraftIndex; i < _recipe._OutputItem.Count; i++)
            {
                int remainingQuantity = 0;
                if (i  == _failedCraftIndex && _failedCraftQuantity != 0)
                {
                    remainingQuantity = _Inventory.TryAddItems(_recipe._OutputItem[i]._Item, _failedCraftQuantity);
                }
                else
                {
                    remainingQuantity = _Inventory.TryAddItems(_recipe._OutputItem[i]._Item, _recipe._OutputItem[i]._Quantity);
                }

                if (remainingQuantity > 0)
                {
                    _canCraft = false;
                    _failedCraftIndex = i;
                    _failedCraftQuantity = remainingQuantity;
                    return;
                }
            }
            _failedCraftQuantity = 0;
            _failedCraftIndex = -1;
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
        _canCraft = false;
        return true;
    }
}
