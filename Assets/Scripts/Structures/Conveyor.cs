using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Conveyor : Structure
{
    [SerializeField] private TextMeshProUGUI _amount;

    private void Start()
    {
        UpdateSprite();
        _cooldown = _maxOutputCooldown;
	}
	
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
        foreach (Slot slot in _Inventory._InputSlots)
        {
            if (outputs.Count <= 0)
            {
                return false;
            }
            bool succeded = false;
            foreach (Output output in outputs)
            {
                if (output.PullOutInventory(slot.Item, slot.Quantity/ outputs.Count, InputOrOutput._InputSlots))
                {
                    succeded = true;
                }
            }
            if (outputs[0].PullOutInventory(slot.Item, slot.Quantity % outputs.Count, InputOrOutput._InputSlots))
            {
                succeded = true;
            }
            if (succeded)
            {
                break;
            }
        }
        UpdateSprite();
        return true;
    }

    public override void UpdateSprite()
    {
        if (_Inventory.IsInventoryEmpty(InputOrOutput._InputSlots))
        {
            _amount.SetText("");
            SetSprite(null);
        }
        else
        {
            _amount.SetText(_Inventory._InputSlots[0].Quantity.ToString());
            SetSprite(_Inventory._InputSlots[0].Item.Sprite);
        }
    }
}
