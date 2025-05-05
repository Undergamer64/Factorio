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
        if (!_Input.IsInventoryEmpty())
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
        if (_Input._Slots[0].Quantity == 0)
        {    
            return false;
        }
        foreach (Slot slot in _Input._Slots)
        {
            bool succeded = _Output.PullOutInventory(slot.Item, slot.Quantity);
            
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
        if (_Input.IsInventoryEmpty())
        {
            _amount.SetText("");
            SetSprite(null);
        }
        else
        {
            _amount.SetText(_Input._Slots[0].Quantity.ToString());
            SetSprite(_Input._Slots[0].Item.Sprite);
        }
    }
}
