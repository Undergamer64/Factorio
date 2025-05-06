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

    public override void Process()
    {
        if (_Output == null || _Input.IsInventoryEmpty())
            return;
        ItemBase item = _Input._Slots[0].Item;
        if (!_Output.IsInventoryFull(item, 1))
        {
            if (_Output.CanAddItem(item))
            {
                int LeftToAdd = _Output.TryAddItems(item, _Input._Slots[0].Quantity);
                _Input.TryRemoveItems(item, _Input._Slots[0].Quantity - LeftToAdd);
                UpdateSprite();
            }
        }
    }
    
    protected override void Update()
    {
        if (!_Output.IsInventoryEmpty())
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
        if (_Output.IsInventoryEmpty())
        {    
            return false;
        }
        foreach (Slot slot in _Output._Slots)
        {
            bool succeded = _Output.PullOutInventory();
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
        if (_Output.IsInventoryEmpty())
        {
            _amount.SetText("");
            SetSprite(null);
        }
        else
        {
            _amount.SetText(_Output._Slots[0].Quantity.ToString());
            SetSprite(_Output._Slots[0].Item.Sprite);
        }
    }
}
