using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Structure
{
    protected override void Update()
    {
        if (!_Inventory.IsInventoryEmpty(InputOrOutput._InputSlots))
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

    protected override bool CallOutput()
    {
        if (_Inventory._InputSlots[0].Quantity == 0)
        {    
            return false;
        }
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
        foreach (Output output in outputs)
        {
            output.PullOutInventory(_Inventory._InputSlots[0].Item, _Inventory._InputSlots[0].Quantity/ outputs.Count,InputOrOutput._InputSlots);
        }
        outputs[0].PullOutInventory(_Inventory._InputSlots[0].Item, _Inventory._InputSlots[0].Quantity % outputs.Count, InputOrOutput._InputSlots);
        return true;
    }
}
