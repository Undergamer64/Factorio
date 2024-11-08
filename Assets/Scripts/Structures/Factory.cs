using System.Collections.Generic;
using UnityEngine;

public class Factory : Structure
{
    public float _craftCooldown { get; protected set; } = 0;
    public Recipe _Recipe;

    public bool _CanCraft { get; private set; } = false;
    private int _failedCraftIndex = -1;
    private int _failedCraftQuantity = 0;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        if (_Recipe == null)
        {
            return;
        }
        foreach (var item in _Recipe._InputItem)
        {
            _Inventory._WhiteListItems.Add(item._Item);
        }
    }


    private void CraftUpdate()
    {
        if (!_CanCraft)
        {
            TryToStartCraft();
        }

        if (_CanCraft)
        {
            _craftCooldown -= Time.deltaTime;
        }
        if (_craftCooldown < 0)
        {
            if (_failedCraftIndex < 0)
            {
                if (_Recipe == null)
                {
                    return;
                }
                foreach (var item in _Recipe._InputItem)
                {
                    if (_Inventory.CountItem(item._Item, InputOrOutput._InputSlots) > item._Quantity)
                    {
                        RemoveCraftInput();
                    }
                    else
                    {
                        _CanCraft = false;
                        return;
                    }
                }
            }
            TryAddCraftOutput();
        }
    }

    protected override bool CallOutput()
    {
        if (_Recipe == null)
        {
            return false;
        }
        bool HasOutputItem = false;
        foreach (ItemsWithQuantity item in _Recipe._OutputItem)
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
            foreach (ItemsWithQuantity item in _Recipe._OutputItem)
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
        CraftUpdate();
        if (!_Inventory.IsInventoryEmpty(InputOrOutput._OutputSlots))
        {
            _cooldown -= Time.deltaTime;
        }
        if (_cooldown <= 0)
        {
            if (CallOutput())
            {
                _cooldown = _maxOutputCooldown;
            }
        }
    }

    public override void Process()
    {
        TryToStartCraft();
    }

    private void RemoveCraftInput()
    {
        if (_Recipe != null)
        {
            foreach (var InputItem in _Recipe._InputItem)
            {
                _Inventory.TryRemoveItems(InputItem._Item, InputItem._Quantity, InputOrOutput._InputSlots);
            }
        }
    }


    public void TryAddCraftOutput()
    {
        if (_Recipe != null)
        {
            int temp = _failedCraftIndex;
            if (_failedCraftIndex < 0)
            {
                temp = 0;
            }
            for (int i = temp; i < _Recipe._OutputItem.Count; i++)
            {
                int remainingQuantity = 0;
                if (i == _failedCraftIndex)
                {
                    remainingQuantity = _Inventory.TryAddItems(_Recipe._OutputItem[i]._Item, _failedCraftQuantity, InputOrOutput._OutputSlots);
                }
                else
                {
                    remainingQuantity = _Inventory.TryAddItems(_Recipe._OutputItem[i]._Item, _Recipe._OutputItem[i]._Quantity, InputOrOutput._OutputSlots);

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
            _CanCraft = false;
        }
    }

    public void TryToStartCraft()
    {
        if (_craftCooldown > 0 || _Recipe == null)
        {
            return;
        }
        foreach (var inputItem in _Recipe._InputItem)
        {
            if (_Inventory.CountItem(inputItem._Item, InputOrOutput._InputSlots) < inputItem._Quantity)
            {
                return;
            }
        }
        if (_CanCraft)
        {
            return;
        }
        _craftCooldown = _Recipe._Cooldown;
        _CanCraft = true;
    }
}
