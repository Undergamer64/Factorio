using System.Collections.Generic;
using UnityEngine;

public class Factory : Structure
{
    protected float _craftCooldown = 0;
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
        if (_recipe == null)
        {
            return;
        }
        foreach (var item in _recipe._InputItem)
        {
            _Inventory._WhiteListItems.Add(item._Item);
        }
    }


    private void CraftUpdate()
    {
        if (!_canCraft)
        {
            TryToStartCraft();
        }

        if (_canCraft)
        {
            _craftCooldown -= Time.deltaTime;
        }
        if (_craftCooldown < 0)
        {
            if (_failedCraftIndex < 0)
            {
                if (_recipe == null)
                {
                    return;
                }
                foreach (var item in _recipe._InputItem)
                {
                    if (_Inventory.CountItem(item._Item, InputOrOutput._InputSlots) > item._Quantity)
                    {
                        RemoveCraftInput();
                    }
                    else
                    {
                        _canCraft = false;
                        return;
                    }
                }
            }
            TryAddCraftOutput();
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
            if (_Inventory.CountItem(item._Item, InputOrOutput._OutputSlots) >= 1)
            {
                HasOutputItem = true; break;
            }
        }
        if (HasOutputItem)
        {
            List<Output> outputs = new List<Output>();
            foreach (Output output in _Inventory._Outputs)
            {
                if (output._Input != null)
                {
                    outputs.Add(output);
                }
            }
            if (outputs.Count <= 0)
            {
                return false;
            }
            foreach (ItemsWithQuantity item in _recipe._OutputItem)
            {

                foreach (Output output in outputs)
                {
                    output.PullOutInventory(item._Item, item._Quantity / outputs.Count, InputOrOutput._OutputSlots);
                }
                outputs[0].PullOutInventory(item._Item, item._Quantity % outputs.Count, InputOrOutput._OutputSlots);
            }
            return true;
        }
        return false;
    }

    protected override void Update()
    {
        _cooldown -= Time.deltaTime;
        CraftUpdate();
        if (!_Inventory.IsInventoryEmpty(InputOrOutput._OutputSlots) && _cooldown <= 0)
        {
            _cooldown = _maxOutputCooldown;
            CallOutput();
        }
    }

    public override void Process()
    {
        TryToStartCraft();
    }

    private void RemoveCraftInput()
    {
        if (_recipe != null)
        {
            foreach (var InputItem in _recipe._InputItem)
            {
                _Inventory.TryRemoveItems(InputItem._Item, InputItem._Quantity, InputOrOutput._InputSlots);
            }
        }
    }


    public void TryAddCraftOutput()
    {
        if (_recipe != null)
        {
            int temp = _failedCraftIndex;
            if (_failedCraftIndex < 0)
            {
                temp = 0;
            }
            for (int i = temp; i < _recipe._OutputItem.Count; i++)
            {
                int remainingQuantity = 0;
                if (i == _failedCraftIndex)
                {
                    remainingQuantity = _Inventory.TryAddItems(_recipe._OutputItem[i]._Item, _failedCraftQuantity, InputOrOutput._OutputSlots);
                }
                else
                {
                    remainingQuantity = _Inventory.TryAddItems(_recipe._OutputItem[i]._Item, _recipe._OutputItem[i]._Quantity, InputOrOutput._OutputSlots);

                }

                if (remainingQuantity > 0)
                {
                    _failedCraftIndex = i;
                    _failedCraftQuantity = remainingQuantity;
                    return;
                }
            }
            _failedCraftQuantity = 0;
            _failedCraftIndex = -1;
            _canCraft = false;
        }
    }

    public void TryToStartCraft()
    {
        if (_canCraft)
        {
            return;
        }
        if (_craftCooldown > 0 || _recipe == null)
        {
            return;
        }
        foreach (var inputItem in _recipe._InputItem)
        {
            if (_Inventory.CountItem(inputItem._Item, InputOrOutput._InputSlots) < inputItem._Quantity)
            {
                return;
            }
        }
        _canCraft = true;
        _craftCooldown = _recipe._Cooldown;
    }
}
