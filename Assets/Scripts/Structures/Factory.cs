using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Factory : Structure
{
    private float _craftCooldown = 0;
    [SerializeField] protected Recipe _recipe;

    private bool _canCraft = false;
    private int _failedCraftIndex = -1;
    private int _failedCraftQuantity = 0;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        foreach(var item in _recipe._InputItem)
        {
            _Inventory._WhiteListItems.Add(item._Item);
        }
    }


    private void CraftUpdate()
    {
        if (_craftCooldown >= 0)
        {
            _craftCooldown -= Time.deltaTime;
            if (_craftCooldown < 0)
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

    protected override bool CallOutput()
    {
        if (_recipe == null)
        {
            return false;
        }
        bool HasOutputItem = false;
        foreach (ItemsWithQuantity item in _recipe._OutputItem)
        {
            if (_Inventory.CountItem(item._Item) >= 1)
            {
                HasOutputItem = true; break;
            }
        }
        if(HasOutputItem)
        {
            List<Output> outputs = new List<Output>();
            foreach (Output output in _Inventory._Outputs)
            {
                if (output._Input != null)
                {
                    outputs.Add(output);
                }
            }
            foreach (ItemsWithQuantity item in _recipe._OutputItem)
            {
                foreach (Output output in outputs)
                {
                    output.PullOutInventory(item._Item, item._Quantity / outputs.Count);   
                }
                outputs[0].PullOutInventory(item._Item, item._Quantity % outputs.Count);
            }
            return true;
        }
        return false;
    }

    protected override void Update()
    {
        base.Update();
        CraftUpdate();
        CallOutput();
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
        if (_craftCooldown > 0 || _recipe == null) 
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
        _craftCooldown = _recipe._Cooldown;
        _canCraft = false;
        return true;
    }
}
