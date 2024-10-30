using System.Collections.Generic;

public class Conveyor : Structure
{
    protected override bool CallOutput()
    {
        if (_Inventory._Slots[0].Quantity == 0)
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
        foreach (Output output in outputs)
        {
            output.PullOutInventory(_Inventory._Slots[0].Item, _Inventory._Slots[0].Quantity/ outputs.Count);
        }
        outputs[0].PullOutInventory(_Inventory._Slots[0].Item, _Inventory._Slots[0].Quantity / outputs.Count);
        return true;
    }
}
